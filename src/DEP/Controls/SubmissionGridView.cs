using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using DEP.Models;

namespace DEP.Controls
{
    /// <summary>
    /// Custom DataGridView for displaying submission information
    /// </summary>
    public class SubmissionGridView : DataGridView
    {
        private TaskInfo selectedSubmission;
        private bool isFileSystemView;

        /// <summary>
        /// Gets or sets whether the grid is in file system view mode
        /// </summary>
        public bool IsFileSystemView
        {
            get => isFileSystemView;
            set
            {
                isFileSystemView = value;
                UpdateColumns();
            }
        }

        /// <summary>
        /// Gets the currently selected submission
        /// </summary>
        public TaskInfo SelectedSubmission => selectedSubmission;

        /// <summary>
        /// Event raised when selection changes
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the SubmissionGridView
        /// </summary>
        public SubmissionGridView()
        {
            InitializeGrid();
        }

        /// <summary>
        /// Initializes the grid with default settings
        /// </summary>
        private void InitializeGrid()
        {
            // Настройка внешнего вида
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            MultiSelect = false;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToOrderColumns = true;
            AllowUserToResizeRows = false;
            ReadOnly = true;
            BackgroundColor = Color.White;
            BorderStyle = BorderStyle.None;
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            GridColor = Color.LightGray;
            RowHeadersVisible = false;

            // Настройка стилей
            DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            DefaultCellStyle.SelectionForeColor = Color.White;
            DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            DefaultCellStyle.Padding = new Padding(5);

            // Настройка заголовков
            ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64);
            ColumnHeadersHeight = 40;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            EnableHeadersVisualStyles = false;

            // Настройка строк
            RowTemplate.Height = 35;
            AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Добавляем колонки
            UpdateColumns();

            // Добавляем обработчик выбора строки
            SelectionChanged += (s, e) => OnSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Updates grid columns based on view mode
        /// </summary>
        private void UpdateColumns()
        {
            Columns.Clear();

            if (isFileSystemView)
            {
                // Колонки для файловой системы
                Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FileName",
                    HeaderText = "Имя файла",
                    DataPropertyName = "FileName"
                });

                Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "LastModified",
                    HeaderText = "Дата изменения",
                    DataPropertyName = "LastModified"
                });

                Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Size",
                    HeaderText = "Размер",
                    DataPropertyName = "Size"
                });
            }
            else
            {
                // Колонки для БД (соответствуют get_submissions_for_review)
                Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "SubmissionId",
                    HeaderText = "ID",
                    DataPropertyName = "TaskId",
                    Width = 50
                });

                Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TaskTitle",
                    HeaderText = "Название задачи",
                    DataPropertyName = "TaskTitle"
                });

                Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "StudentName",
                    HeaderText = "Студент",
                    DataPropertyName = "SubmitterName"
                });

                Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FileName",
                    HeaderText = "Файл",
                    DataPropertyName = "FilePath"
                });

                Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "SubmittedAt",
                    HeaderText = "Дата отправки",
                    DataPropertyName = "SubmissionDate",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy HH:mm" }
                });

                Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Status",
                    HeaderText = "Статус",
                    DataPropertyName = "Status"
                });
            }
        }

        /// <summary>
        /// Displays a task in the grid
        /// </summary>
        /// <param name="task">Task to display</param>
        public void DisplayTask(TaskInfo task)
        {
            if (task == null) return;

            if (isFileSystemView)
            {
                var fileInfo = new FileInfo(task.FilePath);
                if (fileInfo.Exists)
                {
                    Rows.Add(
                        fileInfo.Name,
                        fileInfo.LastWriteTime,
                        FormatFileSize(fileInfo.Length)
                    );
                }
            }
            else
            {
                Rows.Add(
                    task.TaskId,
                    task.TaskTitle,
                    task.SubmitterName,
                    task.FilePath,
                    task.SubmissionDate,
                    task.Status
                );
            }
        }

        /// <summary>
        /// Formats file size in human-readable format
        /// </summary>
        /// <param name="bytes">Size in bytes</param>
        /// <returns>Formatted size string</returns>
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Handles selection change
        /// </summary>
        protected override void OnSelectionChanged(EventArgs e)
        {
            base.OnSelectionChanged(e);
            if (SelectedRows.Count > 0)
            {
                var row = SelectedRows[0];
                selectedSubmission = new TaskInfo
                {
                    TaskId = Convert.ToInt32(row.Cells["StudentId"].Value),
                    TaskTitle = row.Cells["TaskTitle"].Value?.ToString(),
                    SubmitterName = row.Cells["StudentName"].Value?.ToString(),
                    FilePath = row.Cells["FileName"].Value?.ToString(),
                    SubmissionDate = Convert.ToDateTime(row.Cells["SubmittedAt"].Value),
                    Status = (SubmissionStatus)Enum.Parse(typeof(SubmissionStatus), row.Cells["Status"].Value?.ToString() ?? "Pending")
                };
            }
            else
            {
                selectedSubmission = null;
            }
            SelectionChanged?.Invoke(this, e);
        }
    }
} 