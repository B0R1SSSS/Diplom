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
    /// <summary>
    /// Form for adding information to the database
    /// Форма для добавления информации в базу данных
    /// </summary>
    public partial class AddInfo : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly string _tempDirectory;
        private readonly int _userId;

        /// <summary>
        /// Event that is raised when a user is successfully deleted
        /// Used to notify other forms that the data has changed and needs to be refreshed
        /// Событие, которое возникает при успешном удалении пользователя
        /// Используется для уведомления других форм о том, что данные изменились и требуют обновления
        /// </summary>
        public event EventHandler UserDeleted;

        /// <summary>
        /// Event that is raised when a task is successfully added
        /// Used to notify other forms that the data has changed and needs to be refreshed
        /// Событие, которое возникает при успешном добавлении задания
        /// Используется для уведомления других форм о том, что данные изменились и требуют обновления
        /// </summary>
        public event EventHandler TaskAdded;

        /// <summary>
        /// Event that is raised when a study material is successfully added
        /// Used to notify other forms that the data has changed and needs to be refreshed
        /// Событие, которое возникает при успешном добавлении материала
        /// Используется для уведомления других форм о том, что данные изменились и требуют обновления
        /// </summary>
        public event EventHandler MaterialAdded;

        /// <summary>
        /// Event that is raised when a user is successfully added
        /// Used to notify other forms that the data has changed and needs to be refreshed
        /// Событие, которое возникает при успешном добавлении пользователя
        /// Используется для уведомления других форм о том, что данные изменились и требуют обновления
        /// </summary>
        public event EventHandler UserAdded;

        /// <summary>
        /// Initializes a new instance of the AddInfo form
        /// </summary>
        /// <param name="userId">The ID of the current user</param>
        public AddInfo(int userId)
        {
            this.MaximizeBox = false;
            InitializeComponent();
            _userId = userId;
            _dbManager = DatabaseManager.Instance;
            
            // Create temporary directory for files
            _tempDirectory = Path.Combine(Path.GetTempPath(), "DEP_Temp");
            Directory.CreateDirectory(_tempDirectory);
        }

        /// <summary>
        /// Handles form load event
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void AddInfo_Load(object sender, EventArgs e)
        {
            try
            {
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка заданий: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the list of tasks into the ComboBox
        /// </summary>
        private void LoadTasks()
        {
            var tasks = _dbManager.GetAllTasks();
            TaskComboBox.DisplayMember = "Title";
            TaskComboBox.ValueMember = "TaskId";
            TaskComboBox.DataSource = tasks;
        }

        /// <summary>
        /// Handles SelectMaterialButton click event
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void SelectMaterialButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Все файлы|*.*";
                openFileDialog.Title = "Выберите файл методического материала";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string tempPath = Path.Combine(_tempDirectory, Path.GetFileName(openFileDialog.FileName));
                        File.Copy(openFileDialog.FileName, tempPath, true);
                        MaterialTitleBox.Tag = tempPath;
                        SelectedFileLabel.Text = Path.GetFileName(openFileDialog.FileName);
                        MessageBox.Show("Файл успешно выбран", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", 
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Handles AddMaterialButton click event
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void AddMaterialButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MaterialTitleBox.Text) || MaterialTitleBox.Text == MaterialTitleBox.PlaceholderText)
            {
                MessageBox.Show("Введите название материала", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (TaskComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите задание", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MaterialTitleBox.Tag == null)
            {
                MessageBox.Show("Выберите файл", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string filePath = MaterialTitleBox.Tag.ToString();
                var selectedTask = (TaskAdd)TaskComboBox.SelectedItem;
                if (_dbManager.AddStudyMaterial(
                    MaterialTitleBox.Text,
                    MaterialDescriptionBox.Text,
                    filePath,
                    _userId,
                    selectedTask.TaskId))
                {
                    MessageBox.Show("Материал успешно добавлен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearMaterialFields();
                    MaterialAdded?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении материала: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Clears all material input fields
        /// </summary>
        private void ClearMaterialFields()
        {
            MaterialTitleBox.Clear();
            MaterialDescriptionBox.Clear();
            MaterialTitleBox.Tag = null;
            SelectedFileLabel.Text = "Файл не выбран";
            TaskComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Handles AddUserButton click event
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void AddUserButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameBox.Text) || UsernameBox.Text == UsernameBox.PlaceholderText)
            {
                MessageBox.Show("Введите имя пользователя", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(FullNameBox.Text) || FullNameBox.Text == FullNameBox.PlaceholderText)
            {
                MessageBox.Show("Введите полное имя", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Text) || PasswordBox.Text == PasswordBox.PlaceholderText)
            {
                MessageBox.Show("Введите пароль", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_dbManager.AddUser(UsernameBox.Text, PasswordBox.Text, FullNameBox.Text, RoleComboBox.SelectedItem.ToString()))
                {
                    MessageBox.Show("Пользователь успешно добавлен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearUserFields();
                    UserAdded?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles DeleteUserButton click event
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void DeleteUserButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameToDeleteBox.Text) || UsernameToDeleteBox.Text == UsernameToDeleteBox.PlaceholderText)
            {
                MessageBox.Show("Введите логин пользователя для удаления", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string username = UsernameToDeleteBox.Text;
                
                // Show confirmation dialog
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить пользователя '{username}'?\n\n" +
                    "Это действие удалит:\n" +
                    "- Все задания, созданные пользователем\n" +
                    "- Все работы, отправленные пользователем\n" +
                    "- Все работы, проверенные пользователем\n" +
                    "- Все материалы, загруженные пользователем\n\n" +
                    "Это действие нельзя отменить!",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    if (_dbManager.DeleteUser(username))
                    {
                        MessageBox.Show("Пользователь и все связанные данные успешно удалены", 
                            "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UsernameToDeleteBox.Clear();
                        UserDeleted?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Пользователь с таким логином не найден", 
                            "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Clears all user input fields
        /// </summary>
        private void ClearUserFields()
        {
            UsernameBox.Clear();
            FullNameBox.Clear();
            PasswordBox.Clear();
            RoleComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Handles AddTaskButton click event
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskNameBox.Text) || TaskNameBox.Text == TaskNameBox.PlaceholderText ||
                string.IsNullOrWhiteSpace(TaskDescriptionBox.Text) || TaskDescriptionBox.Text == TaskDescriptionBox.PlaceholderText)
            {
                MessageBox.Show("Заполните все поля", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_dbManager.AddTask(TaskNameBox.Text, TaskDescriptionBox.Text, _userId))
                {
                    MessageBox.Show("Задание успешно добавлено", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTaskFields();
                    LoadTasks(); // Reload tasks list
                    TaskAdded?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении задания: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Clears all task input fields
        /// </summary>
        private void ClearTaskFields()
        {
            TaskNameBox.Clear();
            TaskDescriptionBox.Clear();
        }

        /// <summary>
        /// Handles form closing event
        /// </summary>
        /// <param name="e">The form closing event arguments</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Remove temporary directory when closing the form
            if (Directory.Exists(_tempDirectory))
            {
                try
                {
                    Directory.Delete(_tempDirectory, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении временных файлов: {ex.Message}", 
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Handles BackToReviewButton click event
        /// Returns user to the ReviewTaskForm
        /// Обрабатывает событие нажатия кнопки BackToReviewButton
        /// Возвращает пользователя на форму проверки работ
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void BackToReviewButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при возврате к форме подготовки: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
