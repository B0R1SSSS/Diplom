using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using static ObservationTreeView;
using System.Drawing;
using System.Threading.Tasks;
using DEP.Forms;
using DEP.Database;
using DEP.Models;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.IO.Compression;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace DEP
{
    public partial class PrepareModForm : Form, OtherMethods.ILoadableForm
    {
        private FileSystemWatcher _folderWatcher;//наблюдатель
        private ArchiveActions archiveActions = new ArchiveActions();
        private ObservationTreeView examTreeView;
        private ObservationRichTextBox enhancedRichTextBox;
        private readonly int userId;
        private readonly string userRole;
        private readonly string CurrentPath = OtherMethods.GetDefaultPath();
        private TaskInfo currentTask;
        private ReviewTaskForm reviewTaskForm;
        private bool isReviewTaskFormVisible = false;
        private readonly DatabaseManager dbManager;
        private readonly string tempDirectory;
        private TaskInfo selectedTask;
        private bool isFileSystemView;

        public PrepareModForm(string userRole,int userid)
        {
            this.MaximizeBox = false;
            this.userRole = userRole;
            this.userId = userid;
            InitializeComponent();
            dbManager = DatabaseManager.Instance;
            
            // Test database connection
            Console.WriteLine($"Инициализация PrepareModForm: UserId={userId}, UserRole={userRole}");
            dbManager.TestConnection();
            
            tempDirectory = Path.Combine(Path.GetTempPath(), "DEP_Temp");
            Directory.CreateDirectory(tempDirectory);
            isFileSystemView = false;

            // Загружаем список заданий
            LoadTasks();

            // Управляем доступностью кнопки ChangeModeButton в зависимости от роли
            ChangeModeButton.Enabled = userRole.ToLower() != "student";
            if (!ChangeModeButton.Enabled)
            {
                ChangeModeButton.BackColor = Color.LightGray;
                ChangeModeButton.ForeColor = Color.DarkGray;
                ChangeModeButton.FlatStyle = FlatStyle.Standard;
                ChangeModeButton.Cursor = Cursors.No;
            }

            // Привязываем обработчики событий
            this.FormClosing += PrepareModForm_FormClosing;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            
            // Инициализация TreeView
            examTreeView = new ObservationTreeView();
            examTreeView.Dock = DockStyle.Fill;
            TreeViewPanel.Controls.Add(examTreeView);

            // Инициализация RichTextBox
            enhancedRichTextBox = new ObservationRichTextBox();
            enhancedRichTextBox.Dock = DockStyle.Fill;
            FileViewerPanel.Controls.Add(enhancedRichTextBox);
            
                // Подписка на события
                examTreeView.AfterSelect += ExamPapersTreeView_AfterSelect;
            InitializeFolderWatcher();
        }

        private void InitializeFolderWatcher()
        {
            _folderWatcher = new FileSystemWatcher
            {
                Path = CurrentPath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            _folderWatcher.Changed += (s, e) => RefreshTreeViewSafe();
            _folderWatcher.Created += (s, e) => RefreshTreeViewSafe();
            _folderWatcher.Deleted += (s, e) => RefreshTreeViewSafe();
            _folderWatcher.Renamed += (s, e) => RefreshTreeViewSafe();
        }

        private void RefreshTreeViewSafe()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshTreeView));
                return;
            }
            RefreshTreeView();
        }

        private void ExamPapersTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {


            if (e.Node?.Tag is FileInfo fileInfo)
            {
                enhancedRichTextBox.DisplayFile(fileInfo.FullName);
            }
            else
            {
                enhancedRichTextBox.Clear();
            }
        }

        public void LoadContent(string path)
        {
            examTreeView.LoadDirectory(path);

        }

        public void RefreshTreeView()
        {
            examTreeView.RefreshTreeView();
        }

       

        private void OpenFolderButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(CurrentPath))
                {
                    // Вариант 1: Стандартное открытие
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = CurrentPath,
                        UseShellExecute = true
                    });

                    // Вариант 2: С конкретным файловым менеджером
                    // Process.Start("explorer.exe", appRootFolder);
                }
                else
                {
                    MessageBox.Show($"Корневая папка не найдена:\n{CurrentPath}",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть папку:\n{ex.Message}",
                              "Ошибка",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }


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
                                using (var archive = ZipArchive.Create())
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
                                using (var archive = ZipArchive.Open(openFileDialog.FileName))
                                {
                                    foreach (var entry in archive.Entries)
                                    {
                                        if (!entry.IsDirectory)
                                        {
                                            entry.WriteToDirectory(extractPath, new ExtractionOptions()
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

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentPath))
            {
                MessageBox.Show("Пожалуйста, выберите файл для отправки", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SelectTaskComboBox.SelectedItem == null || selectedTask == null)
            {
                MessageBox.Show("Пожалуйста, выберите задание из списка", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedTaskData = (TaskAdd)SelectTaskComboBox.SelectedItem;
                
                // Открываем форму отправки задания
                using (var submitForm = new SubmitTaskForm(userId, selectedTaskData.TaskId, selectedTaskData.Title))
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
        /// Handles ChangeModeButton click event
        /// Обрабатывает событие нажатия кнопки ChangeModeButton
        /// </summary>
        private void ChangeModeButton_Click(object sender, EventArgs e)
        {
            var reviewTaskForm = Application.OpenForms.OfType<ReviewTaskForm>().FirstOrDefault();
            if (reviewTaskForm != null)
            {
                reviewTaskForm.Show();
                this.Hide();
            }
        }

        /// <summary>
        /// Handles DownloadMaterialsButton click event for downloading and extracting study materials
        /// </summary>
        private void DownloadMaterialsButton_Click(object sender, EventArgs e)
        {
            try
            {
                var dbManager = DatabaseManager.Instance;
                var materials = dbManager.GetAllStudyMaterials();

                if (materials.Count == 0)
                {
                    MessageBox.Show("Методические материалы не найдены",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int successCount = 0;
                foreach (var material in materials)
                {
                    string filePath = dbManager.DownloadStudyMaterial(material.MaterialId, OtherMethods.ExamsPath);
                    if (filePath != null)
                    {
                        try
                        {
                            // Проверяем, является ли файл архивом
                            if (Path.GetExtension(filePath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                            {
                                using (var archive = ArchiveFactory.Open(filePath))
                                {
                                    // Распаковываем архив в папку ExamPapers
                                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                                    {
                                        entry.WriteToDirectory(OtherMethods.ExamsPath, new ExtractionOptions()
                                        {
                                            ExtractFullPath = true,
                                            Overwrite = true
                                        });
                                    }
                                }
                                // Удаляем архив после распаковки
                                File.Delete(filePath);
                            }
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при распаковке архива {Path.GetFileName(filePath)}: {ex.Message}",
                                "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                MessageBox.Show($"Скачано и распаковано материалов: {successCount} из {materials.Count}",
                    "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при скачивании материалов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the list of tasks into the ComboBox
        /// </summary>
        private void LoadTasks()
        {
            try
            {
                var tasks = dbManager.GetAllTasks();
                
                // Create a list with placeholder item
                var taskList = new List<TaskAdd>();
                taskList.Add(new TaskAdd { TaskId = -1, Title = "Выберите задание" });
                taskList.AddRange(tasks);
                
                SelectTaskComboBox.DisplayMember = "Title";
                SelectTaskComboBox.ValueMember = "TaskId";
                SelectTaskComboBox.DataSource = taskList;
                
                // Set initial selection to placeholder
                SelectTaskComboBox.SelectedIndex = 0;
                
                // Add event handler for selection change
                SelectTaskComboBox.SelectedIndexChanged += SelectTaskComboBox_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка заданий: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles SelectTaskComboBox selection change event
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void SelectTaskComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (SelectTaskComboBox.SelectedItem != null)
                {
                    var selectedTaskData = (TaskAdd)SelectTaskComboBox.SelectedItem;
                    
                    // Check if placeholder is selected
                    if (selectedTaskData.TaskId == -1)
                    {
                        selectedTask = null;
                        Console.WriteLine("Выбран placeholder - задание не выбрано");
                        return;
                    }
                    
                    selectedTask = new TaskInfo
                    {
                        TaskId = selectedTaskData.TaskId,
                        TaskTitle = selectedTaskData.Title
                    };
                    
                    Console.WriteLine($"Выбрано задание: TaskId={selectedTask.TaskId}, TaskTitle={selectedTask.TaskTitle}");
                }
                else
                {
                    selectedTask = null;
                    Console.WriteLine("Элемент не выбран");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении выбранного задания: {ex.Message}");
            }
        }

        private void LoadFolderExamPapersButton_Click(object sender, EventArgs e)
        {
            LoadContent(OtherMethods.ExamsPath);
        }

        private void LoadFolderExamTasksButton_Click(object sender, EventArgs e)
        {
            LoadContent(OtherMethods.TestsPath);
        }

        /// <summary>
        /// Handles ViewStatusButton click event for viewing submission status
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void ViewStatusButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedTask != null)
                {
                    Console.WriteLine($"Открываем форму статуса: UserId={userId}, TaskId={selectedTask.TaskId}, TaskTitle={selectedTask.TaskTitle}");
                    var statusForm = new SubmissionStatusForm(userId, selectedTask.TaskId, selectedTask.TaskTitle);
                    statusForm.Show();
                }
                else
                {
                    Console.WriteLine("Задание не выбрано");
                    MessageBox.Show("Пожалуйста, выберите задание из списка", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при открытии формы статуса: {ex.Message}");
                MessageBox.Show($"Ошибка при открытии формы статуса: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles BackToStartButton click event
        /// Returns user to the StartingForm
        /// Обрабатывает событие нажатия кнопки BackToStartButton
        /// Возвращает пользователя на форму входа
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void BackToStartButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Find existing StartingForm or create new one
                var startingForm = Application.OpenForms.OfType<StartingForm>().FirstOrDefault();
                if (startingForm == null)
                {
                    startingForm = new StartingForm();
                    startingForm.Show();
                }
                else
                {
                    startingForm.Show();
                    startingForm.BringToFront();
                }
                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при возврате к форме входа: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepareModForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Find existing StartingForm or create new one
                var startingForm = Application.OpenForms.OfType<StartingForm>().FirstOrDefault();
                if (startingForm == null)
                {
                    startingForm = new StartingForm();
                    startingForm.Show();
                }
                else
                {
                    startingForm.Show();
                    startingForm.BringToFront();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при возврате к форме входа: {ex.Message}");
            }
        }
    }
}