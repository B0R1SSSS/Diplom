using System;

namespace DEP.Models
{
    /// <summary>
    /// Represents study material information
    /// </summary>
    public class StudyMaterialInfo
    {
        /// <summary>
        /// Gets or sets the material ID
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// Gets or sets the task ID
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Gets or sets the task title
        /// </summary>
        public string TaskTitle { get; set; }

        /// <summary>
        /// Gets or sets the material title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the material description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file type
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who uploaded the material
        /// </summary>
        public string UploadedByName { get; set; }

        /// <summary>
        /// Gets or sets the upload date and time
        /// </summary>
        public DateTime UploadedAt { get; set; }
    }
} 