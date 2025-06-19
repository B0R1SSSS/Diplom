namespace DEP.Forms
{
    partial class SubmissionStatusForm
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
            this.StatusLabel = new System.Windows.Forms.Label();
            this.FeedbackTextBox = new System.Windows.Forms.TextBox();
            this.DateLabel = new System.Windows.Forms.Label();
            this.StatusGroupBox = new System.Windows.Forms.GroupBox();
            this.FeedbackGroupBox = new System.Windows.Forms.GroupBox();
            this.BackToPrepareButton = new System.Windows.Forms.Button();
            this.StatusGroupBox.SuspendLayout();
            this.FeedbackGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StatusLabel.Location = new System.Drawing.Point(20, 30);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(130, 20);
            this.StatusLabel.TabIndex = 0;
            this.StatusLabel.Text = "Статус задания";
            // 
            // FeedbackTextBox
            // 
            this.FeedbackTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FeedbackTextBox.Location = new System.Drawing.Point(20, 30);
            this.FeedbackTextBox.Multiline = true;
            this.FeedbackTextBox.Name = "FeedbackTextBox";
            this.FeedbackTextBox.ReadOnly = true;
            this.FeedbackTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.FeedbackTextBox.Size = new System.Drawing.Size(423, 120);
            this.FeedbackTextBox.TabIndex = 1;
            // 
            // DateLabel
            // 
            this.DateLabel.AutoSize = true;
            this.DateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DateLabel.Location = new System.Drawing.Point(20, 60);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(120, 17);
            this.DateLabel.TabIndex = 2;
            this.DateLabel.Text = "Дата отправки: -";
            // 
            // StatusGroupBox
            // 
            this.StatusGroupBox.Controls.Add(this.StatusLabel);
            this.StatusGroupBox.Controls.Add(this.DateLabel);
            this.StatusGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StatusGroupBox.Location = new System.Drawing.Point(20, 20);
            this.StatusGroupBox.Name = "StatusGroupBox";
            this.StatusGroupBox.Size = new System.Drawing.Size(460, 100);
            this.StatusGroupBox.TabIndex = 4;
            this.StatusGroupBox.TabStop = false;
            this.StatusGroupBox.Text = "Статус задания";
            // 
            // FeedbackGroupBox
            // 
            this.FeedbackGroupBox.Controls.Add(this.FeedbackTextBox);
            this.FeedbackGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FeedbackGroupBox.Location = new System.Drawing.Point(20, 140);
            this.FeedbackGroupBox.Name = "FeedbackGroupBox";
            this.FeedbackGroupBox.Size = new System.Drawing.Size(460, 170);
            this.FeedbackGroupBox.TabIndex = 5;
            this.FeedbackGroupBox.TabStop = false;
            this.FeedbackGroupBox.Text = "Комментарии преподавателя";
            // 
            // BackToPrepareButton
            // 
            this.BackToPrepareButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BackToPrepareButton.Location = new System.Drawing.Point(20, 323);
            this.BackToPrepareButton.Name = "BackToPrepareButton";
            this.BackToPrepareButton.Size = new System.Drawing.Size(100, 35);
            this.BackToPrepareButton.TabIndex = 6;
            this.BackToPrepareButton.Text = "← Назад";
            this.BackToPrepareButton.UseVisualStyleBackColor = true;
            this.BackToPrepareButton.Click += new System.EventHandler(this.BackToPrepareButton_Click);
            // 
            // SubmissionStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 370);
            this.Controls.Add(this.BackToPrepareButton);
            this.Controls.Add(this.FeedbackGroupBox);
            this.Controls.Add(this.StatusGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubmissionStatusForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Статус задания";
            this.StatusGroupBox.ResumeLayout(false);
            this.StatusGroupBox.PerformLayout();
            this.FeedbackGroupBox.ResumeLayout(false);
            this.FeedbackGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.TextBox FeedbackTextBox;
        private System.Windows.Forms.Label DateLabel;
        private System.Windows.Forms.GroupBox StatusGroupBox;
        private System.Windows.Forms.GroupBox FeedbackGroupBox;
        private System.Windows.Forms.Button BackToPrepareButton;
    }
} 