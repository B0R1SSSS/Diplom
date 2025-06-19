using System;
using System.Windows.Forms;
using System.Drawing;
using DEP.Database;
using DEP.Models;

namespace DEP
{
    /// <summary>
    /// Form for reviewing submitted tasks
    /// Форма для проверки отправленных работ
    /// </summary>
    public partial class DownloadReviewTaskForm : Form
    {
        private readonly int userId;
        private readonly string userRole;
        private readonly TaskInfo taskInfo;

        /// <summary>
        /// Initializes a new instance of the DownloadReviewTaskForm
        /// </summary>
        /// <param name="userId">ID of the reviewer</param>
        /// <param name="taskInfo">Information about the task to review</param>
        public DownloadReviewTaskForm(int userId, TaskInfo taskInfo)
        {
            this.MaximizeBox = false;
            InitializeComponent();
            this.userId = userId;
            this.userRole = "admin";
            this.taskInfo = taskInfo;
            InitializeForm();
        }

        /// <summary>
        /// Initializes form components and sets up event handlers
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Заполняем информацию о задании
                taskTitleLabel.Text = taskInfo.TaskTitle;
                submissionInfoLabel.Text = $"Отправитель: {taskInfo.SubmitterName}";
                filePathTextBox.Text = taskInfo.FilePath;
                filePathTextBox.ReadOnly = true;

                // Настраиваем кнопки
                approveButton.Click += ApproveButton_Click;
                rejectButton.Click += RejectButton_Click;

                // Настраиваем поле комментариев
                commentsTextBox.Text = "Введите комментарий к работе...";
                commentsTextBox.ForeColor = Color.Gray;
                commentsTextBox.Enter += CommentsTextBox_Enter;
                commentsTextBox.Leave += CommentsTextBox_Leave;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации формы: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the Enter event for comments textbox
        /// </summary>
        private void CommentsTextBox_Enter(object sender, EventArgs e)
        {
            if (commentsTextBox.Text == "Введите комментарий к работе...")
            {
                commentsTextBox.Text = "";
                commentsTextBox.ForeColor = SystemColors.WindowText;
            }
        }

        /// <summary>
        /// Handles the Leave event for comments textbox
        /// </summary>
        private void CommentsTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(commentsTextBox.Text))
            {
                commentsTextBox.Text = "Введите комментарий к работе...";
                commentsTextBox.ForeColor = Color.Gray;
            }
        }

        /// <summary>
        /// Handles the approve button click event
        /// </summary>
        private void ApproveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(commentsTextBox.Text) || 
                    commentsTextBox.Text == "Введите комментарий к работе...")
                {
                    MessageBox.Show("Пожалуйста, добавьте комментарий к решению",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dbManager = DatabaseManager.Instance;
                if (dbManager.UpdateSubmissionStatus(taskInfo.SubmissionId, "Approved", commentsTextBox.Text, userId))
                {
                    MessageBox.Show("Задание одобрено", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось обновить статус задания", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при одобрении задания: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the reject button click event
        /// </summary>
        private void RejectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(commentsTextBox.Text) || 
                    commentsTextBox.Text == "Введите комментарий к работе...")
                {
                    MessageBox.Show("Пожалуйста, укажите причину отклонения",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dbManager = DatabaseManager.Instance;
                if (dbManager.UpdateSubmissionStatus(taskInfo.SubmissionId, "Rejected", commentsTextBox.Text, userId))
                {
                    MessageBox.Show("Задание отклонено", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось обновить статус задания", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отклонении задания: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Ошибка при возврате к форме проверки: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 