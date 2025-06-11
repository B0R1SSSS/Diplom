using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DEP.Database;
using DEP.Models;
using DEP.Controls;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace DEP
{
    public partial class AddInfo : Form
    {
        private DatabaseManager dbManager;
        private string tempDirectory;
        private int userId;
        private Panel mainPanel;

        public AddInfo(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            // Настройка формы
            this.MinimumSize = new Size(601, 900);
            this.MaximumSize = new Size(601, 900);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Добавление информации";

            // Создаем временную директорию для файлов
            tempDirectory = Path.Combine(Path.GetTempPath(), "DEP_Temp");
            Directory.CreateDirectory(tempDirectory);

            // Создаем панель для скролла
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            dbManager = DatabaseManager.Instance;
            InitializeComponents();

            // Добавляем панель на форму
            this.Controls.Add(mainPanel);
        }

        private void InitializeComponents()
        {
            // Группа для добавления методических материалов
            GroupBox materialsGroup = new GroupBox
            {
                Text = "Добавление методических материалов",
                Location = new Point(12, 12),
                Size = new Size(560, 300)
            };

            PlaceholderTextBox materialTitleBox = new PlaceholderTextBox
            {
                Location = new Point(10, 30),
                Size = new Size(300, 20),
                PlaceholderText = "Название материала"
            };

            PlaceholderTextBox materialDescriptionBox = new PlaceholderTextBox
            {
                Location = new Point(10, 70),
                Size = new Size(300, 60),
                Multiline = true,
                PlaceholderText = "Описание материала"
            };

            ComboBox taskComboBox = new ComboBox
            {
                Location = new Point(10, 140),
                Size = new Size(300, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Button selectMaterialButton = new Button
            {
                Text = "Выбрать файл",
                Location = new Point(10, 180),
                Size = new Size(120, 25)
            };

            Label selectedFileLabel = new Label
            {
                Location = new Point(140, 185),
                Size = new Size(300, 20),
                Text = "Файл не выбран"
            };

            Button addMaterialButton = new Button
            {
                Text = "Добавить материал",
                Location = new Point(10, 220),
                Size = new Size(200, 25)
            };

            materialsGroup.Controls.AddRange(new Control[] { 
                materialTitleBox, 
                materialDescriptionBox,
                taskComboBox,
                selectMaterialButton, 
                selectedFileLabel, 
                addMaterialButton 
            });

            // Группа для добавления пользователей
            GroupBox usersGroup = new GroupBox
            {
                Text = "Добавление пользователей",
                Location = new Point(12, 330),
                Size = new Size(560, 250)
            };

            PlaceholderTextBox usernameBox = new PlaceholderTextBox
            {
                Location = new Point(10, 30),
                Size = new Size(300, 20),
                PlaceholderText = "Имя пользователя"
            };

            PlaceholderTextBox fullNameBox = new PlaceholderTextBox
            {
                Location = new Point(10, 70),
                Size = new Size(300, 20),
                PlaceholderText = "Полное имя"
            };

            PlaceholderTextBox passwordBox = new PlaceholderTextBox
            {
                Location = new Point(10, 110),
                Size = new Size(300, 20),
                PlaceholderText = "Пароль",
                UseSystemPasswordChar = true
            };

            ComboBox roleComboBox = new ComboBox
            {
                Location = new Point(10, 150),
                Size = new Size(300, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            roleComboBox.Items.AddRange(new string[] { "Teacher", "Student" });

            Button addUserButton = new Button
            {
                Text = "Добавить пользователя",
                Location = new Point(10, 190),
                Size = new Size(200, 25)
            };

            usersGroup.Controls.AddRange(new Control[] { 
                usernameBox, 
                fullNameBox,
                passwordBox, 
                roleComboBox, 
                addUserButton 
            });

            // Группа для добавления заданий
            GroupBox tasksGroup = new GroupBox
            {
                Text = "Добавление заданий",
                Location = new Point(12, 600),
                Size = new Size(560, 250)
            };

            PlaceholderTextBox taskNameBox = new PlaceholderTextBox
            {
                Location = new Point(10, 30),
                Size = new Size(300, 20),
                PlaceholderText = "Название задания"
            };

            PlaceholderTextBox taskDescriptionBox = new PlaceholderTextBox
            {
                Location = new Point(10, 70),
                Size = new Size(300, 60),
                Multiline = true,
                PlaceholderText = "Описание задания"
            };

            Button addTaskButton = new Button
            {
                Text = "Добавить задание",
                Location = new Point(10, 150),
                Size = new Size(200, 25)
            };

            tasksGroup.Controls.AddRange(new Control[] { taskNameBox, taskDescriptionBox, addTaskButton });

            // Добавляем все группы на панель
            mainPanel.Controls.AddRange(new Control[] { materialsGroup, usersGroup, tasksGroup });

            // Обработчики событий
            selectMaterialButton.Click += (s, e) =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Все файлы|*.*";
                    openFileDialog.Title = "Выберите файл методического материала";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string tempPath = Path.Combine(tempDirectory, Path.GetFileName(openFileDialog.FileName));
                            File.Copy(openFileDialog.FileName, tempPath, true);
                            materialTitleBox.Tag = tempPath;
                            selectedFileLabel.Text = Path.GetFileName(openFileDialog.FileName);
                            MessageBox.Show("Файл успешно выбран", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };

            addMaterialButton.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(materialTitleBox.Text) || materialTitleBox.Text == materialTitleBox.PlaceholderText)
                {
                    MessageBox.Show("Введите название материала", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (taskComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите задание", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (materialTitleBox.Tag == null)
                {
                    MessageBox.Show("Выберите файл", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    string filePath = materialTitleBox.Tag.ToString();
                    var selectedTask = (TaskAdd)taskComboBox.SelectedItem;
                    if (dbManager.AddStudyMaterial(
                        materialTitleBox.Text,
                        materialDescriptionBox.Text,
                        filePath,
                        userId,
                        selectedTask.TaskId))
                    {
                        MessageBox.Show("Материал успешно добавлен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        materialTitleBox.Clear();
                        materialDescriptionBox.Clear();
                        materialTitleBox.Tag = null;
                        selectedFileLabel.Text = "Файл не выбран";
                        taskComboBox.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении материала: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            addUserButton.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(usernameBox.Text) || usernameBox.Text == usernameBox.PlaceholderText)
                {
                    MessageBox.Show("Введите имя пользователя", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(fullNameBox.Text) || fullNameBox.Text == fullNameBox.PlaceholderText)
                {
                    MessageBox.Show("Введите полное имя", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(passwordBox.Text) || passwordBox.Text == passwordBox.PlaceholderText)
                {
                    MessageBox.Show("Введите пароль", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (roleComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите роль", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if (dbManager.AddUser(usernameBox.Text, passwordBox.Text, fullNameBox.Text, roleComboBox.SelectedItem.ToString()))
                    {
                        MessageBox.Show("Пользователь успешно добавлен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        usernameBox.Clear();
                        fullNameBox.Clear();
                        passwordBox.Clear();
                        roleComboBox.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            addTaskButton.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(taskNameBox.Text) || taskNameBox.Text == taskNameBox.PlaceholderText ||
                    string.IsNullOrWhiteSpace(taskDescriptionBox.Text) || taskDescriptionBox.Text == taskDescriptionBox.PlaceholderText)
                {
                    MessageBox.Show("Заполните все поля", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if (dbManager.AddTask(taskNameBox.Text, taskDescriptionBox.Text, userId))
                    {
                        MessageBox.Show("Задание успешно добавлено", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        taskNameBox.Clear();
                        taskDescriptionBox.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении задания: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // Загрузка списка заданий при открытии формы
            this.Load += (s, e) =>
            {
                try
                {
                    var tasks = dbManager.GetAllTasks();
                    taskComboBox.DisplayMember = "Title";
                    taskComboBox.ValueMember = "TaskId";
                    taskComboBox.DataSource = tasks;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке списка заданий: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Удаляем временную директорию при закрытии формы
            if (Directory.Exists(tempDirectory))
            {
                try
                {
                    Directory.Delete(tempDirectory, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении временных файлов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
