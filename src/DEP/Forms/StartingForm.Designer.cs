namespace DEP
{
    partial class StartingForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartingForm));
            this.LoginButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            this.labelGreating = new System.Windows.Forms.Label();
            this.PassTextBox = new System.Windows.Forms.TextBox();
            this.LoginTextBox = new System.Windows.Forms.TextBox();
            this.OfflineButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LoginButton
            // 
            this.LoginButton.BackColor = System.Drawing.SystemColors.Control;
            this.LoginButton.FlatAppearance.BorderSize = 0;
            this.LoginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.LoginButton.Location = new System.Drawing.Point(47, 276);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(183, 42);
            this.LoginButton.TabIndex = 0;
            this.LoginButton.Text = "Авторизоваться";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.SystemColors.Control;
            this.ExitButton.FlatAppearance.BorderSize = 0;
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.ExitButton.Location = new System.Drawing.Point(47, 372);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(183, 42);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Выйти";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Location = new System.Drawing.Point(74, 12);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(128, 128);
            this.LogoPictureBox.TabIndex = 2;
            this.LogoPictureBox.TabStop = false;
            // 
            // labelGreating
            // 
            this.labelGreating.AutoSize = true;
            this.labelGreating.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.labelGreating.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelGreating.Location = new System.Drawing.Point(13, 165);
            this.labelGreating.Name = "labelGreating";
            this.labelGreating.Size = new System.Drawing.Size(250, 26);
            this.labelGreating.TabIndex = 7;
            this.labelGreating.Text = "Вас приветствует DEP!";
            this.labelGreating.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PassTextBox
            // 
            this.PassTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.PassTextBox.Location = new System.Drawing.Point(47, 243);
            this.PassTextBox.Name = "PassTextBox";
            this.PassTextBox.PasswordChar = '*';
            this.PassTextBox.Size = new System.Drawing.Size(183, 27);
            this.PassTextBox.TabIndex = 8;
            // 
            // LoginTextBox
            // 
            this.LoginTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.LoginTextBox.Location = new System.Drawing.Point(47, 210);
            this.LoginTextBox.Name = "LoginTextBox";
            this.LoginTextBox.Size = new System.Drawing.Size(183, 27);
            this.LoginTextBox.TabIndex = 9;
            // 
            // OfflineButton
            // 
            this.OfflineButton.BackColor = System.Drawing.SystemColors.Control;
            this.OfflineButton.FlatAppearance.BorderSize = 0;
            this.OfflineButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.OfflineButton.Location = new System.Drawing.Point(47, 324);
            this.OfflineButton.Name = "OfflineButton";
            this.OfflineButton.Size = new System.Drawing.Size(183, 42);
            this.OfflineButton.TabIndex = 10;
            this.OfflineButton.Text = "Оффлайн-режим";
            this.OfflineButton.UseVisualStyleBackColor = true;
            this.OfflineButton.Click += new System.EventHandler(this.OfflineButton_Click);
            // 
            // StartingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(276, 426);
            this.Controls.Add(this.OfflineButton);
            this.Controls.Add(this.LoginTextBox);
            this.Controls.Add(this.PassTextBox);
            this.Controls.Add(this.labelGreating);
            this.Controls.Add(this.LogoPictureBox);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.LoginButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DEP";
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.PictureBox LogoPictureBox;
        private System.Windows.Forms.Label labelGreating;
        private System.Windows.Forms.TextBox PassTextBox;
        private System.Windows.Forms.TextBox LoginTextBox;
        private System.Windows.Forms.Button OfflineButton;
    }
}

