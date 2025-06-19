namespace DEP
{
    partial class PrepareModForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrepareModForm));
            this.TreeViewPanel = new System.Windows.Forms.Panel();
            this.FileViewerPanel = new System.Windows.Forms.Panel();
            this.OpenFolderButton = new System.Windows.Forms.Button();
            this.UnpackArchiveButton = new System.Windows.Forms.Button();
            this.PackFolderButton = new System.Windows.Forms.Button();
            this.ChangeModeButton = new System.Windows.Forms.Button();
            this.LoadFileButton = new System.Windows.Forms.Button();
            this.DownloadMaterialsButton = new System.Windows.Forms.Button();
            this.SelectTaskComboBox = new System.Windows.Forms.ComboBox();
            this.LoadFolderExamTasksButton = new System.Windows.Forms.Button();
            this.LoadFolderExamPapersButton = new System.Windows.Forms.Button();
            this.ViewStatusButton = new System.Windows.Forms.Button();
            this.BackToStartButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TreeViewPanel
            // 
            this.TreeViewPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.TreeViewPanel.Location = new System.Drawing.Point(12, 102);
            this.TreeViewPanel.Name = "TreeViewPanel";
            this.TreeViewPanel.Size = new System.Drawing.Size(550, 708);
            this.TreeViewPanel.TabIndex = 0;
            // 
            // FileViewerPanel
            // 
            this.FileViewerPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.FileViewerPanel.Location = new System.Drawing.Point(568, 102);
            this.FileViewerPanel.Name = "FileViewerPanel";
            this.FileViewerPanel.Size = new System.Drawing.Size(804, 847);
            this.FileViewerPanel.TabIndex = 1;
            // 
            // OpenFolderButton
            // 
            this.OpenFolderButton.BackColor = System.Drawing.SystemColors.Control;
            this.OpenFolderButton.FlatAppearance.BorderSize = 0;
            this.OpenFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.OpenFolderButton.Location = new System.Drawing.Point(321, 57);
            this.OpenFolderButton.Name = "OpenFolderButton";
            this.OpenFolderButton.Size = new System.Drawing.Size(303, 39);
            this.OpenFolderButton.TabIndex = 3;
            this.OpenFolderButton.Text = "Открыть папку с учебными материалами";
            this.OpenFolderButton.UseVisualStyleBackColor = true;
            this.OpenFolderButton.Click += new System.EventHandler(this.OpenFolderButton_Click);
            // 
            // UnpackArchiveButton
            // 
            this.UnpackArchiveButton.BackColor = System.Drawing.SystemColors.Control;
            this.UnpackArchiveButton.FlatAppearance.BorderSize = 0;
            this.UnpackArchiveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.UnpackArchiveButton.Location = new System.Drawing.Point(630, 12);
            this.UnpackArchiveButton.Name = "UnpackArchiveButton";
            this.UnpackArchiveButton.Size = new System.Drawing.Size(222, 39);
            this.UnpackArchiveButton.TabIndex = 5;
            this.UnpackArchiveButton.Text = "Распаковать архив в папку";
            this.UnpackArchiveButton.UseVisualStyleBackColor = true;
            this.UnpackArchiveButton.Click += new System.EventHandler(this.UnpackArchiveButton_Click);
            // 
            // PackFolderButton
            // 
            this.PackFolderButton.BackColor = System.Drawing.SystemColors.Control;
            this.PackFolderButton.FlatAppearance.BorderSize = 0;
            this.PackFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.PackFolderButton.Location = new System.Drawing.Point(858, 12);
            this.PackFolderButton.Name = "PackFolderButton";
            this.PackFolderButton.Size = new System.Drawing.Size(222, 39);
            this.PackFolderButton.TabIndex = 7;
            this.PackFolderButton.Text = "Запаковать папку в архив";
            this.PackFolderButton.UseVisualStyleBackColor = true;
            this.PackFolderButton.Click += new System.EventHandler(this.PackFolderButton_Click);
            // 
            // ChangeModeButton
            // 
            this.ChangeModeButton.BackColor = System.Drawing.SystemColors.Control;
            this.ChangeModeButton.FlatAppearance.BorderSize = 0;
            this.ChangeModeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ChangeModeButton.Location = new System.Drawing.Point(1086, 12);
            this.ChangeModeButton.Name = "ChangeModeButton";
            this.ChangeModeButton.Size = new System.Drawing.Size(286, 84);
            this.ChangeModeButton.TabIndex = 8;
            this.ChangeModeButton.Text = "Сменить режим программы";
            this.ChangeModeButton.UseVisualStyleBackColor = true;
            this.ChangeModeButton.Click += new System.EventHandler(this.ChangeModeButton_Click);
            // 
            // LoadFileButton
            // 
            this.LoadFileButton.BackColor = System.Drawing.SystemColors.Control;
            this.LoadFileButton.FlatAppearance.BorderSize = 0;
            this.LoadFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LoadFileButton.Location = new System.Drawing.Point(630, 57);
            this.LoadFileButton.Name = "LoadFileButton";
            this.LoadFileButton.Size = new System.Drawing.Size(450, 39);
            this.LoadFileButton.TabIndex = 9;
            this.LoadFileButton.Text = "Отправить архив с файлами на проверку";
            this.LoadFileButton.UseVisualStyleBackColor = true;
            this.LoadFileButton.Click += new System.EventHandler(this.LoadFileButton_Click);
            // 
            // DownloadMaterialsButton
            // 
            this.DownloadMaterialsButton.BackColor = System.Drawing.SystemColors.Control;
            this.DownloadMaterialsButton.FlatAppearance.BorderSize = 0;
            this.DownloadMaterialsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.DownloadMaterialsButton.Location = new System.Drawing.Point(12, 57);
            this.DownloadMaterialsButton.Name = "DownloadMaterialsButton";
            this.DownloadMaterialsButton.Size = new System.Drawing.Size(303, 39);
            this.DownloadMaterialsButton.TabIndex = 10;
            this.DownloadMaterialsButton.Text = "Загрузить материалы из базы данных";
            this.DownloadMaterialsButton.UseVisualStyleBackColor = true;
            this.DownloadMaterialsButton.Click += new System.EventHandler(this.DownloadMaterialsButton_Click);
            // 
            // SelectTaskComboBox
            // 
            this.SelectTaskComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.SelectTaskComboBox.FormattingEnabled = true;
            this.SelectTaskComboBox.Location = new System.Drawing.Point(12, 12);
            this.SelectTaskComboBox.Name = "SelectTaskComboBox";
            this.SelectTaskComboBox.Size = new System.Drawing.Size(303, 39);
            this.SelectTaskComboBox.TabIndex = 11;
            // 
            // LoadFolderExamTasksButton
            // 
            this.LoadFolderExamTasksButton.BackColor = System.Drawing.SystemColors.Control;
            this.LoadFolderExamTasksButton.FlatAppearance.BorderSize = 0;
            this.LoadFolderExamTasksButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LoadFolderExamTasksButton.Location = new System.Drawing.Point(12, 861);
            this.LoadFolderExamTasksButton.Name = "LoadFolderExamTasksButton";
            this.LoadFolderExamTasksButton.Size = new System.Drawing.Size(550, 39);
            this.LoadFolderExamTasksButton.TabIndex = 12;
            this.LoadFolderExamTasksButton.Text = "Задания";
            this.LoadFolderExamTasksButton.UseVisualStyleBackColor = true;
            this.LoadFolderExamTasksButton.Click += new System.EventHandler(this.LoadFolderExamTasksButton_Click);
            // 
            // LoadFolderExamPapersButton
            // 
            this.LoadFolderExamPapersButton.BackColor = System.Drawing.SystemColors.Control;
            this.LoadFolderExamPapersButton.FlatAppearance.BorderSize = 0;
            this.LoadFolderExamPapersButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LoadFolderExamPapersButton.Location = new System.Drawing.Point(12, 816);
            this.LoadFolderExamPapersButton.Name = "LoadFolderExamPapersButton";
            this.LoadFolderExamPapersButton.Size = new System.Drawing.Size(550, 39);
            this.LoadFolderExamPapersButton.TabIndex = 14;
            this.LoadFolderExamPapersButton.Text = "Учебные материалы";
            this.LoadFolderExamPapersButton.UseVisualStyleBackColor = true;
            this.LoadFolderExamPapersButton.Click += new System.EventHandler(this.LoadFolderExamPapersButton_Click);
            // 
            // ViewStatusButton
            // 
            this.ViewStatusButton.BackColor = System.Drawing.SystemColors.Control;
            this.ViewStatusButton.FlatAppearance.BorderSize = 0;
            this.ViewStatusButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ViewStatusButton.Location = new System.Drawing.Point(321, 12);
            this.ViewStatusButton.Name = "ViewStatusButton";
            this.ViewStatusButton.Size = new System.Drawing.Size(303, 39);
            this.ViewStatusButton.TabIndex = 15;
            this.ViewStatusButton.Text = "Просмотреть статус задания";
            this.ViewStatusButton.UseVisualStyleBackColor = true;
            this.ViewStatusButton.Click += new System.EventHandler(this.ViewStatusButton_Click);
            // 
            // BackToStartButton
            // 
            this.BackToStartButton.BackColor = System.Drawing.SystemColors.Control;
            this.BackToStartButton.FlatAppearance.BorderSize = 0;
            this.BackToStartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.BackToStartButton.Location = new System.Drawing.Point(12, 906);
            this.BackToStartButton.Name = "BackToStartButton";
            this.BackToStartButton.Size = new System.Drawing.Size(550, 39);
            this.BackToStartButton.TabIndex = 16;
            this.BackToStartButton.Text = "← Назад к входу";
            this.BackToStartButton.UseVisualStyleBackColor = true;
            this.BackToStartButton.Click += new System.EventHandler(this.BackToStartButton_Click);
            // 
            // PrepareModForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1384, 961);
            this.Controls.Add(this.BackToStartButton);
            this.Controls.Add(this.ViewStatusButton);
            this.Controls.Add(this.LoadFolderExamPapersButton);
            this.Controls.Add(this.LoadFolderExamTasksButton);
            this.Controls.Add(this.SelectTaskComboBox);
            this.Controls.Add(this.DownloadMaterialsButton);
            this.Controls.Add(this.LoadFileButton);
            this.Controls.Add(this.ChangeModeButton);
            this.Controls.Add(this.PackFolderButton);
            this.Controls.Add(this.UnpackArchiveButton);
            this.Controls.Add(this.OpenFolderButton);
            this.Controls.Add(this.FileViewerPanel);
            this.Controls.Add(this.TreeViewPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrepareModForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DemExamPreparation";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TreeViewPanel;
        private System.Windows.Forms.Panel FileViewerPanel;
        private System.Windows.Forms.Button OpenFolderButton;
        private System.Windows.Forms.Button UnpackArchiveButton;
        private System.Windows.Forms.Button PackFolderButton;
        private System.Windows.Forms.Button ChangeModeButton;
        private System.Windows.Forms.Button LoadFileButton;
        private System.Windows.Forms.Button DownloadMaterialsButton;
        private System.Windows.Forms.ComboBox SelectTaskComboBox;
        private System.Windows.Forms.Button LoadFolderExamTasksButton;
        private System.Windows.Forms.Button LoadFolderExamPapersButton;
        private System.Windows.Forms.Button ViewStatusButton;
        private System.Windows.Forms.Button BackToStartButton;
    }
}