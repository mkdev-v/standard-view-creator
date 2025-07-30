namespace StandardViewCreator
{
    partial class SolutionSelecter
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
            this.lvwSolutions = new System.Windows.Forms.ListView();
            this.lvwSolutions_colGuid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwSolutions_colDisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwSolutions_colVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwSolutions_colPublisher = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvwSolutions
            // 
            this.lvwSolutions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvwSolutions_colDisplayName,
            this.lvwSolutions_colVersion,
            this.lvwSolutions_colPublisher,
            this.lvwSolutions_colGuid});
            this.lvwSolutions.FullRowSelect = true;
            this.lvwSolutions.GridLines = true;
            this.lvwSolutions.HideSelection = false;
            this.lvwSolutions.Location = new System.Drawing.Point(12, 12);
            this.lvwSolutions.MultiSelect = false;
            this.lvwSolutions.Name = "lvwSolutions";
            this.lvwSolutions.Size = new System.Drawing.Size(560, 308);
            this.lvwSolutions.TabIndex = 0;
            this.lvwSolutions.UseCompatibleStateImageBehavior = false;
            this.lvwSolutions.View = System.Windows.Forms.View.Details;
            // 
            // lvwSolutions_colGuid
            // 
            this.lvwSolutions_colGuid.Text = "Guid";
            this.lvwSolutions_colGuid.Width = 0;
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
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(497, 326);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(416, 326);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // SolutionSelecter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.lvwSolutions);
            this.Name = "SolutionSelecter";
            this.Text = "SolutionSelecter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwSolutions;
        private System.Windows.Forms.ColumnHeader lvwSolutions_colDisplayName;
        private System.Windows.Forms.ColumnHeader lvwSolutions_colVersion;
        private System.Windows.Forms.ColumnHeader lvwSolutions_colPublisher;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ColumnHeader lvwSolutions_colGuid;
    }
}