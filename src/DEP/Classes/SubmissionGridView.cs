using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Spire.Doc.Documents;
using System.Data;
using DEP.Database;

namespace DEP
{
    /// <summary>
    /// Custom DataGridView control for displaying submission information
    /// </summary>
    public class SubmissionGridView : DataGridView
    {
        DatabaseManager databaseManager;
        private bool isFileSystemView = false;
        private string baseDirectory;
        private readonly string[] allowedExtensions = { ".doc", ".docx", ".pdf", ".txt", ".zip", ".rar" };

        /// <summary>
        /// Gets or sets whether the grid is in file system view mode
        /// </summary>
        public bool IsFileSystemView
        {
            get => isFileSystemView;
            set
            {
                if (isFileSystemView != value)
                {
                    isFileSystemView = value;
                    RefreshView();
                }
            }
        }

        /// <summary>
        /// Gets the currently selected submission
        /// </summary>
        public TaskInfo SelectedSubmission => 
            SelectedRows.Count > 0 ? SelectedRows[0].DataBoundItem as TaskInfo : null;

        /// <summary>
        /// Initializes a new instance of the SubmissionGridView
        /// </summary>
        public SubmissionGridView()
        {
            InitializeGrid();
        }

        /// <summary>
        /// Sets the base directory for file system view
        /// </summary>
        /// <param name="directory">Base directory path</param>
        public void SetBaseDirectory(string directory)
        {
            baseDirectory = directory;
            if (isFileSystemView)
            {
                RefreshView();
            }
        }

        /// <summary>
        /// Initializes the grid with default settings
        /// </summary>
        private void InitializeGrid()
        {
            this.Dock = DockStyle.Fill;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToOrderColumns = true;
            this.ReadOnly = true;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.MultiSelect = false;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.BackgroundColor = SystemColors.Window;
            
            this.RowHeadersVisible = false;
            this.EnableHeadersVisualStyles = false;
            this.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            this.ColumnHeadersDefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);
            this.RowTemplate.Height = 30;
            this.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
        }

        /// <summary>
        /// Refreshes the current view based on the selected mode
        /// </summary>
        public void RefreshView()
        {
            this.DataSource = null;
            if (isFileSystemView)
            {
                LoadFileSystemView();
            }
            else
            {
                LoadDatabaseView();
            }
        }

        /// <summary>
        /// Loads the file system view starting from the base directory
        /// </summary>
        private void LoadFileSystemView()
        {
            if (string.IsNullOrEmpty(baseDirectory) || !Directory.Exists(baseDirectory))
            {
                Console.WriteLine("Ошибка: Базовая директория не существует");
                return;
            }

            try
            {
                var files = Directory.GetFiles(baseDirectory, "*.*", SearchOption.AllDirectories)
                    .Where(f => allowedExtensions.Contains(Path.GetExtension(f).ToLower()))
                    .Select(f => new TaskInfo
                    {
                        TaskTitle = Path.GetFileName(f),
                        FilePath = f,
                        SubmitterName = Path.GetDirectoryName(f).Split(Path.DirectorySeparatorChar).Last(),
                        SubmissionDate = File.GetLastWriteTime(f)
                    })
                    .ToList();

                this.DataSource = files;
                ConfigureColumns();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке файловой системы: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the database view with submissions
        /// </summary>
        private void LoadDatabaseView()
        {
            try
            {
                var submissions = DatabaseManager.Instance.GetSubmissionsForReview();
                this.DataSource = submissions;
                ConfigureColumns();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных из БД: {ex.Message}");
            }
        }

        /// <summary>
        /// Configures grid columns based on the current view mode
        /// </summary>
        private void ConfigureColumns()
        {
            if (this.Columns.Count == 0) return;

            // Настраиваем колонки
            if (this.Columns.Contains("TaskId"))
            {
                this.Columns["TaskId"].Visible = false;
            }

            if (this.Columns.Contains("TaskTitle"))
            {
                this.Columns["TaskTitle"].HeaderText = "Название задания";
                this.Columns["TaskTitle"].Width = 200;
            }

            if (this.Columns.Contains("SubmitterName"))
            {
                this.Columns["SubmitterName"].HeaderText = "Отправитель";
                this.Columns["SubmitterName"].Width = 150;
            }

            if (this.Columns.Contains("SubmissionDate"))
            {
                this.Columns["SubmissionDate"].HeaderText = "Дата отправки";
                this.Columns["SubmissionDate"].Width = 150;
                this.Columns["SubmissionDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            }

            if (this.Columns.Contains("FilePath"))
            {
                this.Columns["FilePath"].HeaderText = "Путь к файлу";
                this.Columns["FilePath"].Width = 300;
            }

            // Добавляем контекстное меню
            var contextMenu = new ContextMenuStrip();
            var openItem = new ToolStripMenuItem("Открыть");
            openItem.Click += (s, e) => OnOpenFileRequested();
            contextMenu.Items.Add(openItem);

            var downloadItem = new ToolStripMenuItem("Скачать");
            downloadItem.Click += (s, e) => OnDownloadFileRequested();
            contextMenu.Items.Add(downloadItem);

            this.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// Event handler for open file request
        /// </summary>
        private void OnOpenFileRequested()
        {
            if (SelectedSubmission != null)
            {
                try
                {
                    System.Diagnostics.Process.Start(SelectedSubmission.FilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при открытии файла: {ex.Message}");
                    MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", 
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Event handler for download file request
        /// </summary>
        private void OnDownloadFileRequested()
        {
            if (SelectedSubmission != null)
            {
                try
                {
                    using (var dialog = new SaveFileDialog())
                    {
                        dialog.FileName = Path.GetFileName(SelectedSubmission.FilePath);
                        dialog.Filter = "Все файлы|*.*";

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            File.Copy(SelectedSubmission.FilePath, dialog.FileName, true);
                            MessageBox.Show("Файл успешно загружен", "Успех", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
                    MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", 
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
} 