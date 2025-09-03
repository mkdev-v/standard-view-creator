namespace StandardViewCreator
{
    partial class ViewSelector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpMain_lvwViews = new System.Windows.Forms.ListView();
            this.lvwViews_colGuid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwViews_colEntity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwViews_colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwViews_colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tlpMain_btnCancel = new System.Windows.Forms.Button();
            this.tlpMain_btnOK = new System.Windows.Forms.Button();
            this.ViewSelector_tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.ViewSelector_tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain_lvwViews
            // 
            this.tlpMain_lvwViews.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvwViews_colGuid,
            this.lvwViews_colName,
            this.lvwViews_colEntity,
            this.lvwViews_colType});
            this.ViewSelector_tlpMain.SetColumnSpan(this.tlpMain_lvwViews, 2);
            this.tlpMain_lvwViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain_lvwViews.FullRowSelect = true;
            this.tlpMain_lvwViews.GridLines = true;
            this.tlpMain_lvwViews.HideSelection = false;
            this.tlpMain_lvwViews.Location = new System.Drawing.Point(6, 6);
            this.tlpMain_lvwViews.MultiSelect = false;
            this.tlpMain_lvwViews.Name = "tlpMain_lvwViews";
            this.tlpMain_lvwViews.Size = new System.Drawing.Size(572, 320);
            this.tlpMain_lvwViews.TabIndex = 0;
            this.tlpMain_lvwViews.UseCompatibleStateImageBehavior = false;
            this.tlpMain_lvwViews.View = System.Windows.Forms.View.Details;
            // 
            // lvwViews_colGuid
            // 
            this.lvwViews_colGuid.Text = "Guid";
            this.lvwViews_colGuid.Width = 0;
            // 
            // lvwViews_colEntity
            // 
            this.lvwViews_colEntity.Text = "Entity";
            this.lvwViews_colEntity.Width = 100;
            // 
            // lvwViews_colType
            // 
            this.lvwViews_colType.Text = "Type";
            this.lvwViews_colType.Width = 100;
            // 
            // lvwViews_colName
            // 
            this.lvwViews_colName.Text = "Name";
            this.lvwViews_colName.Width = 200;
            // 
            // tlpMain_btnCancel
            // 
            this.tlpMain_btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tlpMain_btnCancel.AutoSize = true;
            this.tlpMain_btnCancel.Location = new System.Drawing.Point(503, 332);
            this.tlpMain_btnCancel.Name = "tlpMain_btnCancel";
            this.tlpMain_btnCancel.Size = new System.Drawing.Size(75, 23);
            this.tlpMain_btnCancel.TabIndex = 2;
            this.tlpMain_btnCancel.Text = "Cancel";
            this.tlpMain_btnCancel.UseVisualStyleBackColor = true;
            // 
            // tlpMain_btnOK
            // 
            this.tlpMain_btnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tlpMain_btnOK.AutoSize = true;
            this.tlpMain_btnOK.Location = new System.Drawing.Point(422, 332);
            this.tlpMain_btnOK.Name = "tlpMain_btnOK";
            this.tlpMain_btnOK.Size = new System.Drawing.Size(75, 23);
            this.tlpMain_btnOK.TabIndex = 3;
            this.tlpMain_btnOK.Text = "OK";
            this.tlpMain_btnOK.UseVisualStyleBackColor = true;
            // 
            // ViewSelector_tlpMain
            // 
            this.ViewSelector_tlpMain.AutoSize = true;
            this.ViewSelector_tlpMain.ColumnCount = 2;
            this.ViewSelector_tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ViewSelector_tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ViewSelector_tlpMain.Controls.Add(this.tlpMain_btnOK, 0, 1);
            this.ViewSelector_tlpMain.Controls.Add(this.tlpMain_lvwViews, 0, 0);
            this.ViewSelector_tlpMain.Controls.Add(this.tlpMain_btnCancel, 1, 1);
            this.ViewSelector_tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewSelector_tlpMain.Location = new System.Drawing.Point(0, 0);
            this.ViewSelector_tlpMain.Name = "ViewSelector_tlpMain";
            this.ViewSelector_tlpMain.Padding = new System.Windows.Forms.Padding(3);
            this.ViewSelector_tlpMain.RowCount = 2;
            this.ViewSelector_tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ViewSelector_tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ViewSelector_tlpMain.Size = new System.Drawing.Size(584, 361);
            this.ViewSelector_tlpMain.TabIndex = 4;
            // 
            // ViewSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.ViewSelector_tlpMain);
            this.Name = "ViewSelector";
            this.Text = "ViewSelector";
            this.ViewSelector_tlpMain.ResumeLayout(false);
            this.ViewSelector_tlpMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView tlpMain_lvwViews;
        private System.Windows.Forms.ColumnHeader lvwViews_colName;
        private System.Windows.Forms.ColumnHeader lvwViews_colEntity;
        private System.Windows.Forms.ColumnHeader lvwViews_colType;
        private System.Windows.Forms.Button tlpMain_btnCancel;
        private System.Windows.Forms.Button tlpMain_btnOK;
        private System.Windows.Forms.ColumnHeader lvwViews_colGuid;
        private System.Windows.Forms.TableLayoutPanel ViewSelector_tlpMain;
    }
}