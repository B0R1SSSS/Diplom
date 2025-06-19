namespace DEP
{
    partial class AddInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddInfo));
            this.MaterialsGroupBox = new System.Windows.Forms.GroupBox();
            this.AddMaterialButton = new System.Windows.Forms.Button();
            this.SelectedFileLabel = new System.Windows.Forms.Label();
            this.SelectMaterialButton = new System.Windows.Forms.Button();
            this.TaskComboBox = new System.Windows.Forms.ComboBox();
            this.MaterialDescriptionBox = new DEP.Controls.PlaceholderTextBox();
            this.MaterialTitleBox = new DEP.Controls.PlaceholderTextBox();
            this.UsersGroupBox = new System.Windows.Forms.GroupBox();
            this.DeleteUserButton = new System.Windows.Forms.Button();
            this.UsernameToDeleteBox = new DEP.Controls.PlaceholderTextBox();
            this.AddUserButton = new System.Windows.Forms.Button();
            this.RoleComboBox = new System.Windows.Forms.ComboBox();
            this.PasswordBox = new DEP.Controls.PlaceholderTextBox();
            this.FullNameBox = new DEP.Controls.PlaceholderTextBox();
            this.UsernameBox = new DEP.Controls.PlaceholderTextBox();
            this.TasksGroupBox = new System.Windows.Forms.GroupBox();
            this.BackToReviewButton = new System.Windows.Forms.Button();
            this.AddTaskButton = new System.Windows.Forms.Button();
            this.TaskDescriptionBox = new DEP.Controls.PlaceholderTextBox();
            this.TaskNameBox = new DEP.Controls.PlaceholderTextBox();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.MaterialsGroupBox.SuspendLayout();
            this.UsersGroupBox.SuspendLayout();
            this.TasksGroupBox.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MaterialsGroupBox
            // 
            this.MaterialsGroupBox.Controls.Add(this.AddMaterialButton);
            this.MaterialsGroupBox.Controls.Add(this.SelectedFileLabel);
            this.MaterialsGroupBox.Controls.Add(this.SelectMaterialButton);
            this.MaterialsGroupBox.Controls.Add(this.TaskComboBox);
            this.MaterialsGroupBox.Controls.Add(this.MaterialDescriptionBox);
            this.MaterialsGroupBox.Controls.Add(this.MaterialTitleBox);
            this.MaterialsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MaterialsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.MaterialsGroupBox.Name = "MaterialsGroupBox";
            this.MaterialsGroupBox.Size = new System.Drawing.Size(321, 300);
            this.MaterialsGroupBox.TabIndex = 0;
            this.MaterialsGroupBox.TabStop = false;
            this.MaterialsGroupBox.Text = "Добавление методических материалов";
            // 
            // AddMaterialButton
            // 
            this.AddMaterialButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddMaterialButton.Location = new System.Drawing.Point(10, 220);
            this.AddMaterialButton.Name = "AddMaterialButton";
            this.AddMaterialButton.Size = new System.Drawing.Size(200, 25);
            this.AddMaterialButton.TabIndex = 5;
            this.AddMaterialButton.Text = "Добавить материал";
            this.AddMaterialButton.UseVisualStyleBackColor = true;
            this.AddMaterialButton.Click += new System.EventHandler(this.AddMaterialButton_Click);
            // 
            // SelectedFileLabel
            // 
            this.SelectedFileLabel.AutoSize = true;
            this.SelectedFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SelectedFileLabel.Location = new System.Drawing.Point(140, 185);
            this.SelectedFileLabel.Name = "SelectedFileLabel";
            this.SelectedFileLabel.Size = new System.Drawing.Size(113, 16);
            this.SelectedFileLabel.TabIndex = 4;
            this.SelectedFileLabel.Text = "Файл не выбран";
            // 
            // SelectMaterialButton
            // 
            this.SelectMaterialButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SelectMaterialButton.Location = new System.Drawing.Point(10, 180);
            this.SelectMaterialButton.Name = "SelectMaterialButton";
            this.SelectMaterialButton.Size = new System.Drawing.Size(120, 25);
            this.SelectMaterialButton.TabIndex = 3;
            this.SelectMaterialButton.Text = "Выбрать файл";
            this.SelectMaterialButton.UseVisualStyleBackColor = true;
            this.SelectMaterialButton.Click += new System.EventHandler(this.SelectMaterialButton_Click);
            // 
            // TaskComboBox
            // 
            this.TaskComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TaskComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TaskComboBox.FormattingEnabled = true;
            this.TaskComboBox.Location = new System.Drawing.Point(10, 140);
            this.TaskComboBox.Name = "TaskComboBox";
            this.TaskComboBox.Size = new System.Drawing.Size(300, 24);
            this.TaskComboBox.TabIndex = 2;
            // 
            // MaterialDescriptionBox
            // 
            this.MaterialDescriptionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MaterialDescriptionBox.ForeColor = System.Drawing.Color.Gray;
            this.MaterialDescriptionBox.Location = new System.Drawing.Point(10, 70);
            this.MaterialDescriptionBox.Multiline = true;
            this.MaterialDescriptionBox.Name = "MaterialDescriptionBox";
            this.MaterialDescriptionBox.PlaceholderColor = System.Drawing.Color.Gray;
            this.MaterialDescriptionBox.PlaceholderText = "Описание материала";
            this.MaterialDescriptionBox.Size = new System.Drawing.Size(300, 60);
            this.MaterialDescriptionBox.TabIndex = 1;
            this.MaterialDescriptionBox.Text = "Описание материала";
            // 
            // MaterialTitleBox
            // 
            this.MaterialTitleBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MaterialTitleBox.ForeColor = System.Drawing.Color.Gray;
            this.MaterialTitleBox.Location = new System.Drawing.Point(10, 30);
            this.MaterialTitleBox.Name = "MaterialTitleBox";
            this.MaterialTitleBox.PlaceholderColor = System.Drawing.Color.Gray;
            this.MaterialTitleBox.PlaceholderText = "Название материала";
            this.MaterialTitleBox.Size = new System.Drawing.Size(300, 22);
            this.MaterialTitleBox.TabIndex = 0;
            this.MaterialTitleBox.Text = "Название материала";
            // 
            // UsersGroupBox
            // 
            this.UsersGroupBox.Controls.Add(this.DeleteUserButton);
            this.UsersGroupBox.Controls.Add(this.UsernameToDeleteBox);
            this.UsersGroupBox.Controls.Add(this.AddUserButton);
            this.UsersGroupBox.Controls.Add(this.RoleComboBox);
            this.UsersGroupBox.Controls.Add(this.PasswordBox);
            this.UsersGroupBox.Controls.Add(this.FullNameBox);
            this.UsersGroupBox.Controls.Add(this.UsernameBox);
            this.UsersGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.UsersGroupBox.Location = new System.Drawing.Point(339, 12);
            this.UsersGroupBox.Name = "UsersGroupBox";
            this.UsersGroupBox.Size = new System.Drawing.Size(321, 300);
            this.UsersGroupBox.TabIndex = 1;
            this.UsersGroupBox.TabStop = false;
            this.UsersGroupBox.Text = "Управление пользователями";
            // 
            // DeleteUserButton
            // 
            this.DeleteUserButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DeleteUserButton.Location = new System.Drawing.Point(10, 260);
            this.DeleteUserButton.Name = "DeleteUserButton";
            this.DeleteUserButton.Size = new System.Drawing.Size(200, 25);
            this.DeleteUserButton.TabIndex = 6;
            this.DeleteUserButton.Text = "Удалить пользователя";
            this.DeleteUserButton.UseVisualStyleBackColor = true;
            this.DeleteUserButton.Click += new System.EventHandler(this.DeleteUserButton_Click);
            // 
            // UsernameToDeleteBox
            // 
            this.UsernameToDeleteBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.UsernameToDeleteBox.ForeColor = System.Drawing.Color.Gray;
            this.UsernameToDeleteBox.Location = new System.Drawing.Point(10, 230);
            this.UsernameToDeleteBox.Name = "UsernameToDeleteBox";
            this.UsernameToDeleteBox.PlaceholderColor = System.Drawing.Color.Gray;
            this.UsernameToDeleteBox.PlaceholderText = "Логин пользователя для удаления";
            this.UsernameToDeleteBox.Size = new System.Drawing.Size(300, 22);
            this.UsernameToDeleteBox.TabIndex = 5;
            this.UsernameToDeleteBox.Text = "Логин пользователя для удаления";
            // 
            // AddUserButton
            // 
            this.AddUserButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddUserButton.Location = new System.Drawing.Point(10, 190);
            this.AddUserButton.Name = "AddUserButton";
            this.AddUserButton.Size = new System.Drawing.Size(200, 25);
            this.AddUserButton.TabIndex = 4;
            this.AddUserButton.Text = "Добавить пользователя";
            this.AddUserButton.UseVisualStyleBackColor = true;
            this.AddUserButton.Click += new System.EventHandler(this.AddUserButton_Click);
            // 
            // RoleComboBox
            // 
            this.RoleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RoleComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RoleComboBox.FormattingEnabled = true;
            this.RoleComboBox.Items.AddRange(new object[] {
            "Teacher",
            "Student"});
            this.RoleComboBox.Location = new System.Drawing.Point(10, 150);
            this.RoleComboBox.Name = "RoleComboBox";
            this.RoleComboBox.Size = new System.Drawing.Size(300, 24);
            this.RoleComboBox.TabIndex = 3;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PasswordBox.ForeColor = System.Drawing.Color.Gray;
            this.PasswordBox.Location = new System.Drawing.Point(10, 110);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.PlaceholderColor = System.Drawing.Color.Gray;
            this.PasswordBox.PlaceholderText = "Пароль";
            this.PasswordBox.Size = new System.Drawing.Size(300, 22);
            this.PasswordBox.TabIndex = 2;
            this.PasswordBox.Text = "Пароль";
            this.PasswordBox.UseSystemPasswordChar = true;
            // 
            // FullNameBox
            // 
            this.FullNameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FullNameBox.ForeColor = System.Drawing.Color.Gray;
            this.FullNameBox.Location = new System.Drawing.Point(10, 70);
            this.FullNameBox.Name = "FullNameBox";
            this.FullNameBox.PlaceholderColor = System.Drawing.Color.Gray;
            this.FullNameBox.PlaceholderText = "Полное имя";
            this.FullNameBox.Size = new System.Drawing.Size(300, 22);
            this.FullNameBox.TabIndex = 1;
            this.FullNameBox.Text = "Полное имя";
            // 
            // UsernameBox
            // 
            this.UsernameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.UsernameBox.ForeColor = System.Drawing.Color.Gray;
            this.UsernameBox.Location = new System.Drawing.Point(10, 30);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.PlaceholderColor = System.Drawing.Color.Gray;
            this.UsernameBox.PlaceholderText = "Имя пользователя";
            this.UsernameBox.Size = new System.Drawing.Size(300, 22);
            this.UsernameBox.TabIndex = 0;
            this.UsernameBox.Text = "Имя пользователя";
            // 
            // TasksGroupBox
            // 
            this.TasksGroupBox.Controls.Add(this.BackToReviewButton);
            this.TasksGroupBox.Controls.Add(this.AddTaskButton);
            this.TasksGroupBox.Controls.Add(this.TaskDescriptionBox);
            this.TasksGroupBox.Controls.Add(this.TaskNameBox);
            this.TasksGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TasksGroupBox.Location = new System.Drawing.Point(12, 318);
            this.TasksGroupBox.Name = "TasksGroupBox";
            this.TasksGroupBox.Size = new System.Drawing.Size(648, 200);
            this.TasksGroupBox.TabIndex = 2;
            this.TasksGroupBox.TabStop = false;
            this.TasksGroupBox.Text = "Добавление заданий";
            // 
            // BackToReviewButton
            // 
            this.BackToReviewButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BackToReviewButton.Location = new System.Drawing.Point(10, 169);
            this.BackToReviewButton.Name = "BackToReviewButton";
            this.BackToReviewButton.Size = new System.Drawing.Size(100, 25);
            this.BackToReviewButton.TabIndex = 3;
            this.BackToReviewButton.Text = "← Назад";
            this.BackToReviewButton.UseVisualStyleBackColor = true;
            this.BackToReviewButton.Click += new System.EventHandler(this.BackToReviewButton_Click);
            // 
            // AddTaskButton
            // 
            this.AddTaskButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddTaskButton.Location = new System.Drawing.Point(437, 169);
            this.AddTaskButton.Name = "AddTaskButton";
            this.AddTaskButton.Size = new System.Drawing.Size(200, 25);
            this.AddTaskButton.TabIndex = 2;
            this.AddTaskButton.Text = "Добавить задание";
            this.AddTaskButton.UseVisualStyleBackColor = true;
            this.AddTaskButton.Click += new System.EventHandler(this.AddTaskButton_Click);
            // 
            // TaskDescriptionBox
            // 
            this.TaskDescriptionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TaskDescriptionBox.ForeColor = System.Drawing.Color.Gray;
            this.TaskDescriptionBox.Location = new System.Drawing.Point(10, 70);
            this.TaskDescriptionBox.Multiline = true;
            this.TaskDescriptionBox.Name = "TaskDescriptionBox";
            this.TaskDescriptionBox.PlaceholderColor = System.Drawing.Color.Gray;
            this.TaskDescriptionBox.PlaceholderText = "Описание задания";
            this.TaskDescriptionBox.Size = new System.Drawing.Size(627, 93);
            this.TaskDescriptionBox.TabIndex = 1;
            this.TaskDescriptionBox.Text = "Описание задания";
            // 
            // TaskNameBox
            // 
            this.TaskNameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TaskNameBox.ForeColor = System.Drawing.Color.Gray;
            this.TaskNameBox.Location = new System.Drawing.Point(10, 30);
            this.TaskNameBox.Name = "TaskNameBox";
            this.TaskNameBox.PlaceholderColor = System.Drawing.Color.Gray;
            this.TaskNameBox.PlaceholderText = "Название задания";
            this.TaskNameBox.Size = new System.Drawing.Size(627, 22);
            this.TaskNameBox.TabIndex = 0;
            this.TaskNameBox.Text = "Название задания";
            // 
            // MainPanel
            // 
            this.MainPanel.AutoScroll = true;
            this.MainPanel.Controls.Add(this.TasksGroupBox);
            this.MainPanel.Controls.Add(this.UsersGroupBox);
            this.MainPanel.Controls.Add(this.MaterialsGroupBox);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(671, 529);
            this.MainPanel.TabIndex = 0;
            // 
            // AddInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 529);
            this.Controls.Add(this.MainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавление информации";
            this.Load += new System.EventHandler(this.AddInfo_Load);
            this.MaterialsGroupBox.ResumeLayout(false);
            this.MaterialsGroupBox.PerformLayout();
            this.UsersGroupBox.ResumeLayout(false);
            this.UsersGroupBox.PerformLayout();
            this.TasksGroupBox.ResumeLayout(false);
            this.TasksGroupBox.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.GroupBox MaterialsGroupBox;
        private DEP.Controls.PlaceholderTextBox MaterialTitleBox;
        private DEP.Controls.PlaceholderTextBox MaterialDescriptionBox;
        private System.Windows.Forms.ComboBox TaskComboBox;
        private System.Windows.Forms.Button SelectMaterialButton;
        private System.Windows.Forms.Label SelectedFileLabel;
        private System.Windows.Forms.Button AddMaterialButton;
        private System.Windows.Forms.GroupBox UsersGroupBox;
        private DEP.Controls.PlaceholderTextBox UsernameBox;
        private DEP.Controls.PlaceholderTextBox FullNameBox;
        private DEP.Controls.PlaceholderTextBox PasswordBox;
        private System.Windows.Forms.ComboBox RoleComboBox;
        private System.Windows.Forms.Button AddUserButton;
        private System.Windows.Forms.Button DeleteUserButton;
        private DEP.Controls.PlaceholderTextBox UsernameToDeleteBox;
        private System.Windows.Forms.GroupBox TasksGroupBox;
        private DEP.Controls.PlaceholderTextBox TaskNameBox;
        private DEP.Controls.PlaceholderTextBox TaskDescriptionBox;
        private System.Windows.Forms.Button BackToReviewButton;
        private System.Windows.Forms.Button AddTaskButton;
    }
}