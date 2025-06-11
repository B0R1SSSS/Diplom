namespace DEP
{
    partial class ReviewTaskForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReviewTaskForm));
            this.UpdateDBInfoButton = new System.Windows.Forms.Button();
            this.StudentIdTextBox = new System.Windows.Forms.TextBox();
            this.DownloadTaskButton = new System.Windows.Forms.Button();
            this.SelectButton = new System.Windows.Forms.Button();
            this.ReviewButton = new System.Windows.Forms.Button();
            this.SubmissionGridView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TaskIdTextBox = new System.Windows.Forms.TextBox();
            this.ChangeModeButton = new System.Windows.Forms.Button();
            this.SortButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SubmissionGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateDBInfoButton
            // 
            this.UpdateDBInfoButton.BackColor = System.Drawing.SystemColors.Control;
            this.UpdateDBInfoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.UpdateDBInfoButton.Location = new System.Drawing.Point(789, 12);
            this.UpdateDBInfoButton.Name = "UpdateDBInfoButton";
            this.UpdateDBInfoButton.Size = new System.Drawing.Size(264, 129);
            this.UpdateDBInfoButton.TabIndex = 3;
            this.UpdateDBInfoButton.Text = "Обновить данные в базе";
            this.UpdateDBInfoButton.UseVisualStyleBackColor = true;
            this.UpdateDBInfoButton.Click += new System.EventHandler(this.UpdateDBInfoButton_Click);
            // 
            // StudentIdTextBox
            // 
            this.StudentIdTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.StudentIdTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StudentIdTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.55F);
            this.StudentIdTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.StudentIdTextBox.Location = new System.Drawing.Point(12, 12);
            this.StudentIdTextBox.Name = "StudentIdTextBox";
            this.StudentIdTextBox.Size = new System.Drawing.Size(372, 39);
            this.StudentIdTextBox.TabIndex = 4;
            // 
            // DownloadTaskButton
            // 
            this.DownloadTaskButton.BackColor = System.Drawing.SystemColors.Control;
            this.DownloadTaskButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.DownloadTaskButton.Location = new System.Drawing.Point(390, 57);
            this.DownloadTaskButton.Name = "DownloadTaskButton";
            this.DownloadTaskButton.Size = new System.Drawing.Size(393, 39);
            this.DownloadTaskButton.TabIndex = 5;
            this.DownloadTaskButton.Text = "Скачать выбранный файл";
            this.DownloadTaskButton.UseVisualStyleBackColor = true;
            // 
            // SelectButton
            // 
            this.SelectButton.BackColor = System.Drawing.SystemColors.Control;
            this.SelectButton.FlatAppearance.BorderSize = 0;
            this.SelectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.SelectButton.Location = new System.Drawing.Point(390, 102);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(393, 39);
            this.SelectButton.TabIndex = 6;
            this.SelectButton.Text = "Выбрать";
            this.SelectButton.UseVisualStyleBackColor = true;
            // 
            // ReviewButton
            // 
            this.ReviewButton.BackColor = System.Drawing.SystemColors.Control;
            this.ReviewButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ReviewButton.Location = new System.Drawing.Point(390, 12);
            this.ReviewButton.Name = "ReviewButton";
            this.ReviewButton.Size = new System.Drawing.Size(393, 39);
            this.ReviewButton.TabIndex = 8;
            this.ReviewButton.Text = "Оставить рецензию";
            this.ReviewButton.UseVisualStyleBackColor = true;
            // 
            // SubmissionGridView
            // 
            this.SubmissionGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SubmissionGridView.Location = new System.Drawing.Point(3, 3);
            this.SubmissionGridView.Name = "SubmissionGridView";
            this.SubmissionGridView.Size = new System.Drawing.Size(1288, 716);
            this.SubmissionGridView.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SubmissionGridView);
            this.panel1.Location = new System.Drawing.Point(12, 147);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1294, 720);
            this.panel1.TabIndex = 12;
            // 
            // TaskIdTextBox
            // 
            this.TaskIdTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.TaskIdTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TaskIdTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.55F);
            this.TaskIdTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TaskIdTextBox.Location = new System.Drawing.Point(12, 57);
            this.TaskIdTextBox.Name = "TaskIdTextBox";
            this.TaskIdTextBox.Size = new System.Drawing.Size(372, 39);
            this.TaskIdTextBox.TabIndex = 13;
            // 
            // ChangeModeButton
            // 
            this.ChangeModeButton.BackColor = System.Drawing.SystemColors.Control;
            this.ChangeModeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ChangeModeButton.Location = new System.Drawing.Point(1059, 12);
            this.ChangeModeButton.Name = "ChangeModeButton";
            this.ChangeModeButton.Size = new System.Drawing.Size(247, 129);
            this.ChangeModeButton.TabIndex = 13;
            this.ChangeModeButton.Text = "Сменить режим работы";
            this.ChangeModeButton.UseVisualStyleBackColor = true;
            this.ChangeModeButton.Click += new System.EventHandler(this.ChangeModeButton_Click);
            // 
            // SortButton
            // 
            this.SortButton.BackColor = System.Drawing.SystemColors.Control;
            this.SortButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.SortButton.Location = new System.Drawing.Point(12, 102);
            this.SortButton.Name = "SortButton";
            this.SortButton.Size = new System.Drawing.Size(372, 39);
            this.SortButton.TabIndex = 15;
            this.SortButton.Text = "Сортировать";
            this.SortButton.UseVisualStyleBackColor = true;
            this.SortButton.Click += new System.EventHandler(this.SortButton_Click);
            // 
            // ReviewTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1318, 879);
            this.Controls.Add(this.SortButton);
            this.Controls.Add(this.TaskIdTextBox);
            this.Controls.Add(this.ChangeModeButton);
            this.Controls.Add(this.StudentIdTextBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SelectButton);
            this.Controls.Add(this.DownloadTaskButton);
            this.Controls.Add(this.ReviewButton);
            this.Controls.Add(this.UpdateDBInfoButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReviewTaskForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DemExamPreparation";
            ((System.ComponentModel.ISupportInitialize)(this.SubmissionGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button UpdateDBInfoButton;
        private System.Windows.Forms.TextBox StudentIdTextBox;
        private System.Windows.Forms.Button DownloadTaskButton;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button ReviewButton;
        private System.Windows.Forms.DataGridView SubmissionGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox TaskIdTextBox;
        private System.Windows.Forms.Button ChangeModeButton;
        private System.Windows.Forms.Button SortButton;
    }
}