using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Web.XmlTransform;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Organization;
using Microsoft.Xrm.Sdk.Query;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using XrmToolBox.Extensibility;

namespace StandardViewCreator
{
    public partial class StandardViewCreatorControl : PluginControlBase
    {
        private Settings mySettings;

        public StandardViewCreatorControl()
        {
            InitializeComponent();

        }

        private void StandardViewCreatorControl_Load(object sender, EventArgs e)
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

            tlpViewOptions_cmbType.SelectedIndex = 0;
            tlpViewOptions_cmbDuplicate.SelectedIndex = 0;
            tlpViewOptions_cmbFilter.SelectedIndex = 0;
            tlpSort_cmbSortValue.SelectedIndex = 0;

            txtName_tip.SetToolTip(tlpViewOptions_txtName, "Available: {entityLogicalName}, {entityDisplayName}, {yyyyMMdd}");
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

            tlpEntity_lvwEntities.Items.Clear();
        }

        string ToSingleLine(string text)
        {
            return Regex.Replace(text, @"\s+", " ").Trim();
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

        private EntityCollection GetUserQuery(bool includeAll, List<EntityInfo> selectedEntities)
        {
            var filterNode = includeAll ? String.Empty : $@"
            <filter>
              <condition attribute='returnedtypecode' operator='in'>
                {String.Join("", selectedEntities.Select(s => $"<value>{s.ObjectTypeCode}</value>").ToArray())}
              </condition>
            </filter>";

            string fetchXml = $@"
            <fetch>
              <entity name='userquery'>
                <attribute name='fetchxml' />
                <attribute name='layoutxml' />
                <attribute name='name' />
                <attribute name='querytype' />
                <attribute name='returnedtypecode' />
                <order attribute='returnedtypecode' />
                <order attribute='name' />
                {filterNode}
              </entity>
            </fetch>";

            return SafeRetrieveMultiple(fetchXml);
        }

        private EntityCollection GetSavedQuery(bool includeAll, List<EntityInfo> selectedEntities)
        {
            var filterNode = includeAll ? String.Empty : $@"
            <filter>
              <condition attribute='returnedtypecode' operator='in'>
                {String.Join("", selectedEntities.Select(s => $"<value>{s.ObjectTypeCode}</value>").ToArray())}
              </condition>
            </filter>";

            string fetchXml = $@"
            <fetch>
              <entity name='savedquery'>
                <attribute name='fetchxml' />
                <attribute name='layoutxml' />
                <attribute name='name' />
                <attribute name='querytype' />
                <attribute name='returnedtypecode' />
                <order attribute='returnedtypecode' />
                <order attribute='name' />
                {filterNode}
              </entity>
            </fetch>";

            return SafeRetrieveMultiple(fetchXml);
        }

        private void ViewExportAll()
        {
            ViewExportToExcel(true);
        }

        private void ViewExportFromSelectedEntities()
        {
            ViewExportToExcel(false);
        }

        private void ViewExportToExcel(bool includeAll)
        {
            var selectedEntities = GetSelectedEntities();

            if (!includeAll)
            {
                if (selectedEntities.Count == 0)
                {
                    MessageBox.Show("At least one entity must be selected.");
                    return;
                }
            }

            List<ViewInfo> views = new List<ViewInfo>();
            List<ViewXmlInfo> xmlnodes = new List<ViewXmlInfo>();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading views...",
                Work = (worker, args) =>
                {
                    var userQueries = GetUserQuery(includeAll, selectedEntities);
                    var savedQueries = GetSavedQuery(includeAll, selectedEntities);

                    views = userQueries.Entities.Select(s => new ViewInfo()
                    {
                        Guid = s.Id,
                        Entity = s.GetAttributeValue<string>("returnedtypecode"),
                        Type = s.LogicalName,
                        Name = s.GetAttributeValue<string>("name"),
                        FetchXml = s.GetAttributeValue<string>("fetchxml"),
                        LayoutXml = s.GetAttributeValue<string>("layoutxml"),
                        QueryType = s.GetAttributeValue<int>("querytype"),
                    }).ToList();

                    views.AddRange(savedQueries.Entities.Select(s => new ViewInfo()
                    {
                        Guid = s.Id,
                        Entity = s.GetAttributeValue<string>("returnedtypecode"),
                        Type = s.LogicalName,
                        Name = s.GetAttributeValue<string>("name"),
                        FetchXml = s.GetAttributeValue<string>("fetchxml"),
                        LayoutXml = s.GetAttributeValue<string>("layoutxml"),
                        QueryType = s.GetAttributeValue<int>("querytype"),
                    }).ToList());

                    foreach (var entity in userQueries.Entities)
                    {
                        var baseInfo = new ViewXmlInfo()
                        {
                            Guid = entity.Id,
                            Entity = entity.GetAttributeValue<string>("returnedtypecode"),
                            Type = entity.LogicalName,
                            QueryType = entity.GetAttributeValue<int>("querytype"),
                            Name = entity.GetAttributeValue<string>("name"),
                        };

                        if (entity.TryGetAttributeValue<string>("fetchxml", out string fetchxml))
                        {
                            if (!String.IsNullOrEmpty(fetchxml))
                            {
                                var xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(fetchxml);
                                XmlNode root = xmlDoc.DocumentElement;

                                baseInfo.Type = entity.LogicalName + ".fetchxml";

                                ExploreXmlRecursively(root, xmlnodes, baseInfo);

                            }
                        }

                        if (entity.TryGetAttributeValue<string>("layoutxml", out string layoutxml))
                        {
                            if (!String.IsNullOrEmpty(layoutxml))
                            {
                                var xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(layoutxml);
                                XmlNode root = xmlDoc.DocumentElement;

                                baseInfo.Type = entity.LogicalName + ".layoutxml";

                                ExploreXmlRecursively(root, xmlnodes, baseInfo);

                            }
                        }

                    }

                    foreach (var entity in savedQueries.Entities)
                    {
                        var baseInfo = new ViewXmlInfo()
                        {
                            Guid = entity.Id,
                            Entity = entity.GetAttributeValue<string>("returnedtypecode"),
                            Type = entity.LogicalName,
                            QueryType = entity.GetAttributeValue<int>("querytype"),
                            Name = entity.GetAttributeValue<string>("name"),
                        };

                        if (entity.TryGetAttributeValue<string>("fetchxml", out string fetchxml))
                        {
                            if (!String.IsNullOrEmpty(fetchxml))
                            {
                                var xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(fetchxml);
                                XmlNode root = xmlDoc.DocumentElement;

                                baseInfo.Type = entity.LogicalName + ".fetchxml";

                                ExploreXmlRecursively(root, xmlnodes, baseInfo);

                            }
                        }

                        if (entity.TryGetAttributeValue<string>("layoutxml", out string layoutxml))
                        {
                            if (!String.IsNullOrEmpty(layoutxml))
                            {
                                var xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(layoutxml);
                                XmlNode root = xmlDoc.DocumentElement;

                                baseInfo.Type = entity.LogicalName + ".layoutxml";

                                ExploreXmlRecursively(root, xmlnodes, baseInfo);

                            }
                        }

                    }
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    using (SaveFileDialog dlg = new SaveFileDialog())
                    {
                        dlg.Filter = "Excel file(*.xlsx)|*.xlsx";

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            ExcelPackage.License.SetNonCommercialPersonal("mkdev");
                            using (var package = new ExcelPackage())
                            {
                                var workbook = package.Workbook;

                                var worksheet1 = workbook.Worksheets.Add("Views");

                                var range1 = worksheet1.Cells["A1"].LoadFromCollection(views, c =>
                                {
                                    c.PrintHeaders = true;
                                    c.TableStyle = TableStyles.Light2;
                                });

                                var worksheet2 = workbook.Worksheets.Add("ViewNodes");

                                var range2 = worksheet2.Cells["A1"].LoadFromCollection(xmlnodes, c =>
                                {
                                    c.PrintHeaders = true;
                                    c.TableStyle = TableStyles.Light2;
                                });

                                package.SaveAs(new FileInfo(dlg.FileName));
                            }

                            DialogResult result = MessageBox.Show("The file has been saved successfully. Do you want to open it?", "Successfully", MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                            {
                                Process.Start(new ProcessStartInfo(dlg.FileName) { UseShellExecute = true });
                            }
                        }
                    }
                }
            });
        }

        private void ExploreXmlRecursively(XmlNode node, List<ViewXmlInfo> xmls, ViewXmlInfo baseInfo)
        {

            WriteNodeValue(node, xmls, baseInfo);

            foreach (XmlNode childNode in node.ChildNodes)
            {
                ExploreXmlRecursively(childNode, xmls, baseInfo);
            }
        }

        private void WriteNodeValue(XmlNode node, List<ViewXmlInfo> xmls, ViewXmlInfo baseInfo)
        {
            var nodeAttributes = new List<(string Name, string Value)>();

            if (node.Attributes != null)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    nodeAttributes.Add((attribute.LocalName, attribute.Value));
                }
            }

            xmls.Add(new ViewXmlInfo()
            {
                Guid = baseInfo.Guid,
                Entity = baseInfo.Entity,
                Type = baseInfo.Type,
                QueryType = baseInfo.QueryType,
                Name = baseInfo.Name,
                ParentNode = node.ParentNode?.LocalName,
                NodeName = node.LocalName,
                NodeValue = ValidateEmptyValue(node.Value),
                NodeAttributes = "{" + $"{String.Join(",", nodeAttributes.Select(s => $"\"{s.Name}\": \"{s.Value}\"").ToArray())}" + "}",
            });
        }

        private string ValidateEmptyValue(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return "{value is empty}";
            }
            else
            {
                return value;
            }
        }

        private void GetEntitiesFromSolution(string solutionId)
        {
            var solutionComponents = GetSolutionComponents(solutionId);

            if (solutionComponents.Entities.Count > 0)
            {
                WorkAsync(new WorkAsyncInfo
                {
                    // Access form components inside PostWorkCallBack to avoid potential runtime errors.

                    Message = "Get entities from solution.",
                    Work = (worker, args) =>
                    {
                        var entityMetadataIds = solutionComponents.Entities
                            .Select(s => s.GetAttributeValue<Guid>("objectid"))
                            .ToHashSet();

                        RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest
                        {
                            EntityFilters = EntityFilters.Entity,
                        };
                        RetrieveAllEntitiesResponse allEntities = null;

                        try
                        {
                            allEntities = (RetrieveAllEntitiesResponse)Service.Execute(request);
                        }
                        catch (FaultException<OrganizationServiceFault> fault)
                        {
                            MessageBox.Show(
                                $"An error occurred while executing request:\n{fault.Detail.Message}",
                                "CRM Execute Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                $"An unexpected error occurred:\n{ex.Message}",
                                "Unexpected Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

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

                        args.Result = Entities;

                    },
                    PostWorkCallBack = (args) =>
                    {
                        var Entities = args.Result as List<EntityInfo>;

                        tlpEntity_lvwEntities.Items.Clear();

                        foreach (var entity in Entities)
                        {
                            var item = new System.Windows.Forms.ListViewItem(entity.ToStringArray());
                            item.Tag = entity;
                            tlpEntity_lvwEntities.Items.Add(item);
                        }
                    }
                });
                
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

        private Guid SafeCreate(Entity entity)
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

        private List<EntityInfo> GetSelectedEntities()
        {
            var selectedEntities = tlpEntity_lvwEntities.Items.Cast<ListViewItem>()
                .Where(w => w.Checked == true && ((EntityInfo)w.Tag).IsValidForAdvancedFind == true)
                .Select(s => (EntityInfo)s.Tag)
                .ToList();

            return selectedEntities;
        }

        private void CreateView()
        {
            if (String.IsNullOrWhiteSpace(tlpViewOptions_txtName.Text))
            {
                MessageBox.Show("Name is required.");
                tlpViewOptions_txtName.Focus();
                return;
            }

            ListView listViewColumns = tlpColumn_lvwColumns;

            var selectedEntities = GetSelectedEntities();

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
                            ["fetchxml"] = ToSingleLine(fetchxml),
                            ["layoutxml"] = ToSingleLine(layoutxml),
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

            foreach (ListViewItem selectedItem in listView.SelectedItems)
            {
                if (selectedItem.SubItems.Count > 3)
                {
                    selectedItem.SubItems[3].Text = selectedValue;
                }
            }
        }

        private void tsmMain_btnClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void tsmMain_btnLoadEntitiesFromSolution_Click(object sender, EventArgs e)
        {
            ExecuteMethod(PickSolution);
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

        private void tlpColumn_lvwColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tlpColumn_lvwColumns.SelectedItems.Count > 1)
            {
                tlpPosition_btnUp.Enabled = false;
                tlpPosition_btnDown.Enabled = false;
                tlpSort_btnSet.Enabled = false;
                tlpSort_btnClear.Enabled = false;
            }
            else
            {
                tlpPosition_btnUp.Enabled = true;
                tlpPosition_btnDown.Enabled = true;
                tlpSort_btnSet.Enabled = true;
                tlpSort_btnClear.Enabled = true;
            }
        }

        private void tlpColumn_lvwColumns_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void ddbExport_itemAll_Click(object sender, EventArgs e)
        {
            ExecuteMethod(ViewExportAll);
        }

        private void ddbExport_itemSelectedEntities_Click(object sender, EventArgs e)
        {
            ExecuteMethod(ViewExportFromSelectedEntities);
        }
    }
}