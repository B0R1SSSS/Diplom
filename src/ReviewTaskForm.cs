using System;
using System.Windows.Forms;

public partial class ReviewTaskForm : Form
{
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
} 