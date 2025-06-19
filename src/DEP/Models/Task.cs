using System;

namespace DEP.Models
{
    /// <summary>
    /// Represents a task in the system
    /// </summary>
    public class TaskAdd
    {
        /// <summary>
        /// Gets or sets the task ID
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Gets or sets the task title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the task description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the task
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// Gets or sets the task creation date
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets whether the task is active
        /// </summary>
        public bool IsActive { get; set; }
    }
} 