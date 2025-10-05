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
using System.Windows.Shapes;
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
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using StandardViewCreator.Models;
using StandardViewCreator.MyDataTable;
using XrmToolBox.Extensibility;

namespace StandardViewCreator
{
    public partial class StandardViewCreatorControl : PluginControlBase
    {
        private Settings mySettings;

        private DataTable dtNodes = new DataTable("Xml node");
        private DataTable dtViews = new DataTable("Views list");
        private DataTable dtFiles = new DataTable("Load files");

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
            txtName_tip.SetToolTip(tlpViewOptions_lblName, "Available: {!EntityLogicalName}, {!EntityDisplayName}, {!yyyyMMdd}");
            cmbOverwrite_tip.SetToolTip(tlpViewOptions_lblOverwrite, "Update mode unavailable. Running in create mode.");

            // Set textbox
            tlpViewOptions_txtName.Text = "My View - {!EntityDisplayName} - {!yyyyMMdd}";
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
                <attribute name='statecode' />
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
                    <attribute name='ismanaged' />
                    <attribute name='statecode' />
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

                    if (queries.Count == 0)
                    {
                        return;
                    }

                    var items = queries
                            .Select(s => new ViewModel
                            {
                                Guid = s.Id,
                                Entity = s.GetAttributeValue<string>("returnedtypecode"),
                                Type = s.LogicalName,
                                Name = s.GetAttributeValue<string>("name"),
                            })
                            .ToList();

                    var selectedItems = new List<ViewModel>();

                    using (var form = new ViewSelector(items))
                    {
                        if (form.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        selectedItems.Add(form.SelectedItem);
                    }

                    WorkAsync(new WorkAsyncInfo
                    {
                        Message = "Duplicating view...",
                        Work = (worker, args2) =>
                        {
                            foreach (var selectedItem in selectedItems)
                            {
                                var query = SafeRetrieve(selectedItem.Type, selectedItem.Guid, new ColumnSet(true));

                                if (query.Id == null)
                                {
                                    continue;
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
                            }
                        },
                        PostWorkCallBack = (args2) =>
                        {
                            MessageBox.Show("View duplicated.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    });
                }
            });
        }

        private void ExcelExportFromLoadedXml()
        {
            var openPaths = SelectOpenXmlPath();

            if (openPaths.Length == 0)
            {
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading xml...",
                Work = (worker, args) =>
                {
                    dtFiles.Reset();
                    dtFiles.Columns.Add(Columns.FilePath);

                    dtNodes.Reset();
                    // Option columns
                    dtNodes.Columns.Add(Columns.FileName);
                    // Core columns
                    dtNodes.Columns.Add(Columns.NodeURI);
                    dtNodes.Columns.Add(Columns.ParentNode);
                    dtNodes.Columns.Add(Columns.NodeName);
                    // dtNodes.Columns.Add(Columns.InnerXml);
                    dtNodes.Columns.Add(Columns.NodeValue);
                    dtNodes.Columns.Add(Columns.NodeAttributes);
                    dtNodes.Columns.Add(Columns.AttributeName);
                    dtNodes.Columns.Add(Columns.AttributeValue);

                    foreach (var openPath in openPaths)
                    {
                        var xmlDoc = new XmlDocument();

                        try
                        {
                            xmlDoc.Load(openPath); // Load from a XML file
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to load XML: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        XmlNode root = xmlDoc.DocumentElement;
                        ParseLocalXmlToDataTable(root, System.IO.Path.GetFileName(openPath));

                        var dr = dtFiles.NewRow();
                        dr[Columns.FilePath] = openPath;
                        dtFiles.Rows.Add(dr);
                    }
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    var savePath = SelectSaveExcelPath();

                    if (String.IsNullOrEmpty(savePath))
                    {
                        return;
                    }

                    WorkAsync(new WorkAsyncInfo
                    {
                        Message = "Exporting xmls...",
                        Work = (worker, args2) =>
                        {
                            var sheets = new DataSet();
                            // Use Copy() because a DataTable cannot belong to multiple DataSets.
                            sheets.Tables.Add(dtFiles.Copy());
                            sheets.Tables.Add(dtNodes.Copy());

                            var config = new OpenXmlConfiguration()
                            {
                                TableStyles = MiniExcelLibs.OpenXml.TableStyles.None
                            };

                            try
                            {
                                MiniExcel.SaveAs(savePath, sheets, configuration: config, overwriteFile: true);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to save file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
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
                                Process.Start(new ProcessStartInfo(savePath) { UseShellExecute = true });
                            }
                        }
                    });
                }
            });
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

                    dtViews.Reset();
                    dtViews.Columns.Add(Columns.Guid);
                    dtViews.Columns.Add(Columns.Entity);
                    dtViews.Columns.Add(Columns.Type);
                    dtViews.Columns.Add(Columns.Name);
                    dtViews.Columns.Add(Columns.QueryType);
                    dtViews.Columns.Add(Columns.IsDefault);
                    dtViews.Columns.Add(Columns.Status);
                    dtViews.Columns.Add(Columns.IsManaged);
                    dtViews.Columns.Add(Columns.FetchXml);
                    dtViews.Columns.Add(Columns.LayoutXml);

                    dtNodes.Reset();
                    // Option columns
                    dtNodes.Columns.Add(Columns.Guid);
                    dtNodes.Columns.Add(Columns.Entity);
                    dtNodes.Columns.Add(Columns.Type);
                    dtNodes.Columns.Add(Columns.Name);
                    dtNodes.Columns.Add(Columns.QueryType);
                    dtNodes.Columns.Add(Columns.IsDefault);
                    dtNodes.Columns.Add(Columns.Status);
                    dtNodes.Columns.Add(Columns.IsManaged);
                    dtNodes.Columns.Add(Columns.XmlType);
                    // Core columns
                    dtNodes.Columns.Add(Columns.NodeURI);
                    dtNodes.Columns.Add(Columns.ParentNode);
                    dtNodes.Columns.Add(Columns.NodeName);
                    // dtNodes.Columns.Add(Columns.InnerXml);
                    dtNodes.Columns.Add(Columns.NodeValue);
                    dtNodes.Columns.Add(Columns.NodeAttributes);
                    dtNodes.Columns.Add(Columns.AttributeName);
                    dtNodes.Columns.Add(Columns.AttributeValue);

                    // View node
                    foreach (var entity in queries)
                    {
                        var dr = dtViews.NewRow();

                        dr[Columns.Guid] = entity.Id;
                        dr[Columns.Entity] = entity.GetAttributeValue<string>("returnedtypecode");
                        dr[Columns.Type] = entity.LogicalName;
                        dr[Columns.Name] = entity.GetAttributeValue<string>("name");
                        dr[Columns.QueryType] = GetQueryTypeLabel(entity.GetAttributeValue<int>("querytype"));
                        dr[Columns.IsDefault] = ValidateFormattedValue(entity, "isdefault");
                        dr[Columns.Status] = ValidateFormattedValue(entity, "statecode");
                        dr[Columns.IsManaged] = ValidateFormattedValue(entity, "ismanaged");
                        dr[Columns.FetchXml] = entity.GetAttributeValue<string>("fetchxml");
                        dr[Columns.LayoutXml] = entity.GetAttributeValue<string>("layoutxml");

                        dtViews.Rows.Add(dr);

                        if (entity.TryGetAttributeValue<string>("fetchxml", out string fetchxml))
                        {
                            if (!String.IsNullOrEmpty(fetchxml))
                            {
                                var xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(fetchxml);  // Load from a xml string.
                                XmlNode root = xmlDoc.DocumentElement;

                                ParseViewXmlToDataTable(root, dr, "fetchxml");
                            }
                        }

                        if (entity.TryGetAttributeValue<string>("layoutxml", out string layoutxml))
                        {
                            if (!String.IsNullOrEmpty(layoutxml))
                            {
                                var xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(layoutxml); // Load from a xml string.
                                XmlNode root = xmlDoc.DocumentElement;

                                ParseViewXmlToDataTable(root, dr, "layoutxml");
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

                    var savePath = SelectSaveExcelPath();

                    if (String.IsNullOrEmpty(savePath))
                    {
                        return;
                    }

                    WorkAsync(new WorkAsyncInfo
                    {
                        Message = "Exporting views...",
                        Work = (worker, args2) =>
                        {
                            var sheets = new DataSet();
                            // Use Copy() because a DataTable cannot belong to multiple DataSets.
                            sheets.Tables.Add(dtViews.Copy());
                            sheets.Tables.Add(dtNodes.Copy());

                            var config = new OpenXmlConfiguration()
                            {
                                TableStyles = MiniExcelLibs.OpenXml.TableStyles.None
                            };

                            try
                            {
                                MiniExcel.SaveAs(savePath, sheets, configuration: config, overwriteFile: true);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to save file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
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
                                Process.Start(new ProcessStartInfo(savePath) { UseShellExecute = true });
                            }
                        }
                    });
                }
            });
        }
        private void ParseLocalXmlToDataTable(XmlNode node, string fileName)
        {
            var dr = dtNodes.NewRow();

            // Option columns
            dr[Columns.FileName] = fileName;
            // Core columns
            ParseNodeToDataRow(dr, node);

            // dtNodes.Rows.Add(dr);

            foreach (XmlNode childNode in node.ChildNodes)
            {
                ParseLocalXmlToDataTable(childNode, fileName);
            }
        }

        private void ParseViewXmlToDataTable(XmlNode node, DataRow view, string xmlType)
        {
            var dr = dtNodes.NewRow();

            // Option columns
            dr[Columns.Guid] = view.Field<string>(Columns.Guid);
            dr[Columns.Entity] = view.Field<string>(Columns.Entity);
            dr[Columns.Type] = view.Field<string>(Columns.Type);
            dr[Columns.Name] = view.Field<string>(Columns.Name);
            dr[Columns.QueryType] = view.Field<string>(Columns.QueryType);
            dr[Columns.IsDefault] = view.Field<string>(Columns.IsDefault);
            dr[Columns.Status] = view.Field<string>(Columns.Status);
            dr[Columns.IsManaged] = view.Field<string>(Columns.IsManaged);
            dr[Columns.XmlType] = xmlType;
            // Core columns
            ParseNodeToDataRow(dr, node);

            // dtNodes.Rows.Add(dr);

            foreach (XmlNode childNode in node.ChildNodes)
            {
                ParseViewXmlToDataTable(childNode, view, xmlType);
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
            
            dr[Columns.NodeURI] = GetNodePath(node);
            dr[Columns.ParentNode] = node.ParentNode?.LocalName;
            dr[Columns.NodeName] = node.LocalName;
            // Cut off if length exceeds Excel's limit (32767 chars)
            // dr[Columns.InnerXml] = CheckStringLength(node.InnerXml); 
            dr[Columns.NodeValue] = ValidateEmptyValue(node.Value);
            dr[Columns.NodeAttributes] = "{" + $"{String.Join(", ", nodeAttributes.Select(s => $"\"{s.Name}\": \"{s.Value}\"").ToArray())}" + "}";

            if (node.Attributes != null)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    var drExpand = dtNodes.NewRow();
                    drExpand.ItemArray = dr.ItemArray;

                    var attributeName = $"{node.Name}.{attribute.Name}";

                    drExpand[Columns.AttributeName] = attributeName;
                    drExpand[Columns.AttributeValue] = attribute.Value;

                    dtNodes.Rows.Add(drExpand);

                    // var attributeName = $"{Columns.NodeAttributes}.{attribute.Name}";
                    // 
                    // if (!dtNodes.Columns.Contains(attributeName))
                    // {
                    //     dtNodes.Columns.Add(attributeName);
                    // }
                    // 
                    // dr[attributeName] = attribute.Value;
                }
            } else
            {
                dtNodes.Rows.Add(dr);
            }
        }

        private string CheckStringLength(string value)
        {
            if (!String.IsNullOrEmpty(value) && value.Length > 32767)
            {
                return "{value too long}";
            }
            else
            {
                return value;
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

        private List<string> ParseParentNodes(XmlNode node)
        {
            if (node.ParentNode == null)
            {
                return new List<string> { };
            }
            else
            {
                var result = ParseParentNodes(node.ParentNode);
                result.Add(node.LocalName);
                return result;
            }
        }

        private string GetNodePath(XmlNode node)
        {
            var result = ParseParentNodes(node);

            return String.Join("/", result);
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

        private string ValidateFormattedValue(Entity entity, string attributeName)
        {
            if (entity.FormattedValues.Contains(attributeName))
            {
                return entity.FormattedValues[attributeName];
            }
            else
            {
                return String.Empty;
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

            var Entities = new List<EntityModel>();

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

                    Entities = allEntities.EntityMetadata
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

                },
                PostWorkCallBack = (args) =>
                {
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

                    string formatPrimaryName = "{!PrimaryAttribute}";
                    string formatEntityLogicalName = "{!EntityLogicalName}";
                    string formatEntityDisplayName = "{!EntityDisplayName}";
                    string formatDate = "{!yyyyMMdd}";

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

        private string SelectSaveExcelPath()
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "Excel file(*.xlsx)|*.xlsx";

                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return String.Empty;
                }

                return dlg.FileName;
            }
        }

        private string[] SelectOpenXmlPath()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "XML file(*.xml)|*.xml";
                dlg.Multiselect = true;

                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return Array.Empty<string>();
                }

                return dlg.FileNames;
            }
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

        private void ddbSaveAs_itemUser_Click(object sender, EventArgs e)
        {
            ExecuteMethod(DuplicateAsUserView);
        }

        private void ddbExport_itemLoadXML_Click(object sender, EventArgs e)
        {
            ExcelExportFromLoadedXml(); // No DataVerse operations.
        }
    }
}