using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using DEP.Models;
using DEP.Database;
using DEP.Controls;
using SharpCompress.Archives.Zip;

namespace DEP
{
    public partial class PrepareModForm : Form
    {
        private readonly DatabaseManager dbManager;
        private readonly string tempDirectory;
        private TaskInfo selectedTask;
        private bool isFileSystemView;

        /// <summary>
        /// Initializes a new instance of the PrepareModForm
        /// </summary>
        public PrepareModForm()
        {
            InitializeComponent();
            dbManager = DatabaseManager.Instance;
            tempDirectory = Path.Combine(Path.GetTempPath(), "DEP_Temp");
            Directory.CreateDirectory(tempDirectory);
            isFileSystemView = false;

            // Привязываем обработчики событий
            SelectButton.Click += SelectButton_Click;
            ReviewButton.Click += ReviewButton_Click;
            CancelButton.Click += CancelButton_Click;
            loadTaskButton.Click += LoadTaskButton_Click;
            ChangeModeButton.Click += ChangeModeButton_Click;
            FileDataGridView.SelectionChanged += FileDataGridView_SelectionChanged;
            SelectTextBox.TextChanged += SelectTextBox_TextChanged;
            UnpackArchiveButton.Click += UnpackArchiveButton_Click;
            PackFolderButton.Click += PackFolderButton_Click;
            LoadFileButton.Click += LoadFileButton_Click;

            // Загружаем данные при открытии формы
            LoadSubmissions();
        }

        /// <summary>
        /// Loads submissions from database into the grid
        /// </summary>
        private void LoadSubmissions()
        {
            try
            {
                FileDataGridView.Rows.Clear();
                var submissions = dbManager.GetAllSubmissions();
                foreach (var submission in submissions)
                {
                    FileDataGridView.Rows.Add(
                        submission.TaskId,
                        submission.TaskTitle,
                        submission.SubmitterName,
                        submission.FilePath,
                        submission.SubmissionDate,
                        submission.Status
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles SelectButton click event
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
                var targetPath = Path.Combine(tempDirectory, selectedTask.FilePath);
                if (dbManager.DownloadSubmission(selectedTask.TaskId, tempDirectory))
                {
                    MessageBox.Show($"Файл успешно загружен: {targetPath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// </summary>
        private void ReviewButton_Click(object sender, EventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Пожалуйста, выберите работу для проверки", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Открыть форму для проверки работы
            MessageBox.Show("Функционал проверки работы будет реализован позже", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles CancelButton click event
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles LoadTaskButton click event
        /// </summary>
        private void LoadTaskButton_Click(object sender, EventArgs e)
        {
            LoadSubmissions();
        }

        /// <summary>
        /// Handles ChangeModeButton click event
        /// </summary>
        private void ChangeModeButton_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
            FileDataGridView.Visible = !FileDataGridView.Visible;
        }

        /// <summary>
        /// Handles FileDataGridView selection change event
        /// </summary>
        private void FileDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (FileDataGridView.SelectedRows.Count > 0)
            {
                var row = FileDataGridView.SelectedRows[0];
                selectedTask = new TaskInfo
                {
                    TaskId = Convert.ToInt32(row.Cells["SubmissionId"].Value),
                    TaskTitle = row.Cells["TaskTitle"].Value?.ToString(),
                    SubmitterName = row.Cells["StudentName"].Value?.ToString(),
                    FilePath = row.Cells["FileName"].Value?.ToString(),
                    SubmissionDate = Convert.ToDateTime(row.Cells["SubmittedAt"].Value),
                    Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), row.Cells["Status"].Value?.ToString() ?? "Pending")
                };
            }
            else
            {
                selectedTask = null;
            }
        }

        /// <summary>
        /// Handles SelectTextBox text change event
        /// </summary>
        private void SelectTextBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(SelectTextBox.Text, out int taskId))
            {
                foreach (DataGridViewRow row in FileDataGridView.Rows)
                {
                    if (Convert.ToInt32(row.Cells["SubmissionId"].Value) == taskId)
                    {
                        row.Selected = true;
                        FileDataGridView.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles UnpackArchiveButton click event
        /// </summary>
        private void UnpackArchiveButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*";
                    openFileDialog.Title = "Выберите архив для распаковки";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Выбираем папку для распаковки
                        using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                        {
                            folderDialog.Description = "Выберите папку для распаковки архива";
                            folderDialog.ShowNewFolderButton = true;

                            if (folderDialog.ShowDialog() == DialogResult.OK)
                            {
                                string extractPath = folderDialog.SelectedPath;

                                // Распаковываем архив
                                using (var archive = SharpCompress.Archives.Zip.ZipArchive.Open(openFileDialog.FileName))
                                {
                                    foreach (var entry in archive.Entries)
                                    {
                                        if (!entry.IsDirectory)
                                        {
                                            entry.WriteToDirectory(extractPath, new SharpCompress.Common.ExtractionOptions()
                                            {
                                                ExtractFullPath = true,
                                                Overwrite = true
                                            });
                                        }
                                    }
                                }

                                MessageBox.Show("Архив успешно распакован", 
                                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при распаковке архива: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles PackFolderButton click event
        /// </summary>
        private void PackFolderButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Выбираем папку для архивации
                using (FolderBrowserDialog sourceDialog = new FolderBrowserDialog())
                {
                    sourceDialog.Description = "Выберите папку для архивации";
                    sourceDialog.ShowNewFolderButton = false;

                    if (sourceDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Выбираем папку для сохранения архива
                        using (FolderBrowserDialog saveDialog = new FolderBrowserDialog())
                        {
                            saveDialog.Description = "Выберите папку для сохранения архива";
                            saveDialog.ShowNewFolderButton = true;

                            if (saveDialog.ShowDialog() == DialogResult.OK)
                            {
                                string sourcePath = sourceDialog.SelectedPath;
                                string savePath = saveDialog.SelectedPath;
                                string archiveName = $"Archive_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
                                string archivePath = Path.Combine(savePath, archiveName);

                                // Создаем архив
                                using (var archive = SharpCompress.Archives.Zip.ZipArchive.Create())
                                {
                                    foreach (string file in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                                    {
                                        string relativePath = file.Substring(sourcePath.Length + 1);
                                        archive.AddEntry(relativePath, file);
                                    }

                                    using (var stream = File.Create(archivePath))
                                    {
                                        archive.SaveTo(stream);
                                    }
                                }

                                MessageBox.Show("Архив успешно создан", 
                                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании архива: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles LoadFileButton click event
        /// </summary>
        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentPathTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, выберите файл для отправки", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (combobox1.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите задание из списка", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedTask = (TaskAdd)combobox1.SelectedItem;
                
                // Открываем форму отправки задания
                using (var submitForm = new SubmitTaskForm(userId, selectedTask.TaskId, selectedTask.Title))
                {
                    submitForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы отправки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles form closing event
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            try
            {
                if (Directory.Exists(tempDirectory))
                {
                    Directory.Delete(tempDirectory, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении временной директории: {ex.Message}");
            }
        }
    }
} 