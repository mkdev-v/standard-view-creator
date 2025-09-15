using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
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
using StandardViewCreator.Models;

namespace StandardViewCreator
{
    public partial class StandardViewCreatorControl : PluginControlBase
    {
        private Settings mySettings;

        private DataTable nodes = new DataTable();

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

            // Set default value for combo box
            tlpViewOptions_cmbType.SelectedIndex = 0;
            tlpViewOptions_cmbOverwrite.SelectedIndex = 0;
            tlpViewOptions_cmbFilter.SelectedIndex = 0;
            tlpOrder_cmbOrder.SelectedIndex = 0;

            // Set tooltip
            txtName_tip.SetToolTip(tlpViewOptions_lblName, "Available: {entityLogicalName}, {entityDisplayName}, {yyyyMMdd}");
            cmbOverwrite_tip.SetToolTip(tlpViewOptions_lblOverwrite, "Update mode unavailable. Running in create mode.");
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

        private Entity SafeRetrieve(string entityName, Guid guid, ColumnSet columnSet)
        {
            try
            {
                return Service.Retrieve(entityName, guid, columnSet);
            }
            catch (FaultException<OrganizationServiceFault> fault)
            {
                // ShowErrorDialog(fault);
                MessageBox.Show(
                    $"An error occurred while retrieving data:\n{fault.Detail.Message}",
                    "CRM Retrieve Error",
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

            return new Entity();
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

        private EntityCollection GetUserQueryForExport(List<EntityModel> selectedEntities)
        {
            var filterNode = selectedEntities.Count == 0 ? String.Empty : $@"
            <filter>
              <condition attribute='returnedtypecode' operator='in'>
                {String.Join(String.Empty, selectedEntities.Select(s => $"<value>{s.ObjectTypeCode}</value>").ToArray())}
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

        private List<Entity> GetSavedQueryForExport(List<EntityModel> selectedEntities)
        {
            var queries = new List<Entity>();

            string pagingCookie = String.Empty;
            int fetchCount = 5000; 
            int pageNumber = 1;

            string filterNode = selectedEntities.Count == 0 ? String.Empty : $@"
            <filter>
              <condition attribute='returnedtypecode' operator='in'>
                {String.Join(String.Empty, selectedEntities.Select(s => $"<value>{s.ObjectTypeCode}</value>").ToArray())}
              </condition>
            </filter>";

            while (true)
            {
                string fetchXml = $@"
                <fetch page='{pageNumber}' count='{fetchCount}'>
                  <entity name='savedquery'>
                    <attribute name='fetchxml' />
                    <attribute name='layoutxml' />
                    <attribute name='name' />
                    <attribute name='querytype' />
                    <attribute name='isdefault' />
                    <attribute name='returnedtypecode' />
                    <order attribute='returnedtypecode' />
                    <order attribute='querytype' />
                    <order attribute='name' />
                    {filterNode}
                  </entity>
                </fetch>";

                XElement fetchNode = XElement.Parse(fetchXml);
                if (!String.IsNullOrEmpty(pagingCookie))
                {
                    fetchNode.SetAttributeValue("paging-cookie", pagingCookie);
                }

                var savedQueries = SafeRetrieveMultiple(fetchNode.ToString());

                if (savedQueries.Entities.Count > 0)
                {
                    queries.AddRange(savedQueries.Entities.ToList());
                }

                if (savedQueries.MoreRecords)
                {
                    pageNumber++;
                    pagingCookie = savedQueries.PagingCookie;
                }
                else
                {
                    break;
                }
            }

            return queries;
        }

        private EntityCollection GetUserQueryForPick(List<EntityModel> selectedEntities)
        {
            string fetchXml = $@"
            <fetch>
              <entity name='userquery'>
                <attribute name='name' />
                <attribute name='returnedtypecode' />
                <order attribute='returnedtypecode' />
                <order attribute='name' />
                <filter>
                  <condition attribute='returnedtypecode' operator='in'>
                    {String.Join(String.Empty, selectedEntities.Select(s => $"<value>{s.ObjectTypeCode}</value>").ToArray())}
                  </condition>
                  <condition attribute='querytype' operator='eq' value='0' />
                </filter>
              </entity>
            </fetch>";

            return SafeRetrieveMultiple(fetchXml);
        }

        private EntityCollection GetSavedQueryForPick(List<EntityModel> selectedEntities)
        {
            string fetchXml = $@"
            <fetch>
              <entity name='savedquery'>
                <attribute name='name' />
                <attribute name='returnedtypecode' />
                <order attribute='returnedtypecode' />
                <order attribute='name' />
                <filter>
                  <condition attribute='returnedtypecode' operator='in'>
                    {String.Join(String.Empty, selectedEntities.Select(s => $"<value>{s.ObjectTypeCode}</value>").ToArray())}
                  </condition>
                  <condition attribute='querytype' operator='eq' value='0' />
                </filter>
              </entity>
            </fetch>";

            return SafeRetrieveMultiple(fetchXml);
        }

        private void DuplicateAsPublicView()
        {
            DuplicateView(true, false);
        }

        private void DuplicateAsUserView()
        {
            DuplicateView(false, true);
        }

        private void DuplicateView(bool asPublicView, bool asUserView)
        {
            var selectedEntities = GetSelectedEntities();

            if (selectedEntities.Count == 0)
            {
                MessageBox.Show("At least one entity must be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var queries = new List<Entity>();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading views...",
                Work = (worker, args) =>
                {
                    // System views
                    var savedQueries = GetSavedQueryForPick(selectedEntities);

                    if (savedQueries.Entities.Count > 0)
                    {
                        queries.AddRange(savedQueries.Entities.ToList());
                    }

                    // Personal views
                    var userQueries = GetUserQueryForPick(selectedEntities);

                    if (userQueries.Entities.Count > 0)
                    {
                        queries.AddRange(userQueries.Entities.ToList());
                    }
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (queries.Count > 0)
                    {
                        var items = queries
                            .Select(s => new ViewModel
                            {
                                Guid = s.Id,
                                Entity = s.GetAttributeValue<string>("returnedtypecode"),
                                Type = s.LogicalName,
                                Name = s.GetAttributeValue<string>("name"),
                            })
                            .ToList();

                        using (var form = new ViewSelector(items))
                        {
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                WorkAsync(new WorkAsyncInfo
                                {
                                    Message = "Duplicating view...",
                                    Work = (worker, args2) =>
                                    {
                                        var selected = form.SelectedItem;

                                        var query = SafeRetrieve(selected.Type, selected.Guid, new ColumnSet(true));

                                        if (query.Id == null)
                                        {
                                            return;
                                        }

                                        var create = new Entity()
                                        {
                                            ["name"] = query.GetAttributeValue<string>("name"),
                                            ["fetchxml"] = query.GetAttributeValue<string>("fetchxml"),
                                            ["layoutxml"] = query.GetAttributeValue<string>("layoutxml"),
                                            ["returnedtypecode"] = query.GetAttributeValue<string>("returnedtypecode"),
                                            ["querytype"] = 0,
                                        };

                                        if (asPublicView)
                                        {
                                            create.LogicalName = "savedquery";
                                            var id = SafeCreate(create);
                                        }

                                        if (asUserView)
                                        {
                                            create.LogicalName = "userquery";
                                            var id = SafeCreate(create);
                                        }
                                    },
                                    PostWorkCallBack = (args2) =>
                                    {
                                        MessageBox.Show("View duplicated.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                });
                                
                            }
                        }
                    }
                }
            });
        }

        private void ExcelExportFromLoadedXml()
        {
            using (var dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Filter = "XML file(*.xml)|*.xml";

                if (dlgOpen.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(dlgOpen.FileName); // Load from a XML file
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load XML: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                XmlNode root = xmlDoc.DocumentElement;

                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Loading xml...",
                    Work = (worker, args) =>
                    {
                        nodes.Reset();
                        // Core columns
                        nodes.Columns.Add("NodeURI");
                        nodes.Columns.Add("ParentNode");
                        nodes.Columns.Add("NodeName");
                        nodes.Columns.Add("InnerXml");
                        nodes.Columns.Add("NodeValue");
                        nodes.Columns.Add("NodeAttributes");

                        ExploreXmlRecursively(root);
                    },
                    PostWorkCallBack = (args) =>
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        using (var dlgSave = new SaveFileDialog())
                        {
                            dlgSave.Filter = "Excel file(*.xlsx)|*.xlsx";

                            if (dlgSave.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }

                            ExcelPackage.License.SetNonCommercialPersonal("mkdev");
                            using (var package = new ExcelPackage())
                            {
                                var wb = package.Workbook;

                                // Xml node
                                var ws = wb.Worksheets.Add("Xml node");
                                ws.Cells["A1"].Value = dlgOpen.FileName;
                                var range = ws.Cells["A3"].LoadFromDataTable(nodes, c =>
                                {
                                    c.PrintHeaders = true;
                                    c.TableStyle = TableStyles.Light8;
                                });

                                for (int i = 1; i <= range.Columns; i++)
                                {
                                    ws.Column(i).Width = 12;
                                }

                                package.SaveAs(new FileInfo(dlgSave.FileName));
                            }

                            DialogResult result = MessageBox.Show("The file has been saved successfully.\r\nDo you want to open it?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                Process.Start(new ProcessStartInfo(dlgSave.FileName) { UseShellExecute = true });
                            }
                        }
                    }
                });
            }
        }

        private void ViewExportAll()
        {
            DialogResult result = MessageBox.Show("This may take a few seconds.\r\nDo you want to continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ViewExportBase(true);
            }
        }

        private void ViewExportFromSelectedEntities()
        {
            ViewExportBase(false);
        }

        private void ViewExportBase(bool includeAll)
        {
            var selectedEntities = new List<EntityModel>();

            if (!includeAll)
            {
                selectedEntities = GetSelectedEntities();

                if (selectedEntities.Count == 0)
                {
                    MessageBox.Show("At least one entity must be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var views = new List<ViewModel>();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading views...",
                Work = (worker, args) =>
                {
                    var queries = new List<Entity>();

                    // System views: Support for more than 5000 records
                    var savedQueries = GetSavedQueryForExport(selectedEntities);

                    if (savedQueries.Count > 0)
                    {
                        queries.AddRange(savedQueries);
                    }

                    // Personal views
                    var userQueries = GetUserQueryForExport(selectedEntities);

                    if (userQueries.Entities.Count > 0)
                    {
                        queries.AddRange(userQueries.Entities.ToList());
                    }

                    nodes.Reset();
                    // Option columns
                    nodes.Columns.Add("Guid");
                    nodes.Columns.Add("Entity");
                    nodes.Columns.Add("Type");
                    nodes.Columns.Add("QueryType");
                    nodes.Columns.Add("IsDefault");
                    nodes.Columns.Add("Name");
                    nodes.Columns.Add("XmlType");
                    // Core columns
                    nodes.Columns.Add("NodeURI");
                    nodes.Columns.Add("ParentNode");
                    nodes.Columns.Add("NodeName");
                    nodes.Columns.Add("InnerXml");
                    nodes.Columns.Add("NodeValue");
                    nodes.Columns.Add("NodeAttributes");

                    // View node
                    foreach (var entity in queries)
                    {
                        var view = new ViewModel()
                        {
                            Guid = entity.Id,
                            Type = entity.LogicalName,
                            Entity = entity.GetAttributeValue<string>("returnedtypecode"),
                            QueryType = GetQueryTypeLabel(entity.GetAttributeValue<int>("querytype")),
                            IsDefault = entity.GetAttributeValue<bool?>("isdefault"),
                            Name = entity.GetAttributeValue<string>("name"),
                            FetchXml = entity.GetAttributeValue<string>("fetchxml"),
                            LayoutXml = entity.GetAttributeValue<string>("layoutxml"),
                        };

                        views.Add(view);

                        if (entity.TryGetAttributeValue<string>("fetchxml", out string fetchxml))
                        {
                            if (!String.IsNullOrEmpty(fetchxml))
                            {
                                var xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(fetchxml);  // Load from a xml string.
                                XmlNode root = xmlDoc.DocumentElement;

                                ExploreXmlRecursively(root, view, "fetchxml");
                            }
                        }

                        if (entity.TryGetAttributeValue<string>("layoutxml", out string layoutxml))
                        {
                            if (!String.IsNullOrEmpty(layoutxml))
                            {
                                var xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(layoutxml); // Load from a xml string.
                                XmlNode root = xmlDoc.DocumentElement;

                                ExploreXmlRecursively(root, view, "layoutxml");
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

                    using (var dlg = new SaveFileDialog())
                    {
                        dlg.Filter = "Excel file(*.xlsx)|*.xlsx";

                        if (dlg.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        WorkAsync(new WorkAsyncInfo
                        {
                            Message = "Exporting views...",
                            Work = (worker, args2) =>
                            {
                                ExcelPackage.License.SetNonCommercialPersonal("mkdev");
                                using (var package = new ExcelPackage())
                                {
                                    var wb = package.Workbook;

                                    // Views list
                                    var ws1 = wb.Worksheets.Add("Views list");
                                    var range1 = ws1.Cells["A3"].LoadFromCollection(views, c =>
                                    {
                                        c.PrintHeaders = true;
                                        c.TableStyle = TableStyles.Light8;
                                    });

                                    for (int i = 1; i <= range1.Columns; i++)
                                    {
                                        ws1.Column(i).Width = 12;
                                    }

                                    // Xml node
                                    var ws2 = wb.Worksheets.Add("Xml node");
                                    var range2 = ws2.Cells["A3"].LoadFromDataTable(nodes, c =>
                                    {
                                        c.PrintHeaders = true;
                                        c.TableStyle = TableStyles.Light8;
                                    });

                                    for (int i = 1; i <= range2.Columns; i++)
                                    {
                                        ws2.Column(i).Width = 12;
                                    }

                                    package.SaveAs(new FileInfo(dlg.FileName));
                                }
                            },
                            PostWorkCallBack = (args2) =>
                            {
                                if (args2.Error != null)
                                {
                                    MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                DialogResult result = MessageBox.Show("The file has been saved successfully.\r\nDo you want to open it?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    Process.Start(new ProcessStartInfo(dlg.FileName) { UseShellExecute = true });
                                }
                            }
                        });
                    }
                }
            });
        }
        private void ExploreXmlRecursively(XmlNode node)
        {
            var dr = nodes.NewRow();

            // Core columns
            ParseNodeToDataRow(dr, node);

            nodes.Rows.Add(dr);

            foreach (XmlNode childNode in node.ChildNodes)
            {
                ExploreXmlRecursively(childNode);
            }
        }

        private void ExploreXmlRecursively(XmlNode node, ViewModel view, string xmlType)
        {
            var dr = nodes.NewRow();

            // Option columns
            dr["Guid"] = view.Guid;
            dr["Entity"] = view.Entity;
            dr["Type"] = view.Type;
            dr["QueryType"] = view.QueryType;
            dr["IsDefault"] = view.IsDefault;
            dr["Name"] = view.Name;
            dr["XmlType"] = xmlType;
            // Core columns
            ParseNodeToDataRow(dr, node);

            nodes.Rows.Add(dr);

            foreach (XmlNode childNode in node.ChildNodes)
            {
                ExploreXmlRecursively(childNode, view, xmlType);
            }
        }

        private void ParseNodeToDataRow(DataRow dr, XmlNode node)
        {
            var nodeAttributes = new List<(string Name, string Value)>();
            var nodePath = new List<(string Name, string Value)>();

            if (node.Attributes != null)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    nodeAttributes.Add((attribute.LocalName, attribute.Value));
                }
            }

            dr["NodeURI"] = String.Join(".", GetNodePath(node));
            dr["ParentNode"] = node.ParentNode?.LocalName;
            dr["NodeName"] = node.LocalName;
            dr["InnerXml"] = node.InnerXml;
            dr["NodeValue"] = ValidateEmptyValue(node.Value);
            dr["NodeAttributes"] = "{" + $"{String.Join(", ", nodeAttributes.Select(s => $"\"{s.Name}\": \"{s.Value}\"").ToArray())}" + "}";

            if (node.Attributes != null)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    var attributeName = $"NodeAttributes.{attribute.Name}";

                    if (!nodes.Columns.Contains(attributeName))
                    {
                        nodes.Columns.Add(attributeName);
                    }

                    dr[attributeName] = attribute.Value;
                }
            }
        }

        private string GetQueryTypeLabel(int queryType)
        {
           var SavedQueryQueryTypeMap = new Dictionary<int, string>
            {
                { 0, "MainApplicationView" },
                { 1, "AdvancedSearch" },
                { 2, "SubGrid" },
                { 4, "QuickFindSearch" },
                { 8, "Reporting" },
                { 16, "OfflineFilters" },
                { 64, "LookupView" },
                { 128, "SMAppointmentBookView" },
                { 256, "OutlookFilters" },
                { 512, "AddressBookFilters" },
                { 1024, "MainApplicationViewWithoutSubject" },
                { 2048, "SavedQueryTypeOther" },
                { 4096, "InteractiveWorkflowView" },
                { 8192, "OfflineTemplate" },
                { 16384, "CustomDefinedView" },
                { 65536, "ExportFieldTranslationsView" },
                { 131072, "OutlookTemplate" }
            };

            string queryTypeLabel = String.Empty;

            if (SavedQueryQueryTypeMap.TryGetValue(queryType, out string label))
            {
                return $"{queryType}: {label}";
            }
            else
            {
                return $"{queryType}";
            }
        }

        private List<string> GetNodePath(XmlNode node)
        {
            if (node.ParentNode == null)
            {
                return new List<string> { };
            }
            else
            {
                var result = GetNodePath(node.ParentNode);
                result.Add(node.LocalName);
                return result;
            }
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

            if (solutionComponents.Entities.Count == 0)
            {
                MessageBox.Show("No entities found in solution.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (solutionComponents.Entities.Count > 0)
            {
                WorkAsync(new WorkAsyncInfo
                {
                    // Access form components inside PostWorkCallBack to avoid potential runtime errors.

                    Message = "Getting entities from solution...",
                    Work = (worker, args) =>
                    {
                        var entityMetadataIds = solutionComponents.Entities
                            .Select(s => s.GetAttributeValue<Guid>("objectid"))
                            .ToHashSet();

                        var request = new RetrieveAllEntitiesRequest
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
                            .Select(s => new EntityModel
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
                        var Entities = args.Result as List<EntityModel>;

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
        }

        private OrderModel GetOrder(string logicalName, string sortValue)
        {
            int priority = int.Parse(sortValue[2].ToString());
            bool descending = sortValue[0].ToString() == "↓";

            return new OrderModel()
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

        private List<EntityModel> GetSelectedEntities()
        {
            var selectedEntities = tlpEntity_lvwEntities.Items.Cast<ListViewItem>()
                .Where(w => w.Checked == true && ((EntityModel)w.Tag).IsValidForAdvancedFind == true)
                .Select(s => (EntityModel)s.Tag)
                .ToList();

            return selectedEntities;
        }


        private void CreateView()
        {
            if (String.IsNullOrWhiteSpace(tlpViewOptions_txtName.Text))
            {
                MessageBox.Show("Name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tlpViewOptions_txtName.Focus();
                return;
            }

            ListView listViewColumns = tlpColumn_lvwColumns;

            var selectedEntities = GetSelectedEntities();

            if (selectedEntities.Count == 0)
            {
                MessageBox.Show("At least one entity must be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedColumns = listViewColumns.Items.Cast<ListViewItem>()
                .Where(w => w.Checked == true)
                .Select(s => new ColumnModel()
                {
                    DisplayName = s.Text,
                    LogicalName = s.SubItems[1].Text,
                    Order = s.SubItems[2].Text,
                    Width = s.SubItems[3].Text,
                })
                .ToList();

            if (selectedColumns.Count == 0)
            {
                MessageBox.Show("At least one column must be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 0: User view
            // 1: Public view
            int typeCmdValue = int.Parse(tlpViewOptions_cmbType.Text[0].ToString());
            string queryTable = String.Empty;

            switch (typeCmdValue)
            {
                case 0:
                    queryTable = "userquery";
                    break;
                case 1:
                    queryTable = "savedquery";
                    break;
                default:
                    MessageBox.Show("Type: An unexpected item has been selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            // 0: Disable overwrite
            // 1: Enable overwrite
            int overwriteCmdValue = int.Parse(tlpViewOptions_cmbOverwrite.Text[0].ToString());
            bool disableOverwrite = true;

            switch (overwriteCmdValue)
            {
                case 0:
                    disableOverwrite = true;
                    break;
                case 1:
                    disableOverwrite = false;
                    break;
                default:
                    MessageBox.Show("Overwrite: An unexpected item has been selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            // 0: All (No filter)
            // 1: Active only
            // 2: Inactive only
            int filterCmdValue = int.Parse(tlpViewOptions_cmbFilter.Text[0].ToString());
            string filterNode = String.Empty;

            switch (filterCmdValue)
            {
                case 0:
                    filterNode = String.Empty;
                    break;
                case 1:
                    filterNode = "<filter><condition attribute='statecode' operator='eq' value='0' /></filter>";
                    break;
                case 2:
                    filterNode = "<filter><condition attribute='statecode' operator='eq' value='1' /></filter>";
                    break;
                default:
                    MessageBox.Show("Filter: An unexpected item has been selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            DialogResult result = MessageBox.Show(String.Join("\r\n", new string[]
            {
                "Do you want to create the view?",
                String.Empty,
                "ConnectionDetail:",
                $"  Organization: {ConnectionDetail.OrganizationFriendlyName} [{ConnectionDetail.Organization}]",
                $"  WebApplicationUrl: {ConnectionDetail.WebApplicationUrl}",
                $"  UserName: {ConnectionDetail.UserName}",
            }), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Creating view...",
                Work = (worker, args) =>
                {
                    string formatAttribute = "<attribute name='{0}' />";
                    string formatOrder = "<order attribute='{0}' descending='{1}' />";
                    string formatCell = "<cell name='{0}' width='{1}' />";

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

                        var request = new RetrieveEntityRequest
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
                            .Where(w => !String.IsNullOrEmpty(w.Order))
                            .Select(s => GetOrder(s.LogicalName, s.Order))
                            .OrderBy(o => o.Priority)
                            .ToList();

                        var attributeNode = String.Join(String.Empty, columnSet
                            .Select(s => String.Format(formatAttribute, s.LogicalName))).Replace(formatPrimaryName, entityPrimaryName);

                        var orderNode = String.Join(String.Empty, sortSet
                            .Select(s => String.Format(formatOrder, s.LogicalName, s.Descending))).Replace(formatPrimaryName, entityPrimaryName); ;

                        var cellNode = String.Join(String.Empty, columnSet
                            .Select(s => String.Format(formatCell, s.LogicalName, s.Width))).Replace(formatPrimaryName, entityPrimaryName);

                        string fetchxml = $@"
                        <fetch>
                          <entity name='{entityLocicalName}'>
                            {attributeNode}
                            {orderNode}
                            {filterNode}
                          </entity>
                        </fetch>
                        ";

                        string layoutxml = $@"
                        <grid name='resultset' object='{entityObjectTypeCode}' jump='{entityPrimaryName}' select='1' icon='1' preview='1'>
                          <row name='result' id='{entityPrimaryId}'>
                            {cellNode}
                          </row>
                        </grid>
                        ";

                        string yyyymmdd = DateTime.Now.ToString("yyyyMMdd");
                        var formattedViewName = viewName
                            .Replace(formatEntityDisplayName, entity.DisplayName)
                            .Replace(formatEntityLogicalName, entity.LogicalName)
                            .Replace(formatDate, yyyymmdd);

                        var create = new Entity(queryTable)
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
                        MessageBox.Show("View created.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            });
        }

        private void PickSolution()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting solutions...",
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
                            .Select(s => new SolutionModel
                            {
                                SolutionId = s.Id.ToString(),
                                DisplayName = s.GetAttributeValue<string>("friendlyname"),
                                Version = s.GetAttributeValue<string>("version"),
                                Publisher = s.GetAttributeValue<EntityReference>("publisherid")?.Name
                            })
                            .ToList();

                        using (var form = new SolutionSelector(items))
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

        private void SetOrderValue(ListView listView, ComboBox comboBox)
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

        private void ClearOrderValue(ListView listView, ComboBox comboBox)
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

        private void tlpOrder_cbxOrder_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tlpOrder_btnSet_Click(object sender, EventArgs e)
        {
            SetOrderValue(tlpColumn_lvwColumns, tlpOrder_cmbOrder);
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

        private void tlpOrder_btnClear_Click(object sender, EventArgs e)
        {
            ClearOrderValue(tlpColumn_lvwColumns, tlpOrder_cmbOrder);
        }

        private void tlpColumn_lvwColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tlpColumn_lvwColumns.SelectedItems.Count > 1)
            {
                tlpPosition_btnUp.Enabled = false;
                tlpPosition_btnDown.Enabled = false;
                tlpOrder_btnSet.Enabled = false;
                tlpOrder_btnClear.Enabled = false;
            }
            else
            {
                tlpPosition_btnUp.Enabled = true;
                tlpPosition_btnDown.Enabled = true;
                tlpOrder_btnSet.Enabled = true;
                tlpOrder_btnClear.Enabled = true;
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

        private void ddbSaveAs_itemPublic_Click(object sender, EventArgs e)
        {
            ExecuteMethod(DuplicateAsPublicView);
        }

        private void ddbSaveAs_itemPersonal_Click(object sender, EventArgs e)
        {
            ExecuteMethod(DuplicateAsUserView);
        }

        private void ddbExport_itemLoadXML_Click(object sender, EventArgs e)
        {
            ExcelExportFromLoadedXml(); // No DataVerse operations.
        }
    }
}