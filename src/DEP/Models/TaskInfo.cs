using System;

namespace DEP.Models
{
    /// <summary>
    /// Represents task information
    /// Представляет информацию о задании
    /// </summary>
    public class TaskInfo
    {
        /// <summary>
        /// Gets or sets the submission ID
        /// Получает или устанавливает ID отправки
        /// </summary>
        public int SubmissionId { get; set; }

        /// <summary>
        /// Gets or sets the task ID
        /// Получает или устанавливает ID задания
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Gets or sets the student ID
        /// Получает или устанавливает ID студента
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Gets or sets the task title
        /// Получает или устанавливает название задания
        /// </summary>
        public string TaskTitle { get; set; }

        /// <summary>
        /// Gets or sets the submitter name
        /// Получает или устанавливает имя отправителя
        /// </summary>
        public string SubmitterName { get; set; }

        /// <summary>
        /// Gets or sets the submission date
        /// Получает или устанавливает дату отправки
        /// </summary>
        public DateTime SubmissionDate { get; set; }

        /// <summary>
        /// Gets or sets the submission status
        /// Получает или устанавливает статус отправки
        /// </summary>
        public SubmissionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the file path
        /// Получает или устанавливает путь к файлу
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the comments
        /// Получает или устанавливает комментарии
        /// </summary>
        public string Comments { get; set; }
    }

    /// <summary>
    /// Represents submission status
    /// Представляет статус отправки
    /// </summary>
    public enum SubmissionStatus
    {
        /// <summary>
        /// Pending status
        /// Статус ожидания
        /// </summary>
        Pending,

        /// <summary>
        /// Approved status
        /// Статус одобрения
        /// </summary>
        Approved,

        /// <summary>
        /// Rejected status
        /// Статус отклонения
        /// </summary>
        Rejected
    }
}