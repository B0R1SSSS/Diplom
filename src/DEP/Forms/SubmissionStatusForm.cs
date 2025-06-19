using System;
using System.Windows.Forms;
using DEP.Database;
using DEP.Models;

namespace DEP.Forms
{
    /// <summary>
    /// Form for displaying submission status and feedback
    /// Форма для отображения статуса отправки и обратной связи
    /// </summary>
    public partial class SubmissionStatusForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly int _studentId;
        private readonly int _taskId;
        private readonly string _taskTitle;

        /// <summary>
        /// Initializes a new instance of the SubmissionStatusForm
        /// </summary>
        /// <param name="studentId">The ID of the student</param>
        /// <param name="taskId">The ID of the task</param>
        /// <param name="taskTitle">The title of the task</param>
        public SubmissionStatusForm(int studentId, int taskId, string taskTitle)
        {
            this.MaximizeBox = false;
            InitializeComponent();
            _dbManager = DatabaseManager.Instance;
            _studentId = studentId;
            _taskId = taskId;
            _taskTitle = taskTitle;
            
            LoadSubmissionStatus();
        }

        /// <summary>
        /// Loads and displays the submission status and feedback
        /// </summary>
        private void LoadSubmissionStatus()
        {
            try
            {
                // Set form title
                this.Text = $"Статус задания: {_taskTitle}";
                
                Console.WriteLine($"Загружаем статус для студента ID: {_studentId}, задания ID: {_taskId}");
                
                // Get submission information from database
                var submission = _dbManager.GetSubmissionByStudentAndTask(_studentId, _taskId);
                
                if (submission != null)
                {
                    Console.WriteLine($"Найдена отправка: ID={submission.SubmissionId}, Статус={submission.Status}, Feedback={submission.Feedback}");
                    
                    // Display status
                    StatusLabel.Text = GetStatusDisplayText(submission.Status);
                    StatusLabel.ForeColor = GetStatusColor(submission.Status);
                    
                    // Display feedback if available
                    if (!string.IsNullOrEmpty(submission.Feedback))
                    {
                        FeedbackTextBox.Text = submission.Feedback;
                        FeedbackTextBox.ReadOnly = true;
                    }
                    else
                    {
                        FeedbackTextBox.Text = "Комментарии отсутствуют";
                        FeedbackTextBox.ReadOnly = true;
                    }
                    
                    // Display submission date
                    DateLabel.Text = $"Дата отправки: {submission.SubmissionDate:dd.MM.yyyy HH:mm}";
                }
                else
                {
                    Console.WriteLine("Отправка не найдена");
                    
                    // No submission found
                    StatusLabel.Text = "Задание не отправлено";
                    StatusLabel.ForeColor = System.Drawing.Color.Gray;
                    FeedbackTextBox.Text = "Задание еще не было отправлено";
                    FeedbackTextBox.ReadOnly = true;
                    DateLabel.Text = "Дата отправки: -";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке статуса задания: {ex.Message}");
                MessageBox.Show($"Ошибка при загрузке статуса задания: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets the display text for the submission status
        /// </summary>
        /// <param name="status">The submission status</param>
        /// <returns>Localized status text</returns>
        private string GetStatusDisplayText(SubmissionStatus status)
        {
            switch (status)
            {
                case SubmissionStatus.Pending:
                    return "В ожидании проверки";
                case SubmissionStatus.Approved:
                    return "Одобрено";
                case SubmissionStatus.Rejected:
                    return "Отклонено";
                default:
                    return "Неизвестный статус";
            }
        }

        /// <summary>
        /// Gets the color for the submission status
        /// </summary>
        /// <param name="status">The submission status</param>
        /// <returns>Color for the status</returns>
        private System.Drawing.Color GetStatusColor(SubmissionStatus status)
        {
            switch (status)
            {
                case SubmissionStatus.Pending:
                    return System.Drawing.Color.Orange;
                case SubmissionStatus.Approved:
                    return System.Drawing.Color.Green;
                case SubmissionStatus.Rejected:
                    return System.Drawing.Color.Red;
                default:
                    return System.Drawing.Color.Gray;
            }
        }

        

        /// <summary>
        /// Handles BackToPrepareButton click event
        /// Returns user to the PrepareModForm
        /// Обрабатывает событие нажатия кнопки BackToPrepareButton
        /// Возвращает пользователя на форму подготовки
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void BackToPrepareButton_Click(object sender, EventArgs e)
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