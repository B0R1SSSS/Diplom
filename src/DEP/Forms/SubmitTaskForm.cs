using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DEP.Database;

namespace DEP.Forms
{
    /// <summary>
    /// Form for submitting task files
    /// </summary>
    public partial class SubmitTaskForm : Form
    {
        private readonly int userId;
        private readonly int taskId;
        private readonly string taskTitle;
        private Form activeForm = null;

        /// <summary>
        /// Initializes a new instance of the SubmitTaskForm
        /// </summary>
        /// <param name="userId">The ID of the user submitting the task</param>
        /// <param name="taskId">The ID of the task being submitted</param>
        /// <param name="taskTitle">The title of the task being submitted</param>
        public SubmitTaskForm(int userId, int taskId, string taskTitle)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.userId = userId;
            this.taskId = taskId;
            this.taskTitle = taskTitle;
            InitializeForm();
        }

        /// <summary>
        /// Initializes the form with task information
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                taskTitleLabel.Text = taskTitle;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации формы: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens a new form and closes the current one
        /// </summary>
        /// <param name="form">The form to open</param>
        

        /// <summary>
        /// Handles the browse button click event
        /// </summary>
        private void browseButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Архивы (*.zip;*.rar)|*.zip;*.rar";
                    openFileDialog.Title = "Выберите архив для отправки";
                    openFileDialog.Multiselect = false;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePathTextBox.Text = openFileDialog.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выборе файла: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Checks if the file type is valid
        /// </summary>
        /// <param name="fileType">The file extension to check</param>
        /// <returns>True if the file type is valid, false otherwise</returns>
        private bool IsValidFileType(string fileType)
        {
            string[] allowedExtensions = { ".zip", ".rar" };
            return allowedExtensions.Contains(fileType.ToLower());
        }

        /// <summary>
        /// Handles the submit button click event
        /// </summary>
        private void submitButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(filePathTextBox.Text))
                {
                    MessageBox.Show("Выберите файл для отправки", 
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!File.Exists(filePathTextBox.Text))
                {
                    MessageBox.Show("Выбранный файл не существует", 
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (SubmitTask())
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке задания: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Submits the task to the system
        /// </summary>
        /// <returns>True if submission was successful, false otherwise</returns>
        private bool SubmitTask()
        {
            try
            {
                // Проверяем тип файла
                string fileType = Path.GetExtension(filePathTextBox.Text).ToLower();
                if (!IsValidFileType(fileType))
                {
                    MessageBox.Show("Недопустимый тип файла. Разрешены только .zip и .rar архивы", 
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Читаем файл в массив байтов
                byte[] fileData = File.ReadAllBytes(filePathTextBox.Text);
                string fileName = Path.GetFileName(filePathTextBox.Text);

                // Отправляем задание в базу данных
                var dbManager = DatabaseManager.Instance;
                int submissionId = dbManager.SubmitTask(taskId, userId, fileName, fileData, fileType);

                if (submissionId > 0)
                {
                    MessageBox.Show("Задание успешно отправлено", 
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Не удалось отправить задание", 
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении задания: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Handles the cancel button click event
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при закрытии формы: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 