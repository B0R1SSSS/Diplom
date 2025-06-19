namespace DEP.Forms
{
    partial class SubmitTaskForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubmitTaskForm));
            this.taskTitleLabel = new System.Windows.Forms.Label();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.submitButton = new System.Windows.Forms.Button();
            this.fileTypeLabel = new System.Windows.Forms.Label();
            this.BackToPrepareButton = new System.Windows.Forms.Button();
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
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(12, 68);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.ReadOnly = true;
            this.filePathTextBox.Size = new System.Drawing.Size(300, 20);
            this.filePathTextBox.TabIndex = 1;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(318, 68);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "Обзор...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(318, 120);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 3;
            this.submitButton.Text = "Отправить";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // fileTypeLabel
            // 
            this.fileTypeLabel.AutoSize = true;
            this.fileTypeLabel.Location = new System.Drawing.Point(13, 42);
            this.fileTypeLabel.Name = "fileTypeLabel";
            this.fileTypeLabel.Size = new System.Drawing.Size(89, 13);
            this.fileTypeLabel.TabIndex = 6;
            this.fileTypeLabel.Text = "Выберите файл:";
            // 
            // BackToPrepareButton
            // 
            this.BackToPrepareButton.Location = new System.Drawing.Point(16, 120);
            this.BackToPrepareButton.Name = "BackToPrepareButton";
            this.BackToPrepareButton.Size = new System.Drawing.Size(75, 23);
            this.BackToPrepareButton.TabIndex = 7;
            this.BackToPrepareButton.Text = "← Назад";
            this.BackToPrepareButton.UseVisualStyleBackColor = true;
            this.BackToPrepareButton.Click += new System.EventHandler(this.BackToPrepareButton_Click);
            // 
            // SubmitTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 155);
            this.Controls.Add(this.BackToPrepareButton);
            this.Controls.Add(this.fileTypeLabel);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.taskTitleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SubmitTaskForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Отправка задания";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label taskTitleLabel;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Label fileTypeLabel;
        private System.Windows.Forms.Button BackToPrepareButton;
    }
}