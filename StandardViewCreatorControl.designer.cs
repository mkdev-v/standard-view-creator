namespace StandardViewCreator
{
    partial class StandardViewCreatorControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "Created On",
            "createdon",
            "",
            "150"}, -1);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
            "Created By",
            "createdby",
            "",
            "150"}, -1);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] {
            "Created By (Delegate)",
            "createdonbehalfby",
            "",
            "150"}, -1);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] {
            "Modified On",
            "modifiedon",
            "↓(1)",
            "150"}, -1);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] {
            "Modified By",
            "modifiedby",
            "",
            "150"}, -1);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] {
            "Modified By (Delegate)",
            "modifiedonbehalfby",
            "",
            "150"}, -1);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem(new string[] {
            "Status",
            "statecode",
            "",
            "150"}, -1);
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem(new string[] {
            "Status Reason",
            "statuscode",
            "",
            "150"}, -1);
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem(new string[] {
            "{primaryAttribute}",
            "{primaryAttribute}",
            "",
            "150"}, -1);
            System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem(new string[] {
            "Owner",
            "ownerid",
            "",
            "150"}, -1);
            System.Windows.Forms.ListViewItem listViewItem22 = new System.Windows.Forms.ListViewItem(new string[] {
            "Owning Business Unit",
            "owningbusinessunit",
            "",
            "150"}, -1);
            this.tsmMain = new System.Windows.Forms.ToolStrip();
            this.tsmMain_btnLoadEntitiesFromSolution = new System.Windows.Forms.ToolStripButton();
            this.tsmMain_sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmMain_btnClose = new System.Windows.Forms.ToolStripButton();
            this.tsmMain_sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmMain_ddbExport = new System.Windows.Forms.ToolStripDropDownButton();
            this.ddbExport_itemAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ddbExport_itemSelectedEntities = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMain_sep3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmMain_ddbSaveAs = new System.Windows.Forms.ToolStripDropDownButton();
            this.ddbSaveAs_itemPublic = new System.Windows.Forms.ToolStripMenuItem();
            this.ddbSaveAs_itemPersonal = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpMain_gbxEntity = new System.Windows.Forms.GroupBox();
            this.gbxEntity_tlpEntity = new System.Windows.Forms.TableLayoutPanel();
            this.tlpEntity_lvwEntities = new System.Windows.Forms.ListView();
            this.lvwEntities_colDisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwEntities_colLogicalName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwEntities_colPrimary = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwEntities_colIsValidForAdvancedFind = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tlpEntity_lblNote = new System.Windows.Forms.Label();
            this.tlpEntity_tlpCommands = new System.Windows.Forms.TableLayoutPanel();
            this.tlpCommands_gbxSelect = new System.Windows.Forms.GroupBox();
            this.gbxSelect_tlpSelect = new System.Windows.Forms.TableLayoutPanel();
            this.tlpSelect_btnUnselectAll = new System.Windows.Forms.Button();
            this.tlpSelect_btnSelectAll = new System.Windows.Forms.Button();
            this.tlpMain_gbxView = new System.Windows.Forms.GroupBox();
            this.gbxView_tlpViewOptions = new System.Windows.Forms.TableLayoutPanel();
            this.tlpViewOptions_lblType = new System.Windows.Forms.Label();
            this.tlpViewOptions_lblName = new System.Windows.Forms.Label();
            this.tlpViewOptions_lblOverwrite = new System.Windows.Forms.Label();
            this.tlpViewOptions_lblFilter = new System.Windows.Forms.Label();
            this.tlpViewOptions_cmbType = new System.Windows.Forms.ComboBox();
            this.tlpViewOptions_cmbOverwrite = new System.Windows.Forms.ComboBox();
            this.tlpViewOptions_cmbFilter = new System.Windows.Forms.ComboBox();
            this.tlpViewOptions_txtName = new System.Windows.Forms.TextBox();
            this.tlpMain_gbxColumn = new System.Windows.Forms.GroupBox();
            this.gbxColumn_tlpColumn = new System.Windows.Forms.TableLayoutPanel();
            this.tlpColumn_tlpCommands = new System.Windows.Forms.TableLayoutPanel();
            this.tlpCommands_gbxPosition = new System.Windows.Forms.GroupBox();
            this.gbxPosition_tlpPosition = new System.Windows.Forms.TableLayoutPanel();
            this.tlpPosition_btnDown = new System.Windows.Forms.Button();
            this.tlpPosition_btnUp = new System.Windows.Forms.Button();
            this.tlpCommands_gbxOrder = new System.Windows.Forms.GroupBox();
            this.gbxOrder_tlpOrder = new System.Windows.Forms.TableLayoutPanel();
            this.tlpOrder_cmbOrder = new System.Windows.Forms.ComboBox();
            this.tlpOrder_btnClear = new System.Windows.Forms.Button();
            this.tlpOrder_btnSet = new System.Windows.Forms.Button();
            this.tlpCommands_gbxWidth = new System.Windows.Forms.GroupBox();
            this.gbxWidth_tlpWidth = new System.Windows.Forms.TableLayoutPanel();
            this.tlpWidth_btnSet = new System.Windows.Forms.Button();
            this.tlpWidth_sudWidthValue = new System.Windows.Forms.NumericUpDown();
            this.tlpCommands_gbxProcess = new System.Windows.Forms.GroupBox();
            this.gbxProcess_tlpProcess = new System.Windows.Forms.TableLayoutPanel();
            this.tlpProcess_btnCreate = new System.Windows.Forms.Button();
            this.tlpColumn_lvwColumns = new System.Windows.Forms.ListView();
            this.lvwColumns_colDisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwColumns_colLogicalName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwColumns_colOrder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwColumns_colWidth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tlpColumn_lblNote = new System.Windows.Forms.Label();
            this.cmbOverwrite_tip = new System.Windows.Forms.ToolTip(this.components);
            this.txtName_tip = new System.Windows.Forms.ToolTip(this.components);
            this.ddbExport_itemLoadXML = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMain.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.tlpMain_gbxEntity.SuspendLayout();
            this.gbxEntity_tlpEntity.SuspendLayout();
            this.tlpEntity_tlpCommands.SuspendLayout();
            this.tlpCommands_gbxSelect.SuspendLayout();
            this.gbxSelect_tlpSelect.SuspendLayout();
            this.tlpMain_gbxView.SuspendLayout();
            this.gbxView_tlpViewOptions.SuspendLayout();
            this.tlpMain_gbxColumn.SuspendLayout();
            this.gbxColumn_tlpColumn.SuspendLayout();
            this.tlpColumn_tlpCommands.SuspendLayout();
            this.tlpCommands_gbxPosition.SuspendLayout();
            this.gbxPosition_tlpPosition.SuspendLayout();
            this.tlpCommands_gbxOrder.SuspendLayout();
            this.gbxOrder_tlpOrder.SuspendLayout();
            this.tlpCommands_gbxWidth.SuspendLayout();
            this.gbxWidth_tlpWidth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlpWidth_sudWidthValue)).BeginInit();
            this.tlpCommands_gbxProcess.SuspendLayout();
            this.gbxProcess_tlpProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsmMain
            // 
            this.tsmMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmMain_btnLoadEntitiesFromSolution,
            this.tsmMain_sep1,
            this.tsmMain_btnClose,
            this.tsmMain_sep2,
            this.tsmMain_ddbExport,
            this.tsmMain_sep3,
            this.tsmMain_ddbSaveAs});
            this.tsmMain.Location = new System.Drawing.Point(0, 0);
            this.tsmMain.Name = "tsmMain";
            this.tsmMain.Size = new System.Drawing.Size(960, 25);
            this.tsmMain.TabIndex = 4;
            this.tsmMain.Text = "toolStrip1";
            // 
            // tsmMain_btnLoadEntitiesFromSolution
            // 
            this.tsmMain_btnLoadEntitiesFromSolution.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmMain_btnLoadEntitiesFromSolution.Name = "tsmMain_btnLoadEntitiesFromSolution";
            this.tsmMain_btnLoadEntitiesFromSolution.Size = new System.Drawing.Size(168, 22);
            this.tsmMain_btnLoadEntitiesFromSolution.Text = "📁 Load Entities from Solution";
            this.tsmMain_btnLoadEntitiesFromSolution.Click += new System.EventHandler(this.tsmMain_btnLoadEntitiesFromSolution_Click);
            // 
            // tsmMain_sep1
            // 
            this.tsmMain_sep1.Name = "tsmMain_sep1";
            this.tsmMain_sep1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsmMain_btnClose
            // 
            this.tsmMain_btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmMain_btnClose.Name = "tsmMain_btnClose";
            this.tsmMain_btnClose.Size = new System.Drawing.Size(54, 22);
            this.tsmMain_btnClose.Text = "❌ Close";
            this.tsmMain_btnClose.Click += new System.EventHandler(this.tsmMain_btnClose_Click);
            // 
            // tsmMain_sep2
            // 
            this.tsmMain_sep2.Name = "tsmMain_sep2";
            this.tsmMain_sep2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsmMain_ddbExport
            // 
            this.tsmMain_ddbExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddbExport_itemAll,
            this.ddbExport_itemSelectedEntities,
            this.ddbExport_itemLoadXML});
            this.tsmMain_ddbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmMain_ddbExport.Name = "tsmMain_ddbExport";
            this.tsmMain_ddbExport.Size = new System.Drawing.Size(113, 22);
            this.tsmMain_ddbExport.Text = "📤 Export to Excel";
            this.tsmMain_ddbExport.ToolTipText = "📤 Export to Excel";
            // 
            // ddbExport_itemAll
            // 
            this.ddbExport_itemAll.Name = "ddbExport_itemAll";
            this.ddbExport_itemAll.Size = new System.Drawing.Size(180, 22);
            this.ddbExport_itemAll.Text = "All";
            this.ddbExport_itemAll.Click += new System.EventHandler(this.ddbExport_itemAll_Click);
            // 
            // ddbExport_itemSelectedEntities
            // 
            this.ddbExport_itemSelectedEntities.Name = "ddbExport_itemSelectedEntities";
            this.ddbExport_itemSelectedEntities.Size = new System.Drawing.Size(180, 22);
            this.ddbExport_itemSelectedEntities.Text = "Selected Entities";
            this.ddbExport_itemSelectedEntities.Click += new System.EventHandler(this.ddbExport_itemSelectedEntities_Click);
            // 
            // tsmMain_sep3
            // 
            this.tsmMain_sep3.Name = "tsmMain_sep3";
            this.tsmMain_sep3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsmMain_ddbSaveAs
            // 
            this.tsmMain_ddbSaveAs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddbSaveAs_itemPublic,
            this.ddbSaveAs_itemPersonal});
            this.tsmMain_ddbSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmMain_ddbSaveAs.Name = "tsmMain_ddbSaveAs";
            this.tsmMain_ddbSaveAs.Size = new System.Drawing.Size(229, 22);
            this.tsmMain_ddbSaveAs.Text = "🗒️ Duplicate View from Selected Entities";
            this.tsmMain_ddbSaveAs.ToolTipText = "🗒️ Duplicate View from Selected Entities";
            // 
            // ddbSaveAs_itemPublic
            // 
            this.ddbSaveAs_itemPublic.Name = "ddbSaveAs_itemPublic";
            this.ddbSaveAs_itemPublic.Size = new System.Drawing.Size(163, 22);
            this.ddbSaveAs_itemPublic.Text = "As Public View";
            this.ddbSaveAs_itemPublic.Click += new System.EventHandler(this.ddbSaveAs_itemPublic_Click);
            // 
            // ddbSaveAs_itemPersonal
            // 
            this.ddbSaveAs_itemPersonal.Name = "ddbSaveAs_itemPersonal";
            this.ddbSaveAs_itemPersonal.Size = new System.Drawing.Size(163, 22);
            this.ddbSaveAs_itemPersonal.Text = "As Personal View";
            this.ddbSaveAs_itemPersonal.Click += new System.EventHandler(this.ddbSaveAs_itemPersonal_Click);
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.tlpMain_gbxEntity, 0, 0);
            this.tlpMain.Controls.Add(this.tlpMain_gbxView, 1, 0);
            this.tlpMain.Controls.Add(this.tlpMain_gbxColumn, 1, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 25);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(960, 515);
            this.tlpMain.TabIndex = 5;
            // 
            // tlpMain_gbxEntity
            // 
            this.tlpMain_gbxEntity.Controls.Add(this.gbxEntity_tlpEntity);
            this.tlpMain_gbxEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain_gbxEntity.Location = new System.Drawing.Point(3, 3);
            this.tlpMain_gbxEntity.Name = "tlpMain_gbxEntity";
            this.tlpMain.SetRowSpan(this.tlpMain_gbxEntity, 2);
            this.tlpMain_gbxEntity.Size = new System.Drawing.Size(474, 509);
            this.tlpMain_gbxEntity.TabIndex = 2;
            this.tlpMain_gbxEntity.TabStop = false;
            this.tlpMain_gbxEntity.Text = "Entity*";
            // 
            // gbxEntity_tlpEntity
            // 
            this.gbxEntity_tlpEntity.ColumnCount = 1;
            this.gbxEntity_tlpEntity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.gbxEntity_tlpEntity.Controls.Add(this.tlpEntity_lvwEntities, 0, 2);
            this.gbxEntity_tlpEntity.Controls.Add(this.tlpEntity_lblNote, 0, 0);
            this.gbxEntity_tlpEntity.Controls.Add(this.tlpEntity_tlpCommands, 0, 1);
            this.gbxEntity_tlpEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxEntity_tlpEntity.Location = new System.Drawing.Point(3, 15);
            this.gbxEntity_tlpEntity.Name = "gbxEntity_tlpEntity";
            this.gbxEntity_tlpEntity.RowCount = 3;
            this.gbxEntity_tlpEntity.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxEntity_tlpEntity.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxEntity_tlpEntity.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxEntity_tlpEntity.Size = new System.Drawing.Size(468, 491);
            this.gbxEntity_tlpEntity.TabIndex = 0;
            // 
            // tlpEntity_lvwEntities
            // 
            this.tlpEntity_lvwEntities.AllowColumnReorder = true;
            this.tlpEntity_lvwEntities.CheckBoxes = true;
            this.tlpEntity_lvwEntities.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvwEntities_colDisplayName,
            this.lvwEntities_colLogicalName,
            this.lvwEntities_colPrimary,
            this.lvwEntities_colIsValidForAdvancedFind});
            this.tlpEntity_lvwEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEntity_lvwEntities.FullRowSelect = true;
            this.tlpEntity_lvwEntities.GridLines = true;
            this.tlpEntity_lvwEntities.HideSelection = false;
            this.tlpEntity_lvwEntities.Location = new System.Drawing.Point(3, 91);
            this.tlpEntity_lvwEntities.Name = "tlpEntity_lvwEntities";
            this.tlpEntity_lvwEntities.Size = new System.Drawing.Size(462, 397);
            this.tlpEntity_lvwEntities.TabIndex = 1;
            this.tlpEntity_lvwEntities.UseCompatibleStateImageBehavior = false;
            this.tlpEntity_lvwEntities.View = System.Windows.Forms.View.Details;
            // 
            // lvwEntities_colDisplayName
            // 
            this.lvwEntities_colDisplayName.Text = "Display Name";
            this.lvwEntities_colDisplayName.Width = 150;
            // 
            // lvwEntities_colLogicalName
            // 
            this.lvwEntities_colLogicalName.Text = "Logical Name";
            this.lvwEntities_colLogicalName.Width = 150;
            // 
            // lvwEntities_colPrimary
            // 
            this.lvwEntities_colPrimary.Text = "Primary";
            this.lvwEntities_colPrimary.Width = 75;
            // 
            // lvwEntities_colIsValidForAdvancedFind
            // 
            this.lvwEntities_colIsValidForAdvancedFind.Text = "IsValidForAdvancedFind";
            this.lvwEntities_colIsValidForAdvancedFind.Width = 150;
            // 
            // tlpEntity_lblNote
            // 
            this.tlpEntity_lblNote.AutoSize = true;
            this.tlpEntity_lblNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEntity_lblNote.Location = new System.Drawing.Point(3, 3);
            this.tlpEntity_lblNote.Margin = new System.Windows.Forms.Padding(3);
            this.tlpEntity_lblNote.Name = "tlpEntity_lblNote";
            this.tlpEntity_lblNote.Size = new System.Drawing.Size(462, 24);
            this.tlpEntity_lblNote.TabIndex = 4;
            this.tlpEntity_lblNote.Text = "*IsValidForAdvancedFind = false means the entity won\'t appear in Advanced Find an" +
    "d personal views cannot be created for it.";
            // 
            // tlpEntity_tlpCommands
            // 
            this.tlpEntity_tlpCommands.AutoSize = true;
            this.tlpEntity_tlpCommands.ColumnCount = 2;
            this.tlpEntity_tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpEntity_tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpEntity_tlpCommands.Controls.Add(this.tlpCommands_gbxSelect, 0, 0);
            this.tlpEntity_tlpCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEntity_tlpCommands.Location = new System.Drawing.Point(3, 33);
            this.tlpEntity_tlpCommands.Name = "tlpEntity_tlpCommands";
            this.tlpEntity_tlpCommands.RowCount = 1;
            this.tlpEntity_tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEntity_tlpCommands.Size = new System.Drawing.Size(462, 52);
            this.tlpEntity_tlpCommands.TabIndex = 5;
            // 
            // tlpCommands_gbxSelect
            // 
            this.tlpCommands_gbxSelect.AutoSize = true;
            this.tlpCommands_gbxSelect.Controls.Add(this.gbxSelect_tlpSelect);
            this.tlpCommands_gbxSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCommands_gbxSelect.Location = new System.Drawing.Point(3, 3);
            this.tlpCommands_gbxSelect.Name = "tlpCommands_gbxSelect";
            this.tlpCommands_gbxSelect.Size = new System.Drawing.Size(193, 46);
            this.tlpCommands_gbxSelect.TabIndex = 3;
            this.tlpCommands_gbxSelect.TabStop = false;
            this.tlpCommands_gbxSelect.Text = "Select";
            // 
            // gbxSelect_tlpSelect
            // 
            this.gbxSelect_tlpSelect.AutoSize = true;
            this.gbxSelect_tlpSelect.ColumnCount = 2;
            this.gbxSelect_tlpSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gbxSelect_tlpSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gbxSelect_tlpSelect.Controls.Add(this.tlpSelect_btnUnselectAll, 1, 0);
            this.gbxSelect_tlpSelect.Controls.Add(this.tlpSelect_btnSelectAll, 0, 0);
            this.gbxSelect_tlpSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxSelect_tlpSelect.Location = new System.Drawing.Point(3, 15);
            this.gbxSelect_tlpSelect.Name = "gbxSelect_tlpSelect";
            this.gbxSelect_tlpSelect.RowCount = 1;
            this.gbxSelect_tlpSelect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.gbxSelect_tlpSelect.Size = new System.Drawing.Size(187, 28);
            this.gbxSelect_tlpSelect.TabIndex = 0;
            // 
            // tlpSelect_btnUnselectAll
            // 
            this.tlpSelect_btnUnselectAll.AutoSize = true;
            this.tlpSelect_btnUnselectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSelect_btnUnselectAll.Location = new System.Drawing.Point(90, 3);
            this.tlpSelect_btnUnselectAll.Name = "tlpSelect_btnUnselectAll";
            this.tlpSelect_btnUnselectAll.Size = new System.Drawing.Size(94, 22);
            this.tlpSelect_btnUnselectAll.TabIndex = 1;
            this.tlpSelect_btnUnselectAll.Text = "Unselect All";
            this.tlpSelect_btnUnselectAll.UseVisualStyleBackColor = true;
            this.tlpSelect_btnUnselectAll.Click += new System.EventHandler(this.tlpSelect_btnUnselectAll_Click);
            // 
            // tlpSelect_btnSelectAll
            // 
            this.tlpSelect_btnSelectAll.AutoSize = true;
            this.tlpSelect_btnSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSelect_btnSelectAll.Location = new System.Drawing.Point(3, 3);
            this.tlpSelect_btnSelectAll.Name = "tlpSelect_btnSelectAll";
            this.tlpSelect_btnSelectAll.Size = new System.Drawing.Size(81, 22);
            this.tlpSelect_btnSelectAll.TabIndex = 0;
            this.tlpSelect_btnSelectAll.Text = "Select All";
            this.tlpSelect_btnSelectAll.UseVisualStyleBackColor = true;
            this.tlpSelect_btnSelectAll.Click += new System.EventHandler(this.tlpSelect_btnSelectAll_Click);
            // 
            // tlpMain_gbxView
            // 
            this.tlpMain_gbxView.AutoSize = true;
            this.tlpMain_gbxView.Controls.Add(this.gbxView_tlpViewOptions);
            this.tlpMain_gbxView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain_gbxView.Location = new System.Drawing.Point(483, 3);
            this.tlpMain_gbxView.Name = "tlpMain_gbxView";
            this.tlpMain_gbxView.Size = new System.Drawing.Size(474, 121);
            this.tlpMain_gbxView.TabIndex = 3;
            this.tlpMain_gbxView.TabStop = false;
            this.tlpMain_gbxView.Text = "View";
            // 
            // gbxView_tlpViewOptions
            // 
            this.gbxView_tlpViewOptions.AutoSize = true;
            this.gbxView_tlpViewOptions.ColumnCount = 2;
            this.gbxView_tlpViewOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gbxView_tlpViewOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gbxView_tlpViewOptions.Controls.Add(this.tlpViewOptions_lblType, 0, 0);
            this.gbxView_tlpViewOptions.Controls.Add(this.tlpViewOptions_lblName, 0, 1);
            this.gbxView_tlpViewOptions.Controls.Add(this.tlpViewOptions_lblOverwrite, 0, 2);
            this.gbxView_tlpViewOptions.Controls.Add(this.tlpViewOptions_lblFilter, 0, 3);
            this.gbxView_tlpViewOptions.Controls.Add(this.tlpViewOptions_cmbType, 1, 0);
            this.gbxView_tlpViewOptions.Controls.Add(this.tlpViewOptions_cmbOverwrite, 1, 2);
            this.gbxView_tlpViewOptions.Controls.Add(this.tlpViewOptions_cmbFilter, 1, 3);
            this.gbxView_tlpViewOptions.Controls.Add(this.tlpViewOptions_txtName, 1, 1);
            this.gbxView_tlpViewOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxView_tlpViewOptions.Location = new System.Drawing.Point(3, 15);
            this.gbxView_tlpViewOptions.Name = "gbxView_tlpViewOptions";
            this.gbxView_tlpViewOptions.RowCount = 4;
            this.gbxView_tlpViewOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxView_tlpViewOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxView_tlpViewOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxView_tlpViewOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxView_tlpViewOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.gbxView_tlpViewOptions.Size = new System.Drawing.Size(468, 103);
            this.gbxView_tlpViewOptions.TabIndex = 1;
            // 
            // tlpViewOptions_lblType
            // 
            this.tlpViewOptions_lblType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tlpViewOptions_lblType.AutoSize = true;
            this.tlpViewOptions_lblType.Location = new System.Drawing.Point(3, 7);
            this.tlpViewOptions_lblType.Name = "tlpViewOptions_lblType";
            this.tlpViewOptions_lblType.Size = new System.Drawing.Size(30, 12);
            this.tlpViewOptions_lblType.TabIndex = 0;
            this.tlpViewOptions_lblType.Text = "Type";
            // 
            // tlpViewOptions_lblName
            // 
            this.tlpViewOptions_lblName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tlpViewOptions_lblName.AutoSize = true;
            this.tlpViewOptions_lblName.Location = new System.Drawing.Point(3, 32);
            this.tlpViewOptions_lblName.Name = "tlpViewOptions_lblName";
            this.tlpViewOptions_lblName.Size = new System.Drawing.Size(34, 12);
            this.tlpViewOptions_lblName.TabIndex = 1;
            this.tlpViewOptions_lblName.Text = "Name";
            // 
            // tlpViewOptions_lblOverwrite
            // 
            this.tlpViewOptions_lblOverwrite.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tlpViewOptions_lblOverwrite.AutoSize = true;
            this.tlpViewOptions_lblOverwrite.Location = new System.Drawing.Point(3, 58);
            this.tlpViewOptions_lblOverwrite.Name = "tlpViewOptions_lblOverwrite";
            this.tlpViewOptions_lblOverwrite.Size = new System.Drawing.Size(54, 12);
            this.tlpViewOptions_lblOverwrite.TabIndex = 2;
            this.tlpViewOptions_lblOverwrite.Text = "Overwrite";
            // 
            // tlpViewOptions_lblFilter
            // 
            this.tlpViewOptions_lblFilter.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tlpViewOptions_lblFilter.AutoSize = true;
            this.tlpViewOptions_lblFilter.Location = new System.Drawing.Point(3, 84);
            this.tlpViewOptions_lblFilter.Name = "tlpViewOptions_lblFilter";
            this.tlpViewOptions_lblFilter.Size = new System.Drawing.Size(32, 12);
            this.tlpViewOptions_lblFilter.TabIndex = 3;
            this.tlpViewOptions_lblFilter.Text = "Filter";
            // 
            // tlpViewOptions_cmbType
            // 
            this.tlpViewOptions_cmbType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpViewOptions_cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tlpViewOptions_cmbType.FormattingEnabled = true;
            this.tlpViewOptions_cmbType.Items.AddRange(new object[] {
            "0: User view",
            "1: Public view"});
            this.tlpViewOptions_cmbType.Location = new System.Drawing.Point(63, 3);
            this.tlpViewOptions_cmbType.Name = "tlpViewOptions_cmbType";
            this.tlpViewOptions_cmbType.Size = new System.Drawing.Size(403, 20);
            this.tlpViewOptions_cmbType.TabIndex = 4;
            // 
            // tlpViewOptions_cmbOverwrite
            // 
            this.tlpViewOptions_cmbOverwrite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpViewOptions_cmbOverwrite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tlpViewOptions_cmbOverwrite.Enabled = false;
            this.tlpViewOptions_cmbOverwrite.FormattingEnabled = true;
            this.tlpViewOptions_cmbOverwrite.Items.AddRange(new object[] {
            "0: Disable overwrite",
            "1: Enable overwrite"});
            this.tlpViewOptions_cmbOverwrite.Location = new System.Drawing.Point(63, 54);
            this.tlpViewOptions_cmbOverwrite.Name = "tlpViewOptions_cmbOverwrite";
            this.tlpViewOptions_cmbOverwrite.Size = new System.Drawing.Size(403, 20);
            this.tlpViewOptions_cmbOverwrite.TabIndex = 6;
            // 
            // tlpViewOptions_cmbFilter
            // 
            this.tlpViewOptions_cmbFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpViewOptions_cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tlpViewOptions_cmbFilter.FormattingEnabled = true;
            this.tlpViewOptions_cmbFilter.Items.AddRange(new object[] {
            "0: All (No filter)",
            "1: Active only",
            "2: Inactive only"});
            this.tlpViewOptions_cmbFilter.Location = new System.Drawing.Point(63, 80);
            this.tlpViewOptions_cmbFilter.Name = "tlpViewOptions_cmbFilter";
            this.tlpViewOptions_cmbFilter.Size = new System.Drawing.Size(403, 20);
            this.tlpViewOptions_cmbFilter.TabIndex = 7;
            // 
            // tlpViewOptions_txtName
            // 
            this.tlpViewOptions_txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpViewOptions_txtName.Location = new System.Drawing.Point(63, 29);
            this.tlpViewOptions_txtName.MaxLength = 100;
            this.tlpViewOptions_txtName.Name = "tlpViewOptions_txtName";
            this.tlpViewOptions_txtName.Size = new System.Drawing.Size(403, 19);
            this.tlpViewOptions_txtName.TabIndex = 8;
            this.tlpViewOptions_txtName.Text = "Audit Monitoring ({entityDisplayName})";
            // 
            // tlpMain_gbxColumn
            // 
            this.tlpMain_gbxColumn.Controls.Add(this.gbxColumn_tlpColumn);
            this.tlpMain_gbxColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain_gbxColumn.Location = new System.Drawing.Point(483, 130);
            this.tlpMain_gbxColumn.Name = "tlpMain_gbxColumn";
            this.tlpMain_gbxColumn.Size = new System.Drawing.Size(474, 382);
            this.tlpMain_gbxColumn.TabIndex = 4;
            this.tlpMain_gbxColumn.TabStop = false;
            this.tlpMain_gbxColumn.Text = "Column*";
            // 
            // gbxColumn_tlpColumn
            // 
            this.gbxColumn_tlpColumn.AutoSize = true;
            this.gbxColumn_tlpColumn.ColumnCount = 1;
            this.gbxColumn_tlpColumn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.gbxColumn_tlpColumn.Controls.Add(this.tlpColumn_tlpCommands, 0, 1);
            this.gbxColumn_tlpColumn.Controls.Add(this.tlpColumn_lvwColumns, 0, 2);
            this.gbxColumn_tlpColumn.Controls.Add(this.tlpColumn_lblNote, 0, 0);
            this.gbxColumn_tlpColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxColumn_tlpColumn.Location = new System.Drawing.Point(3, 15);
            this.gbxColumn_tlpColumn.Name = "gbxColumn_tlpColumn";
            this.gbxColumn_tlpColumn.RowCount = 3;
            this.gbxColumn_tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxColumn_tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxColumn_tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxColumn_tlpColumn.Size = new System.Drawing.Size(468, 364);
            this.gbxColumn_tlpColumn.TabIndex = 0;
            // 
            // tlpColumn_tlpCommands
            // 
            this.tlpColumn_tlpCommands.AutoSize = true;
            this.tlpColumn_tlpCommands.ColumnCount = 4;
            this.tlpColumn_tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpColumn_tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpColumn_tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpColumn_tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpColumn_tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpColumn_tlpCommands.Controls.Add(this.tlpCommands_gbxPosition, 0, 0);
            this.tlpColumn_tlpCommands.Controls.Add(this.tlpCommands_gbxOrder, 1, 0);
            this.tlpColumn_tlpCommands.Controls.Add(this.tlpCommands_gbxWidth, 2, 0);
            this.tlpColumn_tlpCommands.Controls.Add(this.tlpCommands_gbxProcess, 3, 0);
            this.tlpColumn_tlpCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpColumn_tlpCommands.Location = new System.Drawing.Point(3, 30);
            this.tlpColumn_tlpCommands.Name = "tlpColumn_tlpCommands";
            this.tlpColumn_tlpCommands.RowCount = 1;
            this.tlpColumn_tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpColumn_tlpCommands.Size = new System.Drawing.Size(462, 80);
            this.tlpColumn_tlpCommands.TabIndex = 3;
            // 
            // tlpCommands_gbxPosition
            // 
            this.tlpCommands_gbxPosition.AutoSize = true;
            this.tlpCommands_gbxPosition.Controls.Add(this.gbxPosition_tlpPosition);
            this.tlpCommands_gbxPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCommands_gbxPosition.Location = new System.Drawing.Point(3, 3);
            this.tlpCommands_gbxPosition.Name = "tlpCommands_gbxPosition";
            this.tlpCommands_gbxPosition.Size = new System.Drawing.Size(74, 74);
            this.tlpCommands_gbxPosition.TabIndex = 6;
            this.tlpCommands_gbxPosition.TabStop = false;
            this.tlpCommands_gbxPosition.Text = "Position";
            // 
            // gbxPosition_tlpPosition
            // 
            this.gbxPosition_tlpPosition.AutoSize = true;
            this.gbxPosition_tlpPosition.ColumnCount = 1;
            this.gbxPosition_tlpPosition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.gbxPosition_tlpPosition.Controls.Add(this.tlpPosition_btnDown, 0, 1);
            this.gbxPosition_tlpPosition.Controls.Add(this.tlpPosition_btnUp, 0, 0);
            this.gbxPosition_tlpPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxPosition_tlpPosition.Location = new System.Drawing.Point(3, 15);
            this.gbxPosition_tlpPosition.Name = "gbxPosition_tlpPosition";
            this.gbxPosition_tlpPosition.RowCount = 2;
            this.gbxPosition_tlpPosition.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxPosition_tlpPosition.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxPosition_tlpPosition.Size = new System.Drawing.Size(68, 56);
            this.gbxPosition_tlpPosition.TabIndex = 0;
            // 
            // tlpPosition_btnDown
            // 
            this.tlpPosition_btnDown.AutoSize = true;
            this.tlpPosition_btnDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPosition_btnDown.Location = new System.Drawing.Point(3, 31);
            this.tlpPosition_btnDown.Name = "tlpPosition_btnDown";
            this.tlpPosition_btnDown.Size = new System.Drawing.Size(62, 22);
            this.tlpPosition_btnDown.TabIndex = 1;
            this.tlpPosition_btnDown.Text = "Down";
            this.tlpPosition_btnDown.UseVisualStyleBackColor = true;
            this.tlpPosition_btnDown.Click += new System.EventHandler(this.tlpPosition_btnDown_Click);
            // 
            // tlpPosition_btnUp
            // 
            this.tlpPosition_btnUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPosition_btnUp.Location = new System.Drawing.Point(3, 3);
            this.tlpPosition_btnUp.Name = "tlpPosition_btnUp";
            this.tlpPosition_btnUp.Size = new System.Drawing.Size(62, 22);
            this.tlpPosition_btnUp.TabIndex = 0;
            this.tlpPosition_btnUp.Text = "Up";
            this.tlpPosition_btnUp.UseVisualStyleBackColor = true;
            this.tlpPosition_btnUp.Click += new System.EventHandler(this.tlpPosition_btnUp_Click);
            // 
            // tlpCommands_gbxOrder
            // 
            this.tlpCommands_gbxOrder.AutoSize = true;
            this.tlpCommands_gbxOrder.Controls.Add(this.gbxOrder_tlpOrder);
            this.tlpCommands_gbxOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCommands_gbxOrder.Location = new System.Drawing.Point(83, 3);
            this.tlpCommands_gbxOrder.Name = "tlpCommands_gbxOrder";
            this.tlpCommands_gbxOrder.Size = new System.Drawing.Size(126, 74);
            this.tlpCommands_gbxOrder.TabIndex = 7;
            this.tlpCommands_gbxOrder.TabStop = false;
            this.tlpCommands_gbxOrder.Text = "Order";
            // 
            // gbxOrder_tlpOrder
            // 
            this.gbxOrder_tlpOrder.AutoSize = true;
            this.gbxOrder_tlpOrder.ColumnCount = 2;
            this.gbxOrder_tlpOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gbxOrder_tlpOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gbxOrder_tlpOrder.Controls.Add(this.tlpOrder_cmbOrder, 0, 0);
            this.gbxOrder_tlpOrder.Controls.Add(this.tlpOrder_btnClear, 1, 1);
            this.gbxOrder_tlpOrder.Controls.Add(this.tlpOrder_btnSet, 0, 1);
            this.gbxOrder_tlpOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxOrder_tlpOrder.Location = new System.Drawing.Point(3, 15);
            this.gbxOrder_tlpOrder.Name = "gbxOrder_tlpOrder";
            this.gbxOrder_tlpOrder.RowCount = 2;
            this.gbxOrder_tlpOrder.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxOrder_tlpOrder.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxOrder_tlpOrder.Size = new System.Drawing.Size(120, 56);
            this.gbxOrder_tlpOrder.TabIndex = 0;
            // 
            // tlpOrder_cmbOrder
            // 
            this.gbxOrder_tlpOrder.SetColumnSpan(this.tlpOrder_cmbOrder, 2);
            this.tlpOrder_cmbOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpOrder_cmbOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tlpOrder_cmbOrder.FormattingEnabled = true;
            this.tlpOrder_cmbOrder.Items.AddRange(new object[] {
            "↑(1)",
            "↑(2)",
            "↑(3)",
            "↓(1)",
            "↓(2)",
            "↓(3)"});
            this.tlpOrder_cmbOrder.Location = new System.Drawing.Point(3, 3);
            this.tlpOrder_cmbOrder.Name = "tlpOrder_cmbOrder";
            this.tlpOrder_cmbOrder.Size = new System.Drawing.Size(114, 20);
            this.tlpOrder_cmbOrder.TabIndex = 4;
            this.tlpOrder_cmbOrder.SelectedIndexChanged += new System.EventHandler(this.tlpOrder_cbxOrder_SelectedIndexChanged);
            // 
            // tlpOrder_btnClear
            // 
            this.tlpOrder_btnClear.AutoSize = true;
            this.tlpOrder_btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpOrder_btnClear.Location = new System.Drawing.Point(63, 29);
            this.tlpOrder_btnClear.Name = "tlpOrder_btnClear";
            this.tlpOrder_btnClear.Size = new System.Drawing.Size(54, 24);
            this.tlpOrder_btnClear.TabIndex = 6;
            this.tlpOrder_btnClear.Text = "Clear";
            this.tlpOrder_btnClear.UseVisualStyleBackColor = true;
            this.tlpOrder_btnClear.Click += new System.EventHandler(this.tlpOrder_btnClear_Click);
            // 
            // tlpOrder_btnSet
            // 
            this.tlpOrder_btnSet.AutoSize = true;
            this.tlpOrder_btnSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpOrder_btnSet.Location = new System.Drawing.Point(3, 29);
            this.tlpOrder_btnSet.Name = "tlpOrder_btnSet";
            this.tlpOrder_btnSet.Size = new System.Drawing.Size(54, 24);
            this.tlpOrder_btnSet.TabIndex = 5;
            this.tlpOrder_btnSet.Text = "Set";
            this.tlpOrder_btnSet.UseVisualStyleBackColor = true;
            this.tlpOrder_btnSet.Click += new System.EventHandler(this.tlpOrder_btnSet_Click);
            // 
            // tlpCommands_gbxWidth
            // 
            this.tlpCommands_gbxWidth.AutoSize = true;
            this.tlpCommands_gbxWidth.Controls.Add(this.gbxWidth_tlpWidth);
            this.tlpCommands_gbxWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCommands_gbxWidth.Location = new System.Drawing.Point(215, 3);
            this.tlpCommands_gbxWidth.Name = "tlpCommands_gbxWidth";
            this.tlpCommands_gbxWidth.Size = new System.Drawing.Size(74, 74);
            this.tlpCommands_gbxWidth.TabIndex = 8;
            this.tlpCommands_gbxWidth.TabStop = false;
            this.tlpCommands_gbxWidth.Text = "Width";
            // 
            // gbxWidth_tlpWidth
            // 
            this.gbxWidth_tlpWidth.AutoSize = true;
            this.gbxWidth_tlpWidth.ColumnCount = 1;
            this.gbxWidth_tlpWidth.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.gbxWidth_tlpWidth.Controls.Add(this.tlpWidth_btnSet, 0, 1);
            this.gbxWidth_tlpWidth.Controls.Add(this.tlpWidth_sudWidthValue, 0, 0);
            this.gbxWidth_tlpWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxWidth_tlpWidth.Location = new System.Drawing.Point(3, 15);
            this.gbxWidth_tlpWidth.Name = "gbxWidth_tlpWidth";
            this.gbxWidth_tlpWidth.RowCount = 2;
            this.gbxWidth_tlpWidth.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxWidth_tlpWidth.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gbxWidth_tlpWidth.Size = new System.Drawing.Size(68, 56);
            this.gbxWidth_tlpWidth.TabIndex = 0;
            // 
            // tlpWidth_btnSet
            // 
            this.tlpWidth_btnSet.AutoSize = true;
            this.tlpWidth_btnSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWidth_btnSet.Location = new System.Drawing.Point(3, 28);
            this.tlpWidth_btnSet.Name = "tlpWidth_btnSet";
            this.tlpWidth_btnSet.Size = new System.Drawing.Size(62, 25);
            this.tlpWidth_btnSet.TabIndex = 0;
            this.tlpWidth_btnSet.Text = "Set";
            this.tlpWidth_btnSet.UseVisualStyleBackColor = true;
            this.tlpWidth_btnSet.Click += new System.EventHandler(this.tlpWidth_btnSet_Click);
            // 
            // tlpWidth_sudWidthValue
            // 
            this.tlpWidth_sudWidthValue.AutoSize = true;
            this.tlpWidth_sudWidthValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWidth_sudWidthValue.Location = new System.Drawing.Point(3, 3);
            this.tlpWidth_sudWidthValue.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.tlpWidth_sudWidthValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tlpWidth_sudWidthValue.Name = "tlpWidth_sudWidthValue";
            this.tlpWidth_sudWidthValue.Size = new System.Drawing.Size(62, 19);
            this.tlpWidth_sudWidthValue.TabIndex = 1;
            this.tlpWidth_sudWidthValue.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.tlpWidth_sudWidthValue.ValueChanged += new System.EventHandler(this.tlpWidth_sudWidthValue_ValueChanged);
            // 
            // tlpCommands_gbxProcess
            // 
            this.tlpCommands_gbxProcess.AutoSize = true;
            this.tlpCommands_gbxProcess.Controls.Add(this.gbxProcess_tlpProcess);
            this.tlpCommands_gbxProcess.Dock = System.Windows.Forms.DockStyle.Right;
            this.tlpCommands_gbxProcess.Location = new System.Drawing.Point(385, 3);
            this.tlpCommands_gbxProcess.Name = "tlpCommands_gbxProcess";
            this.tlpCommands_gbxProcess.Size = new System.Drawing.Size(74, 74);
            this.tlpCommands_gbxProcess.TabIndex = 9;
            this.tlpCommands_gbxProcess.TabStop = false;
            this.tlpCommands_gbxProcess.Text = "Process";
            // 
            // gbxProcess_tlpProcess
            // 
            this.gbxProcess_tlpProcess.AutoSize = true;
            this.gbxProcess_tlpProcess.ColumnCount = 1;
            this.gbxProcess_tlpProcess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.gbxProcess_tlpProcess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.gbxProcess_tlpProcess.Controls.Add(this.tlpProcess_btnCreate, 0, 0);
            this.gbxProcess_tlpProcess.Dock = System.Windows.Forms.DockStyle.Right;
            this.gbxProcess_tlpProcess.Location = new System.Drawing.Point(3, 15);
            this.gbxProcess_tlpProcess.Name = "gbxProcess_tlpProcess";
            this.gbxProcess_tlpProcess.RowCount = 1;
            this.gbxProcess_tlpProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.gbxProcess_tlpProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.gbxProcess_tlpProcess.Size = new System.Drawing.Size(68, 56);
            this.gbxProcess_tlpProcess.TabIndex = 0;
            // 
            // tlpProcess_btnCreate
            // 
            this.tlpProcess_btnCreate.AutoSize = true;
            this.tlpProcess_btnCreate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProcess_btnCreate.Location = new System.Drawing.Point(3, 3);
            this.tlpProcess_btnCreate.Name = "tlpProcess_btnCreate";
            this.tlpProcess_btnCreate.Size = new System.Drawing.Size(62, 50);
            this.tlpProcess_btnCreate.TabIndex = 2;
            this.tlpProcess_btnCreate.Text = "Create";
            this.tlpProcess_btnCreate.UseVisualStyleBackColor = true;
            this.tlpProcess_btnCreate.Click += new System.EventHandler(this.tlpProcess_btnCreate_Click);
            // 
            // tlpColumn_lvwColumns
            // 
            this.tlpColumn_lvwColumns.CheckBoxes = true;
            this.tlpColumn_lvwColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvwColumns_colDisplayName,
            this.lvwColumns_colLogicalName,
            this.lvwColumns_colOrder,
            this.lvwColumns_colWidth});
            this.tlpColumn_lvwColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpColumn_lvwColumns.FullRowSelect = true;
            this.tlpColumn_lvwColumns.GridLines = true;
            this.tlpColumn_lvwColumns.HideSelection = false;
            listViewItem12.Checked = true;
            listViewItem12.StateImageIndex = 1;
            listViewItem13.Checked = true;
            listViewItem13.StateImageIndex = 1;
            listViewItem14.StateImageIndex = 0;
            listViewItem15.Checked = true;
            listViewItem15.StateImageIndex = 1;
            listViewItem16.Checked = true;
            listViewItem16.StateImageIndex = 1;
            listViewItem17.StateImageIndex = 0;
            listViewItem18.Checked = true;
            listViewItem18.StateImageIndex = 1;
            listViewItem19.Checked = true;
            listViewItem19.StateImageIndex = 1;
            listViewItem20.Checked = true;
            listViewItem20.StateImageIndex = 1;
            listViewItem21.Checked = true;
            listViewItem21.StateImageIndex = 1;
            listViewItem22.Checked = true;
            listViewItem22.StateImageIndex = 1;
            this.tlpColumn_lvwColumns.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17,
            listViewItem18,
            listViewItem19,
            listViewItem20,
            listViewItem21,
            listViewItem22});
            this.tlpColumn_lvwColumns.Location = new System.Drawing.Point(3, 116);
            this.tlpColumn_lvwColumns.Name = "tlpColumn_lvwColumns";
            this.tlpColumn_lvwColumns.Size = new System.Drawing.Size(462, 245);
            this.tlpColumn_lvwColumns.TabIndex = 2;
            this.tlpColumn_lvwColumns.UseCompatibleStateImageBehavior = false;
            this.tlpColumn_lvwColumns.View = System.Windows.Forms.View.Details;
            this.tlpColumn_lvwColumns.SelectedIndexChanged += new System.EventHandler(this.tlpColumn_lvwColumns_SelectedIndexChanged);
            this.tlpColumn_lvwColumns.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tlpColumn_lvwColumns_MouseDoubleClick);
            // 
            // lvwColumns_colDisplayName
            // 
            this.lvwColumns_colDisplayName.Text = "Display Name";
            this.lvwColumns_colDisplayName.Width = 150;
            // 
            // lvwColumns_colLogicalName
            // 
            this.lvwColumns_colLogicalName.Text = "Logical Name";
            this.lvwColumns_colLogicalName.Width = 150;
            // 
            // lvwColumns_colOrder
            // 
            this.lvwColumns_colOrder.Text = "Order";
            this.lvwColumns_colOrder.Width = 50;
            // 
            // lvwColumns_colWidth
            // 
            this.lvwColumns_colWidth.Text = "Width";
            this.lvwColumns_colWidth.Width = 50;
            // 
            // tlpColumn_lblNote
            // 
            this.tlpColumn_lblNote.AutoSize = true;
            this.tlpColumn_lblNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpColumn_lblNote.Location = new System.Drawing.Point(3, 3);
            this.tlpColumn_lblNote.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tlpColumn_lblNote.Name = "tlpColumn_lblNote";
            this.tlpColumn_lblNote.Size = new System.Drawing.Size(462, 24);
            this.tlpColumn_lblNote.TabIndex = 1;
            this.tlpColumn_lblNote.Text = "*If the selected field does not exist in the entity, the view will still be creat" +
    "ed, but the field will not be included.";
            // 
            // ddbExport_itemLoadXML
            // 
            this.ddbExport_itemLoadXML.Name = "ddbExport_itemLoadXML";
            this.ddbExport_itemLoadXML.Size = new System.Drawing.Size(180, 22);
            this.ddbExport_itemLoadXML.Text = "Load XML";
            this.ddbExport_itemLoadXML.Click += new System.EventHandler(this.ddbExport_itemLoadXML_Click);
            // 
            // StandardViewCreatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.tsmMain);
            this.Name = "StandardViewCreatorControl";
            this.Size = new System.Drawing.Size(960, 540);
            this.Load += new System.EventHandler(this.StandardViewCreatorControl_Load);
            this.tsmMain.ResumeLayout(false);
            this.tsmMain.PerformLayout();
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tlpMain_gbxEntity.ResumeLayout(false);
            this.gbxEntity_tlpEntity.ResumeLayout(false);
            this.gbxEntity_tlpEntity.PerformLayout();
            this.tlpEntity_tlpCommands.ResumeLayout(false);
            this.tlpEntity_tlpCommands.PerformLayout();
            this.tlpCommands_gbxSelect.ResumeLayout(false);
            this.tlpCommands_gbxSelect.PerformLayout();
            this.gbxSelect_tlpSelect.ResumeLayout(false);
            this.gbxSelect_tlpSelect.PerformLayout();
            this.tlpMain_gbxView.ResumeLayout(false);
            this.tlpMain_gbxView.PerformLayout();
            this.gbxView_tlpViewOptions.ResumeLayout(false);
            this.gbxView_tlpViewOptions.PerformLayout();
            this.tlpMain_gbxColumn.ResumeLayout(false);
            this.tlpMain_gbxColumn.PerformLayout();
            this.gbxColumn_tlpColumn.ResumeLayout(false);
            this.gbxColumn_tlpColumn.PerformLayout();
            this.tlpColumn_tlpCommands.ResumeLayout(false);
            this.tlpColumn_tlpCommands.PerformLayout();
            this.tlpCommands_gbxPosition.ResumeLayout(false);
            this.tlpCommands_gbxPosition.PerformLayout();
            this.gbxPosition_tlpPosition.ResumeLayout(false);
            this.gbxPosition_tlpPosition.PerformLayout();
            this.tlpCommands_gbxOrder.ResumeLayout(false);
            this.tlpCommands_gbxOrder.PerformLayout();
            this.gbxOrder_tlpOrder.ResumeLayout(false);
            this.gbxOrder_tlpOrder.PerformLayout();
            this.tlpCommands_gbxWidth.ResumeLayout(false);
            this.tlpCommands_gbxWidth.PerformLayout();
            this.gbxWidth_tlpWidth.ResumeLayout(false);
            this.gbxWidth_tlpWidth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlpWidth_sudWidthValue)).EndInit();
            this.tlpCommands_gbxProcess.ResumeLayout(false);
            this.tlpCommands_gbxProcess.PerformLayout();
            this.gbxProcess_tlpProcess.ResumeLayout(false);
            this.gbxProcess_tlpProcess.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip tsmMain;
        private System.Windows.Forms.ToolStripButton tsmMain_btnClose;
        private System.Windows.Forms.ToolStripButton tsmMain_btnLoadEntitiesFromSolution;
        private System.Windows.Forms.ToolStripSeparator tsmMain_sep1;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.ListView tlpEntity_lvwEntities;
        private System.Windows.Forms.ColumnHeader lvwEntities_colDisplayName;
        private System.Windows.Forms.ColumnHeader lvwEntities_colLogicalName;
        private System.Windows.Forms.ColumnHeader lvwEntities_colPrimary;
        private System.Windows.Forms.TableLayoutPanel gbxView_tlpViewOptions;
        private System.Windows.Forms.Label tlpColumn_lblNote;
        private System.Windows.Forms.ListView tlpColumn_lvwColumns;
        private System.Windows.Forms.TableLayoutPanel tlpColumn_tlpCommands;
        private System.Windows.Forms.ColumnHeader lvwColumns_colDisplayName;
        private System.Windows.Forms.ColumnHeader lvwColumns_colLogicalName;
        private System.Windows.Forms.ColumnHeader lvwColumns_colOrder;
        private System.Windows.Forms.ColumnHeader lvwColumns_colWidth;
        private System.Windows.Forms.Label tlpViewOptions_lblType;
        private System.Windows.Forms.Label tlpViewOptions_lblName;
        private System.Windows.Forms.Label tlpViewOptions_lblOverwrite;
        private System.Windows.Forms.Label tlpViewOptions_lblFilter;
        private System.Windows.Forms.ComboBox tlpViewOptions_cmbType;
        private System.Windows.Forms.ComboBox tlpViewOptions_cmbOverwrite;
        private System.Windows.Forms.ComboBox tlpViewOptions_cmbFilter;
        private System.Windows.Forms.Button tlpPosition_btnUp;
        private System.Windows.Forms.Button tlpPosition_btnDown;
        private System.Windows.Forms.Button tlpProcess_btnCreate;
        private System.Windows.Forms.TextBox tlpViewOptions_txtName;
        private System.Windows.Forms.GroupBox tlpMain_gbxEntity;
        private System.Windows.Forms.GroupBox tlpMain_gbxView;
        private System.Windows.Forms.GroupBox tlpMain_gbxColumn;
        private System.Windows.Forms.Button tlpSelect_btnSelectAll;
        private System.Windows.Forms.Button tlpSelect_btnUnselectAll;
        private System.Windows.Forms.TableLayoutPanel gbxEntity_tlpEntity;
        private System.Windows.Forms.TableLayoutPanel gbxColumn_tlpColumn;
        private System.Windows.Forms.ComboBox tlpOrder_cmbOrder;
        private System.Windows.Forms.Button tlpOrder_btnSet;
        private System.Windows.Forms.GroupBox tlpCommands_gbxPosition;
        private System.Windows.Forms.TableLayoutPanel gbxPosition_tlpPosition;
        private System.Windows.Forms.GroupBox tlpCommands_gbxOrder;
        private System.Windows.Forms.TableLayoutPanel gbxOrder_tlpOrder;
        private System.Windows.Forms.GroupBox tlpCommands_gbxWidth;
        private System.Windows.Forms.TableLayoutPanel gbxWidth_tlpWidth;
        private System.Windows.Forms.Button tlpWidth_btnSet;
        private System.Windows.Forms.NumericUpDown tlpWidth_sudWidthValue;
        private System.Windows.Forms.GroupBox tlpCommands_gbxSelect;
        private System.Windows.Forms.TableLayoutPanel gbxSelect_tlpSelect;
        private System.Windows.Forms.Button tlpOrder_btnClear;
        private System.Windows.Forms.Label tlpEntity_lblNote;
        private System.Windows.Forms.ColumnHeader lvwEntities_colIsValidForAdvancedFind;
        private System.Windows.Forms.GroupBox tlpCommands_gbxProcess;
        private System.Windows.Forms.TableLayoutPanel gbxProcess_tlpProcess;
        private System.Windows.Forms.TableLayoutPanel tlpEntity_tlpCommands;
        private System.Windows.Forms.ToolStripSeparator tsmMain_sep2;
        private System.Windows.Forms.ToolStripDropDownButton tsmMain_ddbExport;
        private System.Windows.Forms.ToolStripMenuItem ddbExport_itemAll;
        private System.Windows.Forms.ToolStripMenuItem ddbExport_itemSelectedEntities;
        private System.Windows.Forms.ToolTip cmbOverwrite_tip;
        private System.Windows.Forms.ToolTip txtName_tip;
        private System.Windows.Forms.ToolStripSeparator tsmMain_sep3;
        private System.Windows.Forms.ToolStripDropDownButton tsmMain_ddbSaveAs;
        private System.Windows.Forms.ToolStripMenuItem ddbSaveAs_itemPublic;
        private System.Windows.Forms.ToolStripMenuItem ddbSaveAs_itemPersonal;
        private System.Windows.Forms.ToolStripMenuItem ddbExport_itemLoadXML;
    }
}
