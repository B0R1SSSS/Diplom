using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using DEP.Models;
using DEP.Database;
using System.Collections.Generic;
using System.Linq;

namespace DEP
{
    /// <summary>
    /// Form for reviewing submitted tasks
    /// Форма для проверки отправленных работ
    /// </summary>
    public partial class ReviewTaskForm : Form
    {
        
        private readonly DatabaseManager dbManager;
        private readonly string examReviewDirectory = OtherMethods.ReviewPath;
        private TaskInfo selectedTask;
        private bool isFileSystemView;
        private bool isWorkSelected = false;
        private string userRole;
        private int userId;
        private PrepareModForm prepareModForm;
        private bool isPrepareModFormVisible = false;
        /// <summary>
        /// Initializes a new instance of the ReviewTaskForm
        /// Инициализирует новый экземпляр формы ReviewTaskForm
        /// </summary>
        /// <param name="userRole">Role of the user</param>
        /// <param name="userId">ID of the user</param>
        public ReviewTaskForm(string userRole, int userId)
        {
            this.userRole = userRole;
            this.userId = userId;
            
            InitializeComponent();
            dbManager = DatabaseManager.Instance;
            isFileSystemView = false;

            // Управляем доступностью кнопки ChangeModeButton в зависимости от роли
            UpdateDBInfoButton.Enabled = userRole.ToLower() != "student";
            if (!UpdateDBInfoButton.Enabled)
            {
                UpdateDBInfoButton.BackColor = Color.LightGray;
                UpdateDBInfoButton.ForeColor = Color.DarkGray;
                UpdateDBInfoButton.FlatStyle = FlatStyle.Flat;
                UpdateDBInfoButton.Cursor = Cursors.No;
            }

            // Привязываем обработчики событий
            SelectButton.Click += SelectButton_Click;
            ReviewButton.Click += ReviewButton_Click;
            DownloadTaskButton.Click += DownloadTaskButton_Click;
            SubmissionGridView.SelectionChanged += SubmissionGridView_SelectionChanged;
            
            
            // Привязываем обработчики для текстовых полей
            StudentIdTextBox.Enter += TextBox_Enter;
            StudentIdTextBox.Leave += TextBox_Leave;
            TaskIdTextBox.Enter += TextBox_Enter;
            TaskIdTextBox.Leave += TextBox_Leave;

            // Устанавливаем начальные значения для текстовых полей
            StudentIdTextBox.Text = "Введите ID студента";
            StudentIdTextBox.ForeColor = Color.Gray;
            TaskIdTextBox.Text = "Введите ID задания";
            TaskIdTextBox.ForeColor = Color.Gray;

            // Привязываем обработчик изменения текста для обоих полей
           

            // Настраиваем колонки SubmissionGridView
            SetupSubmissionGridView();

            // Инициализируем состояние кнопок
            DownloadTaskButton.Enabled = false;
            ReviewButton.Enabled = false;
            UpdateButtonStyles();

            // Загружаем данные при открытии формы
            LoadSubmissions();
        }

        /// <summary>
        /// Sets up the columns for the SubmissionGridView
        /// Настраивает колонки для SubmissionGridView
        /// </summary>
        private void SetupSubmissionGridView()
        {
            SubmissionGridView.Columns.Clear();
            SubmissionGridView.Columns.Add("SubmissionId", "ID отправки");
            SubmissionGridView.Columns.Add("StudentId", "ID студента");
            SubmissionGridView.Columns.Add("TaskId", "ID задания");
            SubmissionGridView.Columns.Add("TaskTitle", "Название задания");
            SubmissionGridView.Columns.Add("SubmitterName", "Студент");
            SubmissionGridView.Columns.Add("SubmissionDate", "Дата отправки");
            SubmissionGridView.Columns.Add("Status", "Статус");
            SubmissionGridView.Columns.Add("FilePath", "Файл");
            SubmissionGridView.Columns.Add("Comments", "Комментарии");

            // Настраиваем ширину колонок
            SubmissionGridView.Columns["SubmissionId"].Width = 80;
            SubmissionGridView.Columns["StudentId"].Width = 80;
            SubmissionGridView.Columns["TaskId"].Width = 80;
            SubmissionGridView.Columns["TaskTitle"].Width = 200;
            SubmissionGridView.Columns["SubmitterName"].Width = 150;
            SubmissionGridView.Columns["SubmissionDate"].Width = 150;
            SubmissionGridView.Columns["Status"].Width = 100;
            SubmissionGridView.Columns["FilePath"].Width = 200;
            SubmissionGridView.Columns["Comments"].Width = 200;
        }

        /// <summary>
        /// Loads submissions into the grid
        /// Загружает работы в таблицу
        /// </summary>
        private void LoadSubmissions()
        {
            try
            {
                var submissions = dbManager.GetAllSubmissions();
                SubmissionGridView.Rows.Clear();
                foreach (var submission in submissions)
                {
                    SubmissionGridView.Rows.Add(
                        submission.SubmissionId,  // ID отправки
                        submission.StudentId,     // ID студента
                        submission.TaskId,        // ID задания
                        submission.TaskTitle,     // Название задания
                        submission.SubmitterName, // Имя студента
                        submission.SubmissionDate.ToString("dd.MM.yyyy HH:mm"), // Дата отправки
                        submission.Status.ToString(), // Статус
                        submission.FilePath,      // Путь к файлу
                        submission.Comments       // Комментарии
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке работ: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates button styles based on their state
        /// Обновляет стили кнопок в зависимости от их состояния
        /// </summary>
        private void UpdateButtonStyles()
        {
            // Стиль для заблокированных кнопок
            var disabledStyle = new
            {
                BackColor = Color.LightGray,
                ForeColor = Color.DarkGray,
                FlatStyle = FlatStyle.Standard,
                Cursor = Cursors.No,
                
        };

            // Стиль для активных кнопок
            var enabledStyle = new
            {
                BackColor = SystemColors.Control,
                ForeColor = SystemColors.ControlText,
                FlatStyle = FlatStyle.Standard,
                Cursor = Cursors.Hand,
                
            };
            
            // Применяем стили к кнопкам
            DownloadTaskButton.BackColor = DownloadTaskButton.Enabled ? enabledStyle.BackColor : disabledStyle.BackColor;
            DownloadTaskButton.ForeColor = DownloadTaskButton.Enabled ? enabledStyle.ForeColor : disabledStyle.ForeColor;
            DownloadTaskButton.FlatStyle = DownloadTaskButton.Enabled ? enabledStyle.FlatStyle : disabledStyle.FlatStyle;
            DownloadTaskButton.Cursor = DownloadTaskButton.Enabled ? enabledStyle.Cursor : disabledStyle.Cursor;
            DownloadTaskButton.UseVisualStyleBackColor = true;

            ReviewButton.BackColor = ReviewButton.Enabled ? enabledStyle.BackColor : disabledStyle.BackColor;
            ReviewButton.ForeColor = ReviewButton.Enabled ? enabledStyle.ForeColor : disabledStyle.ForeColor;
            ReviewButton.FlatStyle = ReviewButton.Enabled ? enabledStyle.FlatStyle : disabledStyle.FlatStyle;
            ReviewButton.Cursor = ReviewButton.Enabled ? enabledStyle.Cursor : disabledStyle.Cursor;
            ReviewButton.UseVisualStyleBackColor = true;
        }

        /// <summary>
        /// Handles textbox focus events to show/hide placeholder text
        /// Обрабатывает события фокуса текстовых полей для показа/скрытия подсказок
        /// </summary>
        private void TextBox_Enter(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Text == "Введите ID студента" || textBox.Text == "Введите ID задания")
            {
                textBox.Text = "";
                textBox.ForeColor = SystemColors.WindowText;
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox == StudentIdTextBox)
                {
                    textBox.Text = "Введите ID студента";
                }
                else if (textBox == TaskIdTextBox)
                {
                    textBox.Text = "Введите ID задания";
                }
                textBox.ForeColor = Color.Gray;
            }
        }

        /// <summary>
        /// Handles SelectButton click event
        /// Обрабатывает событие нажатия кнопки SelectButton
        /// </summary>
        private void SelectButton_Click(object sender, EventArgs e)
        {
            if (SubmissionGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите работу для подтверждения", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = SubmissionGridView.SelectedRows[0];
            var taskId = Convert.ToInt32(selectedRow.Cells["TaskId"].Value);
            var studentId = Convert.ToInt32(selectedRow.Cells["StudentId"].Value);
            var submissionId = Convert.ToInt32(selectedRow.Cells["SubmissionId"].Value);

            try
            {
                // Получаем информацию о выбранной работе
                var taskInfo = dbManager.GetSubmissionByStudentAndTask(studentId, taskId);
                if (taskInfo != null)
                {
                    selectedTask = taskInfo;
                    isWorkSelected = true;
                    
                    // Разблокируем кнопки
                    DownloadTaskButton.Enabled = true;
                    ReviewButton.Enabled = true;
                    UpdateButtonStyles();

                    MessageBox.Show($"Работа студента {taskInfo.SubmitterName} выбрана для проверки", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось получить информацию о работе", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выборе работы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles DownloadTaskButton click event
        /// Обрабатывает событие нажатия кнопки DownloadTaskButton
        /// </summary>
        private void DownloadTaskButton_Click(object sender, EventArgs e)
        {
            if (SubmissionGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите работу для скачивания", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = SubmissionGridView.SelectedRows[0];
            var taskId = Convert.ToInt32(selectedRow.Cells["TaskId"].Value);
            var studentId = Convert.ToInt32(selectedRow.Cells["StudentId"].Value);
            var submissionId = Convert.ToInt32(selectedRow.Cells["SubmissionId"].Value);

            try
            {
                // Скачиваем файл в папку ExamReview
                var downloadedFilePath = dbManager.DownloadSubmissionToDirectory(submissionId, examReviewDirectory);
                if (downloadedFilePath != null)
                {
                    MessageBox.Show($"Файл успешно скачан в папку: {downloadedFilePath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось скачать файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при скачивании файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles ReviewButton click event
        /// Обрабатывает событие нажатия кнопки ReviewButton
        /// </summary>
        private void ReviewButton_Click(object sender, EventArgs e)
        {
            if (SubmissionGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите работу для проверки, нажатием на необходимую строчку", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = SubmissionGridView.SelectedRows[0];
            var taskId = Convert.ToInt32(selectedRow.Cells["TaskId"].Value);
            var studentId = Convert.ToInt32(selectedRow.Cells["StudentId"].Value);
            var submissionId = Convert.ToInt32(selectedRow.Cells["SubmissionId"].Value);

            try
            {
                // Получаем информацию о работе
                var taskInfo = dbManager.GetSubmissionByStudentAndTask(studentId, taskId);
                if (taskInfo != null)
                {
                    // Открываем форму для проверки
                    using (var reviewForm = new DownloadReviewTaskForm(studentId,taskInfo))
                    {
                        reviewForm.ShowDialog();
                    }
                    // Обновляем список после проверки
                    LoadSubmissions();
                }
                else
                {
                    MessageBox.Show("Не удалось получить информацию о работе", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы проверки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        

        /// <summary>
        /// Handles LoadTaskButton click event
        /// Обрабатывает событие нажатия кнопки LoadTaskButton
        /// </summary>
        private void LoadTaskButton_Click(object sender, EventArgs e)
        {
            LoadSubmissions();
        }

        /// <summary>
        /// Handles ChangeModeButton click event
        /// Обрабатывает событие нажатия кнопки ChangeModeButton
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">An EventArgs that contains no event data</param>
        private void ChangeModeButton_Click(object sender, EventArgs e)
        {
            
            var prepareModForm = new PrepareModForm(userRole, userId);
            prepareModForm.FormClosed += (s, args) => this.Show();
            prepareModForm.Show();
            this.Hide();
        }

        /// <summary>
        /// Handles SubmissionGridView selection changed event
        /// Обрабатывает событие изменения выбора в SubmissionGridView
        /// </summary>
        private void SubmissionGridView_SelectionChanged(object sender, EventArgs e)
        {
            // При изменении выбора в GridView блокируем кнопки
            isWorkSelected = false;
            DownloadTaskButton.Enabled = false;
            ReviewButton.Enabled = false;
            UpdateButtonStyles();
        }



        /// <summary>
        /// Searches submissions by student ID
        /// Ищет работы по ID студента
        /// </summary>
        private void SearchByStudentId()
        {
            string studentIdText = StudentIdTextBox.Text.Trim();
            if (studentIdText == "Введите ID студента" || !int.TryParse(studentIdText, out int studentId))
            {
                MessageBox.Show("Пожалуйста, введите корректный ID студента", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var submissions = dbManager.GetSubmissionsByStudentId(studentId);
                SubmissionGridView.Rows.Clear();
                foreach (var submission in submissions)
                {
                    SubmissionGridView.Rows.Add(
                        submission.SubmissionId,
                        submission.StudentId,
                        submission.TaskId,
                        submission.TaskTitle,
                        submission.SubmitterName,
                        submission.SubmissionDate.ToString("dd.MM.yyyy HH:mm"),
                        submission.Status.ToString(),
                        submission.FilePath,
                        submission.Comments
                    );
                }

                if (submissions.Count == 0)
                {
                    MessageBox.Show("Работы не найдены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске работ: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchByTaskId()
        {
            string taskIdText = TaskIdTextBox.Text.Trim();
            if (taskIdText == "Введите ID задания" || !int.TryParse(taskIdText, out int taskId))
            {
                MessageBox.Show("Пожалуйста, введите корректный ID задания", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var submissions = dbManager.GetSubmissionsByTaskId(taskId);
                SubmissionGridView.Rows.Clear();
                foreach (var submission in submissions)
                {
                    SubmissionGridView.Rows.Add(
                        submission.SubmissionId,
                        submission.StudentId,
                        submission.TaskId,
                        submission.TaskTitle,
                        submission.SubmitterName,
                        submission.SubmissionDate.ToString("dd.MM.yyyy HH:mm"),
                        submission.Status.ToString(),
                        submission.FilePath,
                        submission.Comments
                    );
                }

                if (submissions.Count == 0)
                {
                    MessageBox.Show("Работы не найдены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске работ: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchByBothIds()
        {
            string studentIdText = StudentIdTextBox.Text.Trim();
            string taskIdText = TaskIdTextBox.Text.Trim();

            if (studentIdText == "Введите ID студента" || !int.TryParse(studentIdText, out int studentId))
            {
                MessageBox.Show("Пожалуйста, введите корректный ID студента", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (taskIdText == "Введите ID задания" || !int.TryParse(taskIdText, out int taskId))
            {
                MessageBox.Show("Пожалуйста, введите корректный ID задания", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var submission = dbManager.GetSubmissionByStudentAndTask(studentId, taskId);
                SubmissionGridView.Rows.Clear();

                if (submission != null)
                {
                    SubmissionGridView.Rows.Add(
                        submission.SubmissionId,
                        submission.StudentId,
                        submission.TaskId,
                        submission.TaskTitle,
                        submission.SubmitterName,
                        submission.SubmissionDate.ToString("dd.MM.yyyy HH:mm"),
                        submission.Status.ToString(),
                        submission.FilePath,
                        submission.Comments
                    );
                }
                else
                {
                    MessageBox.Show("Работа не найдена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске работы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the UpdateDBInfoButton click event
        /// </summary>
        private void UpdateDBInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddInfo addinfo = new AddInfo(userId);
                addinfo.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void SortButton_Click(object sender, EventArgs e)
        {
            // Check if the text is empty or contains placeholder text
            bool isStudentIdEmpty = string.IsNullOrWhiteSpace(StudentIdTextBox.Text) ||
                                  StudentIdTextBox.Text == "Введите ID студента";
            bool isTaskIdEmpty = string.IsNullOrWhiteSpace(TaskIdTextBox.Text) ||
                               TaskIdTextBox.Text == "Введите ID задания";

            if (isStudentIdEmpty && isTaskIdEmpty)
            {
                // If both fields are empty or contain placeholder text - show all submissions
                LoadSubmissions();
            }
            else if (isStudentIdEmpty && !isTaskIdEmpty)
            {
                // If only TaskId is filled - search by task ID
                SearchByTaskId();
            }
            else if (!isStudentIdEmpty && isTaskIdEmpty)
            {
                // If only StudentId is filled - search by student ID
                SearchByStudentId();
            }
            else
            {
                // If both fields are filled - search for specific submission
                SearchByBothIds();
            }
        }
    }
}