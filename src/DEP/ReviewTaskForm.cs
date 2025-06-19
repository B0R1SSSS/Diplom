using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using DEP.Models;
using DEP.Database;

namespace DEP
{
    /// <summary>
    /// Form for reviewing submitted tasks
    /// Форма для проверки отправленных работ
    /// </summary>
    public partial class ReviewTaskForm : Form
    {
        private readonly DatabaseManager dbManager;
        private readonly string examReviewDirectory;
        private TaskInfo selectedTask;
        private bool isFileSystemView;

        /// <summary>
        /// Initializes a new instance of the ReviewTaskForm
        /// Инициализирует новый экземпляр формы ReviewTaskForm
        /// </summary>
        public ReviewTaskForm()
        {
            InitializeComponent();
            dbManager = DatabaseManager.Instance;
            examReviewDirectory = Path.Combine(Application.StartupPath, "ExamReview");
            Directory.CreateDirectory(examReviewDirectory);
            isFileSystemView = false;

            // Привязываем обработчики событий
            SelectButton.Click += SelectButton_Click;
            ReviewButton.Click += ReviewButton_Click;
            CancelButton.Click += CancelButton_Click;
            loadTaskButton.Click += LoadTaskButton_Click;
            ChangeModeButton.Click += ChangeModeButton_Click;
            SubmissionGridView.SelectionChanged += SubmissionGridView_SelectionChanged;
            SelectTextBox.TextChanged += SelectTextBox_TextChanged;

            // Настраиваем колонки SubmissionGridView
            SetupSubmissionGridView();

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
            SubmissionGridView.Columns.Add("TaskId", "ID");
            SubmissionGridView.Columns.Add("TaskTitle", "Название");
            SubmissionGridView.Columns.Add("SubmitterName", "Студент");
            SubmissionGridView.Columns.Add("SubmissionDate", "Дата отправки");
            SubmissionGridView.Columns.Add("Status", "Статус");
        }

        /// <summary>
        /// Loads submissions from the database
        /// Загружает отправленные работы из базы данных
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
                        submission.TaskId,
                        submission.TaskTitle,
                        submission.SubmitterName,
                        submission.SubmissionDate.ToString("dd.MM.yyyy HH:mm"),
                        submission.Status.ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке работ: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles SelectButton click event
        /// Обрабатывает событие нажатия кнопки SelectButton
        /// </summary>
        private void SelectButton_Click(object sender, EventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Пожалуйста, выберите работу для просмотра", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Загружаем файл в директорию ExamReview
                var filePath = dbManager.DownloadSubmissionToDirectory(selectedTask.TaskId, examReviewDirectory);
                if (filePath != null)
                {
                    MessageBox.Show($"Работа студента {selectedTask.SubmitterName} успешно загружена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Открываем форму для проверки работы
                    using (var reviewForm = new DownloadReviewForm(selectedTask, filePath))
                    {
                        reviewForm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles ReviewButton click event
        /// Обрабатывает событие нажатия кнопки ReviewButton
        /// </summary>
        private void ReviewButton_Click(object sender, EventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Пожалуйста, выберите работу для проверки", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Implement review functionality
            MessageBox.Show("Функция проверки будет реализована позже", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles CancelButton click event
        /// Обрабатывает событие нажатия кнопки CancelButton
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
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
        private void ChangeModeButton_Click(object sender, EventArgs e)
        {
            isFileSystemView = !isFileSystemView;
            ChangeModeButton.Text = isFileSystemView ? "Режим БД" : "Режим ФС";
            LoadSubmissions();
        }

        /// <summary>
        /// Handles SubmissionGridView selection changed event
        /// Обрабатывает событие изменения выбора в SubmissionGridView
        /// </summary>
        private void SubmissionGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (SubmissionGridView.SelectedRows.Count > 0)
            {
                var row = SubmissionGridView.SelectedRows[0];
                selectedTask = new TaskInfo
                {
                    TaskId = Convert.ToInt32(row.Cells["TaskId"].Value),
                    TaskTitle = row.Cells["TaskTitle"].Value.ToString(),
                    SubmitterName = row.Cells["SubmitterName"].Value.ToString(),
                    SubmissionDate = Convert.ToDateTime(row.Cells["SubmissionDate"].Value),
                    Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), row.Cells["Status"].Value.ToString())
                };
            }
            else
            {
                selectedTask = null;
            }
        }

        /// <summary>
        /// Handles SelectTextBox text changed event
        /// Обрабатывает событие изменения текста в SelectTextBox
        /// </summary>
        private void SelectTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SelectTextBox.Text))
            {
                LoadSubmissions();
                return;
            }

            List<TaskInfo> submissions;
            if (int.TryParse(SelectTextBox.Text, out int searchId))
            {
                // Поиск по ID студента
                submissions = dbManager.GetSubmissionsByStudentId(searchId);
            }
            else
            {
                // Поиск по имени студента
                submissions = dbManager.GetSubmissionsByStudentName(SelectTextBox.Text);
            }

            SubmissionGridView.Rows.Clear();
            foreach (var submission in submissions)
            {
                SubmissionGridView.Rows.Add(
                    submission.TaskId,
                    submission.TaskTitle,
                    submission.SubmitterName,
                    submission.SubmissionDate.ToString("dd.MM.yyyy HH:mm"),
                    submission.Status.ToString()
                );
            }

            if (submissions.Count == 0)
            {
                MessageBox.Show("Работы не найдены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Handles form closing event
        /// Обрабатывает событие закрытия формы
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            try
            {
                // Не удаляем директорию ExamReview при закрытии формы,
                // так как она может содержать файлы, которые нужны для проверки
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при закрытии формы: {ex.Message}");
            }
        }
    }
} 