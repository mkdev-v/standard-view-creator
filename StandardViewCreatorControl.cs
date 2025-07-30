using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Xml.Linq;
using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Organization;
using Microsoft.Xrm.Sdk.Query;
using XrmToolBox.Extensibility;

namespace StandardViewCreator
{
    public partial class StandardViewCreatorControl : PluginControlBase
    {
        private Settings mySettings;

        public StandardViewCreatorControl()
        {
            InitializeComponent();

            tlpViewOptions_cmbType.SelectedIndex = 0;
            tlpViewOptions_cmbDuplicate.SelectedIndex = 0;
            tlpViewOptions_cmbFilter.SelectedIndex = 0;
            tlpSort_cmbSortValue.SelectedIndex = 0;

            txtName_tip.SetToolTip(tlpViewOptions_txtName, "Available: {entityLogicalName}, {entityDisplayName}, {yyyyMMdd}");
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            // ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        private void tsmMain_btnClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private EntityCollection SafeRetrieveMultiple(string fetchXml)
        {
            try
            {
                return Service.RetrieveMultiple(new FetchExpression(fetchXml));
            }
            catch (FaultException<OrganizationServiceFault> fault)
            {
                // ShowErrorDialog(fault);
                MessageBox.Show(
                    $"An error occurred while retrieving data:\n{fault.Detail.Message}",
                    "CRM RetrieveMultiple Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // ShowErrorDialog(ex);
                MessageBox.Show(
                    $"An unexpected error occurred:\n{ex.Message}",
                    "Unexpected Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return new EntityCollection();
        }

        private EntityCollection GetSolutions()
        {
            string fetchXml = $@"
            <fetch>
              <entity name='solution'>
                <attribute name='friendlyname' />
                <attribute name='publisherid' />
                <attribute name='version' />
                <order attribute='createdon' descending='true' />
                <filter type='and'>
                  <condition attribute='isvisible' operator='eq' value='1' />
                  <condition attribute='uniquename' operator='ne' value='Default' />
                </filter>
              </entity>
            </fetch>";

            return SafeRetrieveMultiple(fetchXml);
        }

        private EntityCollection GetSolutionComponents(string solutionId)
        {
            string fetchXml = $@"
            <fetch>
              <entity name='solutioncomponent'>
                <attribute name='objectid' />
                <filter type='and'>
                  <condition attribute='solutionid' operator='eq' value='{solutionId}' />
                  <condition attribute='componenttype' operator='eq' value='1' />
                </filter>
              </entity>
            </fetch>";

            return SafeRetrieveMultiple(fetchXml);
        }

        private void GetEntitiesFromSolution(string solutionId)
        {
            var solutionComponents = GetSolutionComponents(solutionId);

            if (solutionComponents.Entities.Count > 0)
            {
                var entityMetadataIds = solutionComponents.Entities
                    .Select(s => s.GetAttributeValue<Guid>("objectid"))
                    .ToHashSet();

                RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest
                {
                    EntityFilters = EntityFilters.Entity,
                };
                RetrieveAllEntitiesResponse allEntities = (RetrieveAllEntitiesResponse)Service.Execute(request);

                var Entities = allEntities.EntityMetadata
                    .Where(w => entityMetadataIds.Contains((Guid)w.MetadataId))
                    .Select(s => new EntityInfo
                    {
                        DisplayName = s.DisplayName.UserLocalizedLabel.Label,
                        LogicalName = s.LogicalName,
                        PrimaryNameAttribute = s.PrimaryNameAttribute,
                        IsValidForAdvancedFind = s.IsValidForAdvancedFind,
                        ObjectTypeCode = s.ObjectTypeCode,
                        PrimaryIdAttribute = s.PrimaryIdAttribute,
                    })
                    .OrderBy(o => o.LogicalName)
                    .ToList();

                tlpEntity_lvwEntities.Items.Clear();

                foreach (var entity in Entities)
                {
                    var item = new System.Windows.Forms.ListViewItem(entity.ToStringArray());
                    item.Tag = entity;
                    tlpEntity_lvwEntities.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No entities found in solution.");
                return;
            }
        }

        private SortInfo GetSortInfo(string logicalName, string sortValue)
        {
            int priority = int.Parse(sortValue[2].ToString());
            bool descending = sortValue[0].ToString() == "↓";

            return new SortInfo()
            {
                LogicalName = logicalName,
                Priority = priority,
                Descending = descending.ToString().ToLower()
            };
        }

        public Guid SafeCreate(Entity entity)
        {
            try
            {
                return Service.Create(entity);
            }
            catch (FaultException<OrganizationServiceFault> fault)
            {
                MessageBox.Show(
                    $"An error occurred while creating a record:\n{fault.Detail.Message}",
                    "CRM Create Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An unexpected error occurred:\n{ex.Message}",
                    "Unexpected Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return Guid.Empty;
        }

        private void CreateView()
        {
            if (String.IsNullOrWhiteSpace(tlpViewOptions_txtName.Text))
            {
                MessageBox.Show("Name is required.");
                tlpViewOptions_txtName.Focus();
                return;
            }

            ListView listViewEntities = tlpEntity_lvwEntities;
            ListView listViewColumns = tlpColumn_lvwColumns;

            var selectedEntities = listViewEntities.Items.Cast<ListViewItem>()
                .Where(w => w.Checked == true && ((EntityInfo)w.Tag).IsValidForAdvancedFind == true)
                .Select(s => (EntityInfo)s.Tag)
                .ToList();

            if (selectedEntities.Count == 0)
            {
                MessageBox.Show("At least one entity must be selected.");
                return;
            }

            var selectedColumns = listViewColumns.Items.Cast<ListViewItem>()
                .Where(w => w.Checked == true)
                .Select(s => new ColumnInfo()
                {
                    DisplayName = s.Text,
                    LogicalName = s.SubItems[1].Text,
                    Sorting = s.SubItems[2].Text,
                    Width = s.SubItems[3].Text,
                })
                .ToList();

            if (selectedColumns.Count == 0)
            {
                MessageBox.Show("At least one column must be selected.");
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Creating view...",
                Work = (worker, args) =>
                {
                    string formatAttribute = "<attribute name='{0}' />";
                    string formatOrder = "<order attribute='{0}' descending='{1}' />";
                    string formatCell = "<cell name='{0}' width='{1}' />";

                    // string formatViewName = "Audit Monitoring ({0})";

                    // Available: {entityLogicalName}, {entityDisplayName}, {yyyyMMdd}
                    string viewName = tlpViewOptions_txtName.Text;

                    string formatPrimaryName = "{primaryAttribute}";
                    string formatEntityLogicalName = "{entityLogicalName}";
                    string formatEntityDisplayName = "{entityDisplayName}";
                    string formatDate = "{yyyyMMdd}";

                    var results = new HashSet<Guid>();

                    foreach (var entity in selectedEntities)
                    {
                        var entityObjectTypeCode = entity.ObjectTypeCode;
                        var entityPrimaryName = entity.PrimaryNameAttribute;
                        var entityPrimaryId = entity.PrimaryIdAttribute;
                        var entityLocicalName = entity.LogicalName;

                        RetrieveEntityRequest request = new RetrieveEntityRequest
                        {
                            EntityFilters = EntityFilters.Attributes,
                            LogicalName = entityLocicalName
                        };
                        RetrieveEntityResponse response = (RetrieveEntityResponse)Service.Execute(request);

                        var attributes = response.EntityMetadata.Attributes
                            .Select(s => s.LogicalName)
                            .ToArray();

                        var columnSet = selectedColumns
                            .Where(w => w.LogicalName == formatPrimaryName || attributes.Contains(w.LogicalName))
                            .ToList();

                        var sortSet = columnSet
                            .Where(w => !String.IsNullOrEmpty(w.Sorting))
                            .Select(s => GetSortInfo(s.LogicalName, s.Sorting))
                            .OrderBy(o => o.Priority)
                            .ToList();

                        var nodeAttribute = String.Join(String.Empty, columnSet
                            .Select(s => String.Format(formatAttribute, s.LogicalName))).Replace(formatPrimaryName, entityPrimaryName);

                        var nodeOrder = String.Join(String.Empty, sortSet
                            .Select(s => String.Format(formatOrder, s.LogicalName, s.Descending))).Replace(formatPrimaryName, entityPrimaryName); ;

                        var nodeCell = String.Join(String.Empty, columnSet
                            .Select(s => String.Format(formatCell, s.LogicalName, s.Width))).Replace(formatPrimaryName, entityPrimaryName);
                        
                        string fetchxml = $@"
                        <fetch>
                          <entity name='{entityLocicalName}'>
                            {nodeAttribute}
                            {nodeOrder}
                          </entity>
                        </fetch>
                        ";

                        string layoutxml = $@"
                        <grid name='resultset' object='{entityObjectTypeCode}' jump='{entityPrimaryName}' select='1' icon='1' preview='1'>
                          <row name='result' id='{entityPrimaryId}'>
                            {nodeCell}
                          </row>
                        </grid>
                        ";

                        string yyyymmdd = DateTime.Now.ToString("yyyyMMdd");
                        var formattedViewName = viewName
                            .Replace(formatEntityDisplayName, entity.DisplayName)
                            .Replace(formatEntityLogicalName, entity.LogicalName)
                            .Replace(formatDate, yyyymmdd);

                        Entity create = new Entity("userquery")
                        {
                            ["name"] = formattedViewName,
                            ["fetchxml"] = fetchxml,
                            ["layoutxml"] = layoutxml,
                            ["returnedtypecode"] = entityObjectTypeCode,
                            ["querytype"] = 0,
                        };

                        var id = SafeCreate(create);

                        if (id != Guid.Empty)
                        {
                            results.Add(id);
                        }

                    }

                    args.Result = results;

                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    var results = args.Result as HashSet<Guid>;
                    if (results != null)
                    {
                        MessageBox.Show($"View created.");
                    }
                }
            });
        }

        private void PickSolution()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Get solutions. ",
                Work = (worker, args) =>
                {
                    var solutions = GetSolutions();

                    args.Result = solutions;
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    var solutions = args.Result as EntityCollection;

                    if (solutions.Entities.Count > 0)
                    {
                        var items = solutions.Entities
                            .Select(s => new SolutionInfo
                            {
                                SolutionId = s.Id.ToString(),
                                DisplayName = s.GetAttributeValue<string>("friendlyname"),
                                Version = s.GetAttributeValue<string>("version"),
                                Publisher = s.GetAttributeValue<EntityReference>("publisherid")?.Name
                            })
                            .ToList();

                        using (var form = new SolutionSelecter(items))
                        {
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                var selected = form.SelectedItem;
                                GetEntitiesFromSolution(selected.SolutionId);
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        private void tsmMain_btnLoadEntitiesFromSolution_Click(object sender, EventArgs e)
        {
            ExecuteMethod(PickSolution);
        }

        private void CheckAllItems(ListView listView)
        {
            foreach (ListViewItem item in listView.Items)
            {
                item.Checked = true;
            }
        }

        private void UncheckAllItems(ListView listView)
        {
            foreach (ListViewItem item in listView.Items)
            {
                item.Checked = false;
            }
        }

        private void MoveSelectedItemUp(ListView listView)
        {
            if (listView.SelectedItems.Count == 0)
                return;

            ListViewItem selectedItem = listView.SelectedItems[0];
            int index = selectedItem.Index;

            if (index > 0)
            {
                listView.Items.RemoveAt(index);
                listView.Items.Insert(index - 1, selectedItem);
                selectedItem.Selected = true;
                selectedItem.Focused = true;
            }
        }

        private void MoveSelectedItemDown(ListView listView)
        {
            if (listView.SelectedItems.Count == 0)
                return;

            ListViewItem selectedItem = listView.SelectedItems[0];
            int index = selectedItem.Index;

            if (index < listView.Items.Count - 1)
            {
                listView.Items.RemoveAt(index);
                listView.Items.Insert(index + 1, selectedItem);
                selectedItem.Selected = true;
                selectedItem.Focused = true;
            }
        }

        private void SetSortValue(ListView listView, ComboBox comboBox)
        {
            if (listView.SelectedItems.Count == 0)
            {
                return;
            }

            string selectedValue = comboBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedValue))
            {
                return;
            }

            ClearSamePriority(listView, selectedValue);

            ListViewItem selectedItem = listView.SelectedItems[0];

            if (selectedItem.SubItems.Count > 2)
            {
                selectedItem.SubItems[2].Text = selectedValue;
            }
        }

        private void ClearSortValue(ListView listView, ComboBox comboBox)
        {
            if (listView.SelectedItems.Count == 0)
            {
                return;
            }

            ListViewItem selectedItem = listView.SelectedItems[0];

            if (selectedItem.SubItems.Count > 2)
            {
                selectedItem.SubItems[2].Text = null;
            }
        }

        private void ClearSamePriority(ListView listView, string selectedValue)
        {
            var sortValues = listView.Items.Cast<ListViewItem>()
                .Where(w => !String.IsNullOrEmpty(w.SubItems[2].Text))
                .Select(s => s.SubItems[2].Text)
                .ToList();

            if (sortValues.Count == 0)
            {
                return;
            }

            int selectedPriority = int.Parse(selectedValue[2].ToString());

            foreach (ListViewItem item in listView.Items)
            {
                if (String.IsNullOrEmpty(item.SubItems[2].Text))
                {
                    continue;
                }

                int itemPriority = int.Parse(item.SubItems[2].Text[2].ToString());

                if (itemPriority == selectedPriority)
                {
                    item.SubItems[2].Text = null;
                }
            }
        }

        private void SetWidthValue(ListView listView, NumericUpDown numericUpDown)
        {
            if (listView.SelectedItems.Count == 0)
                return;

            string selectedValue = numericUpDown.Value.ToString();
            if (string.IsNullOrEmpty(selectedValue))
                return;

            ListViewItem selectedItem = listView.SelectedItems[0];

            if (selectedItem.SubItems.Count > 3)
            {
                selectedItem.SubItems[3].Text = selectedValue;
            }
        }


        private void tlpSelect_btnSelectAll_Click(object sender, EventArgs e)
        {
            CheckAllItems(tlpEntity_lvwEntities);
        }

        private void tlpSelect_btnUnselectAll_Click(object sender, EventArgs e)
        {
            UncheckAllItems(tlpEntity_lvwEntities);
        }

        private void tlpPosition_btnUp_Click(object sender, EventArgs e)
        {
            MoveSelectedItemUp(tlpColumn_lvwColumns);
        }

        private void tlpPosition_btnDown_Click(object sender, EventArgs e)
        {
            MoveSelectedItemDown(tlpColumn_lvwColumns);
        }

        private void tlpSort_cbxSortValue_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tlpSort_btnSet_Click(object sender, EventArgs e)
        {
            SetSortValue(tlpColumn_lvwColumns, tlpSort_cmbSortValue);
        }

        private void tlpWidth_sudWidthValue_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tlpWidth_btnSet_Click(object sender, EventArgs e)
        {
            SetWidthValue(tlpColumn_lvwColumns, tlpWidth_sudWidthValue);
        }

        private void tlpProcess_btnCreate_Click(object sender, EventArgs e)
        {
            ExecuteMethod(CreateView);
        }

        private void tlpSort_btnClear_Click(object sender, EventArgs e)
        {
            ClearSortValue(tlpColumn_lvwColumns, tlpSort_cmbSortValue);
        }
    }
}