namespace DEP
{
    partial class OfflineForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfflineForm));
            this.TreeViewPanel = new System.Windows.Forms.Panel();
            this.FileViewerPanel = new System.Windows.Forms.Panel();
            this.OpenFolderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TreeViewPanel
            // 
            this.TreeViewPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.TreeViewPanel.Location = new System.Drawing.Point(12, 12);
            this.TreeViewPanel.Name = "TreeViewPanel";
            this.TreeViewPanel.Size = new System.Drawing.Size(553, 661);
            this.TreeViewPanel.TabIndex = 0;
            // 
            // FileViewerPanel
            // 
            this.FileViewerPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.FileViewerPanel.Location = new System.Drawing.Point(571, 12);
            this.FileViewerPanel.Name = "FileViewerPanel";
            this.FileViewerPanel.Size = new System.Drawing.Size(1001, 737);
            this.FileViewerPanel.TabIndex = 1;
            // 
            // OpenFolderButton
            // 
            this.OpenFolderButton.BackColor = System.Drawing.SystemColors.Control;
            this.OpenFolderButton.FlatAppearance.BorderSize = 0;
            this.OpenFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.OpenFolderButton.Location = new System.Drawing.Point(12, 679);
            this.OpenFolderButton.Name = "OpenFolderButton";
            this.OpenFolderButton.Size = new System.Drawing.Size(553, 70);
            this.OpenFolderButton.TabIndex = 3;
            this.OpenFolderButton.Text = "Открыть папку с учебными материалами";
            this.OpenFolderButton.UseVisualStyleBackColor = true;
            this.OpenFolderButton.Click += new System.EventHandler(this.OpenFolderButton_Click);
            // 
            // OfflineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1584, 761);
            this.Controls.Add(this.OpenFolderButton);
            this.Controls.Add(this.FileViewerPanel);
            this.Controls.Add(this.TreeViewPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OfflineForm";
            this.Text = "DemExamPreparation";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TreeViewPanel;
        private System.Windows.Forms.Panel FileViewerPanel;
        private System.Windows.Forms.Button OpenFolderButton;
    }
}