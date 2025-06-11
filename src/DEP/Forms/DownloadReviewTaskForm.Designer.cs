namespace DEP
{
    partial class DownloadReviewTaskForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadReviewTaskForm));
            this.taskTitleLabel = new System.Windows.Forms.Label();
            this.submissionInfoLabel = new System.Windows.Forms.Label();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.openFileButton = new System.Windows.Forms.Button();
            this.approveButton = new System.Windows.Forms.Button();
            this.rejectButton = new System.Windows.Forms.Button();
            this.commentsTextBox = new System.Windows.Forms.TextBox();
            this.commentsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // taskTitleLabel
            // 
            this.taskTitleLabel.AutoSize = true;
            this.taskTitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.taskTitleLabel.Location = new System.Drawing.Point(12, 9);
            this.taskTitleLabel.Name = "taskTitleLabel";
            this.taskTitleLabel.Size = new System.Drawing.Size(96, 21);
            this.taskTitleLabel.TabIndex = 0;
            this.taskTitleLabel.Text = "Задание: {0}";
            // 
            // submissionInfoLabel
            // 
            this.submissionInfoLabel.AutoSize = true;
            this.submissionInfoLabel.Location = new System.Drawing.Point(12, 40);
            this.submissionInfoLabel.Name = "submissionInfoLabel";
            this.submissionInfoLabel.Size = new System.Drawing.Size(76, 13);
            this.submissionInfoLabel.TabIndex = 1;
            this.submissionInfoLabel.Text = "Отправил: {0}";
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(12, 70);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.ReadOnly = true;
            this.filePathTextBox.Size = new System.Drawing.Size(300, 20);
            this.filePathTextBox.TabIndex = 2;
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(318, 68);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(75, 23);
            this.openFileButton.TabIndex = 3;
            this.openFileButton.Text = "Открыть";
            this.openFileButton.UseVisualStyleBackColor = true;
            // 
            // approveButton
            // 
            this.approveButton.Location = new System.Drawing.Point(237, 200);
            this.approveButton.Name = "approveButton";
            this.approveButton.Size = new System.Drawing.Size(75, 23);
            this.approveButton.TabIndex = 4;
            this.approveButton.Text = "Принять";
            this.approveButton.UseVisualStyleBackColor = true;
            // 
            // rejectButton
            // 
            this.rejectButton.Location = new System.Drawing.Point(156, 200);
            this.rejectButton.Name = "rejectButton";
            this.rejectButton.Size = new System.Drawing.Size(75, 23);
            this.rejectButton.TabIndex = 5;
            this.rejectButton.Text = "Отклонить";
            this.rejectButton.UseVisualStyleBackColor = true;
            // 
            // commentsTextBox
            // 
            this.commentsTextBox.Location = new System.Drawing.Point(12, 120);
            this.commentsTextBox.Multiline = true;
            this.commentsTextBox.Name = "commentsTextBox";
            this.commentsTextBox.Size = new System.Drawing.Size(381, 60);
            this.commentsTextBox.TabIndex = 6;
            // 
            // commentsLabel
            // 
            this.commentsLabel.AutoSize = true;
            this.commentsLabel.Location = new System.Drawing.Point(12, 104);
            this.commentsLabel.Name = "commentsLabel";
            this.commentsLabel.Size = new System.Drawing.Size(80, 13);
            this.commentsLabel.TabIndex = 7;
            this.commentsLabel.Text = "Комментарии:";
            // 
            // DownloadReviewTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 235);
            this.Controls.Add(this.commentsLabel);
            this.Controls.Add(this.commentsTextBox);
            this.Controls.Add(this.rejectButton);
            this.Controls.Add(this.approveButton);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.submissionInfoLabel);
            this.Controls.Add(this.taskTitleLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DownloadReviewTaskForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Проверка задания";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label taskTitleLabel;
        private System.Windows.Forms.Label submissionInfoLabel;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.Button approveButton;
        private System.Windows.Forms.Button rejectButton;
        private System.Windows.Forms.TextBox commentsTextBox;
        private System.Windows.Forms.Label commentsLabel;
    }
}