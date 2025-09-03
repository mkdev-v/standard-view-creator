namespace StandardViewCreator
{
    partial class SolutionSelector
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
            this.tlpMain_lvwSolutions = new System.Windows.Forms.ListView();
            this.lvwSolutions_colDisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwSolutions_colVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwSolutions_colPublisher = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwSolutions_colGuid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tlpMain_btnCancel = new System.Windows.Forms.Button();
            this.tlpMain_btnOK = new System.Windows.Forms.Button();
            this.SolutionSelector_tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.SolutionSelector_tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain_lvwSolutions
            // 
            this.tlpMain_lvwSolutions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvwSolutions_colDisplayName,
            this.lvwSolutions_colVersion,
            this.lvwSolutions_colPublisher,
            this.lvwSolutions_colGuid});
            this.SolutionSelector_tlpMain.SetColumnSpan(this.tlpMain_lvwSolutions, 2);
            this.tlpMain_lvwSolutions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain_lvwSolutions.FullRowSelect = true;
            this.tlpMain_lvwSolutions.GridLines = true;
            this.tlpMain_lvwSolutions.HideSelection = false;
            this.tlpMain_lvwSolutions.Location = new System.Drawing.Point(6, 6);
            this.tlpMain_lvwSolutions.MultiSelect = false;
            this.tlpMain_lvwSolutions.Name = "tlpMain_lvwSolutions";
            this.tlpMain_lvwSolutions.Size = new System.Drawing.Size(572, 320);
            this.tlpMain_lvwSolutions.TabIndex = 0;
            this.tlpMain_lvwSolutions.UseCompatibleStateImageBehavior = false;
            this.tlpMain_lvwSolutions.View = System.Windows.Forms.View.Details;
            // 
            // lvwSolutions_colDisplayName
            // 
            this.lvwSolutions_colDisplayName.Text = "Display Name";
            this.lvwSolutions_colDisplayName.Width = 200;
            // 
            // lvwSolutions_colVersion
            // 
            this.lvwSolutions_colVersion.Text = "Version";
            this.lvwSolutions_colVersion.Width = 100;
            // 
            // lvwSolutions_colPublisher
            // 
            this.lvwSolutions_colPublisher.Text = "Publisher";
            this.lvwSolutions_colPublisher.Width = 200;
            // 
            // lvwSolutions_colGuid
            // 
            this.lvwSolutions_colGuid.Text = "Guid";
            this.lvwSolutions_colGuid.Width = 0;
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
            // SolutionSelector_tlpMain
            // 
            this.SolutionSelector_tlpMain.ColumnCount = 2;
            this.SolutionSelector_tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SolutionSelector_tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.SolutionSelector_tlpMain.Controls.Add(this.tlpMain_lvwSolutions, 0, 0);
            this.SolutionSelector_tlpMain.Controls.Add(this.tlpMain_btnCancel, 1, 1);
            this.SolutionSelector_tlpMain.Controls.Add(this.tlpMain_btnOK, 0, 1);
            this.SolutionSelector_tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SolutionSelector_tlpMain.Location = new System.Drawing.Point(0, 0);
            this.SolutionSelector_tlpMain.Name = "SolutionSelector_tlpMain";
            this.SolutionSelector_tlpMain.Padding = new System.Windows.Forms.Padding(3);
            this.SolutionSelector_tlpMain.RowCount = 2;
            this.SolutionSelector_tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SolutionSelector_tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.SolutionSelector_tlpMain.Size = new System.Drawing.Size(584, 361);
            this.SolutionSelector_tlpMain.TabIndex = 4;
            // 
            // SolutionSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.SolutionSelector_tlpMain);
            this.Name = "SolutionSelector";
            this.Text = "SolutionSelector";
            this.SolutionSelector_tlpMain.ResumeLayout(false);
            this.SolutionSelector_tlpMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView tlpMain_lvwSolutions;
        private System.Windows.Forms.ColumnHeader lvwSolutions_colDisplayName;
        private System.Windows.Forms.ColumnHeader lvwSolutions_colVersion;
        private System.Windows.Forms.ColumnHeader lvwSolutions_colPublisher;
        private System.Windows.Forms.Button tlpMain_btnCancel;
        private System.Windows.Forms.Button tlpMain_btnOK;
        private System.Windows.Forms.ColumnHeader lvwSolutions_colGuid;
        private System.Windows.Forms.TableLayoutPanel SolutionSelector_tlpMain;
    }
}