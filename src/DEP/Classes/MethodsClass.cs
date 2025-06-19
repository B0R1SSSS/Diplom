using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc;
using DEP.Properties;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Texts;
using Spire.Xls;
using System.Data;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Writers;
using System.Diagnostics;

/// <summary>
/// Provides utility methods for file and folder operations
/// </summary>
public class OtherMethods
{
    private const string APP_NAME = "DemonstrationExamPreparationApp";
    private const string LOG_FILE = "app.log";
    private const string README_CONTENT = "Добро пожаловать в Demonstration Exam Preparation App!\n\n" +
                                        "Папка ExamPapers - для хранения экзаменационных заданий и учебных материалов\n" +
                                        "Папка ExamTasks - для хранения выполненных заданий\n" +
                                        "Папка ExamReview - содержит файлы для проверки\n\n";

    // Пути к файлам и папкам
    private static readonly string SettingsFileName = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        APP_NAME,
        "Settings.txt");

    private static readonly string AppFolderPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        APP_NAME);

    private static readonly string LogFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        APP_NAME,
        LOG_FILE);

    public static readonly string ExamsPath = Path.Combine(AppFolderPath, "ExamPapers");
    public static readonly string TestsPath = Path.Combine(AppFolderPath, "ExamTasks");
    public static readonly string ReviewPath = Path.Combine(AppFolderPath, "ExamReview");
    public static readonly string IconsPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "DepProgramFiles");

    /// <summary>
    /// Initializes the application folder structure and logging
    /// </summary>
    static OtherMethods()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFileName));
            Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
            CreateAppFolderStructure();
            LogInfo("Application initialized successfully");
        }
        catch (Exception ex)
        {
            LogError("Initialization error", ex);
            MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Logs an information message
    /// </summary>
    /// <param name="message">The message to log</param>
    private static void LogInfo(string message)
    {
        try
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [INFO] {message}";
            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to write to log: {ex.Message}");
        }
    }

    /// <summary>
    /// Logs an error message with exception details
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="ex">The exception that occurred</param>
    private static void LogError(string message, Exception ex)
    {
        try
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [ERROR] {message}\nException: {ex}\nStackTrace: {ex.StackTrace}";
            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }
        catch (Exception logEx)
        {
            Debug.WriteLine($"Failed to write error to log: {logEx.Message}");
        }
    }

    /// <summary>
    /// Opens a folder selection dialog and processes the selected path
    /// </summary>
    /// <param name="form">The form that will handle the selected path</param>
    /// <param name="description">The description to show in the folder dialog</param>
    /// <returns>The selected folder path or default path if cancelled</returns>
    public static string SelectFolder(Form form, string description = "Выберите папку с учебным материалом")
    {
        try
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                ConfigureFolderDialog(folderDialog, description);

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    ProcessSelectedPath(form, folderDialog.SelectedPath);
                    LogInfo($"Selected folder: {folderDialog.SelectedPath}");
                    return folderDialog.SelectedPath;
                }
                LogInfo("Folder selection cancelled");
                return ExamsPath;
            }
        }
        catch (Exception ex)
        {
            LogError("Error in SelectFolder", ex);
            MessageBox.Show($"Ошибка выбора папки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return ExamsPath;
        }
    }

    /// <summary>
    /// Creates the basic application folder structure
    /// </summary>
    private static void CreateAppFolderStructure()
    {
        try
        {
            Directory.CreateDirectory(AppFolderPath);
            Directory.CreateDirectory(ExamsPath);
            Directory.CreateDirectory(TestsPath);

            string readmePath = Path.Combine(AppFolderPath, "README.txt");
            if (!File.Exists(readmePath))
            {   
                File.WriteAllText(readmePath, README_CONTENT);
                LogInfo("Created README.txt file");
            }
        }
        catch (Exception ex)
        {
            LogError("Error creating folder structure", ex);
            throw;
        }
    }

    /// <summary>
    /// Configures the folder browser dialog with specified settings
    /// </summary>
    private static void ConfigureFolderDialog(FolderBrowserDialog dialog, string description)
    {
        dialog.Description = description;
        dialog.ShowNewFolderButton = false;
        dialog.RootFolder = Environment.SpecialFolder.MyComputer;
        dialog.SelectedPath = ExamsPath;
    }

    /// <summary>
    /// Processes the selected path and updates the form if it implements ILoadableForm
    /// </summary>
    private static void ProcessSelectedPath(Form form, string path)
    {
        try
        {
            SaveDefaultPath(path);

            if (form is ILoadableForm loadableForm)
            {
                loadableForm.LoadContent(path);
                LogInfo($"Loaded content from path: {path}");
            }
            else
            {
                LogInfo("Form does not support content loading");
                MessageBox.Show("Форма не поддерживает загрузку контента", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            LogError("Error processing selected path", ex);
            MessageBox.Show($"Ошибка обработки пути: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Saves the default path to settings file
    /// </summary>
    public static void SaveDefaultPath(string path)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                File.WriteAllText(SettingsFileName, path);
                LogInfo($"Saved default path: {path}");
            }
        }
        catch (Exception ex)
        {
            LogError("Error saving default path", ex);
            MessageBox.Show($"Ошибка сохранения пути: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Retrieves the default path from settings file
    /// </summary>
    /// <returns>The saved path or default path if not found</returns>
    public static string GetDefaultPath()
    {
        try
        {
            if (File.Exists(SettingsFileName))
            {
                string savedPath = File.ReadAllText(SettingsFileName).Trim();
                if (Directory.Exists(savedPath))
                {
                    LogInfo($"Retrieved default path: {savedPath}");
                    return savedPath;
                }
            }
            LogInfo("Using default ExamsPath");
            return ExamsPath;
        }
        catch (Exception ex)
        {
            LogError("Error reading settings", ex);
            MessageBox.Show($"Ошибка чтения настроек: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return ExamsPath;
        }
    }

    /// <summary>
    /// Interface for forms that can load content from a path
    /// </summary>
    public interface ILoadableForm
    {
        void LoadContent(string path);
    }
}

/// <summary>
/// Provides functionality for archive operations (extraction and creation)
/// </summary>
public class ArchiveActions
{
    private const string ARCHIVE_FILTER = "Архивные файлы (*.rar, *.7z, *.zip)|*.rar;*.7z;*.zip|Все файлы (*.*)|*.*";
    private const string ZIP_FILTER = "ZIP архив (*.zip)|*.zip|7Z архив (*.7z)|*.7z";
    private const int MAX_ARCHIVE_SIZE_MB = 1000; // 1GB limit

    /// <summary>
    /// Opens a file dialog to select an archive file
    /// </summary>
    /// <returns>The selected archive file path or empty string if cancelled</returns>
    public string SelectArchiveFile()
    {
        try
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = ARCHIVE_FILTER;
                openFileDialog.Title = "Выберите архив для распаковки";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (new FileInfo(openFileDialog.FileName).Length > MAX_ARCHIVE_SIZE_MB * 1024 * 1024)
                    {
                        MessageBox.Show($"Размер архива превышает {MAX_ARCHIVE_SIZE_MB}MB", "Предупреждение", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return string.Empty;
                    }
                    return openFileDialog.FileName;
                }
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка выбора архива: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return string.Empty;
        }
    }

    /// <summary>
    /// Opens a folder dialog to select destination for archive extraction
    /// </summary>
    /// <returns>The selected folder path or empty string if cancelled</returns>
    public string SelectDestinationFolder()
    {
        try
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите папку для распаковки архива";
                folderDialog.ShowNewFolderButton = true;

                return folderDialog.ShowDialog() == DialogResult.OK
                    ? folderDialog.SelectedPath
                    : string.Empty;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка выбора папки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return string.Empty;
        }
    }

    /// <summary>
    /// Extracts the contents of an archive to the specified destination
    /// </summary>
    /// <param name="archivePath">Path to the archive file</param>
    /// <param name="destinationPath">Path where to extract the contents</param>
    /// <param name="errorMessage">Output parameter for error message if extraction fails</param>
    /// <returns>True if extraction was successful, false otherwise</returns>
    public bool ExtractArchive(string archivePath, string destinationPath, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (!ValidatePathsForExtraction(archivePath, destinationPath, out errorMessage))
            return false;

        try
        {
            using (var archive = ArchiveFactory.Open(archivePath))
            {
                var options = new ExtractionOptions()
                {
                    ExtractFullPath = true,
                    Overwrite = true
                };

                foreach (var entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        try
                        {
                            entry.WriteToDirectory(destinationPath, options);
                        }
                        catch (Exception ex)
                        {
                            errorMessage = $"Ошибка при распаковке файла {entry.Key}: {ex.Message}";
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = $"Ошибка при распаковке архива: {ex.Message}";
            return false;
        }
    }

    /// <summary>
    /// Validates paths for archive extraction
    /// </summary>
    private bool ValidatePathsForExtraction(string archivePath, string destinationPath, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(archivePath))
        {
            errorMessage = "Не указан путь к архиву";
            return false;
        }

        if (string.IsNullOrWhiteSpace(destinationPath))
        {
            errorMessage = "Не указан путь для распаковки";
            return false;
        }

        if (!File.Exists(archivePath))
        {
            errorMessage = "Архив не найден";
            return false;
        }

        if (!Directory.Exists(destinationPath))
        {
            try
            {
                Directory.CreateDirectory(destinationPath);
            }
            catch (Exception ex)
            {
                errorMessage = $"Не удалось создать папку для распаковки: {ex.Message}";
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Creates an archive from a selected folder
    /// </summary>
    /// <param name="archivePath">Output parameter for the created archive path</param>
    /// <param name="errorMessage">Output parameter for error message if creation fails</param>
    /// <returns>True if archive creation was successful, false otherwise</returns>
    public bool CreateArchiveFromSelectedFolder(out string archivePath, out string errorMessage)
    {
        archivePath = string.Empty;
        errorMessage = string.Empty;

        string folderPath = SelectFolderToArchive();
        if (string.IsNullOrEmpty(folderPath))
        {
            return false;
        }

        string defaultFileName = Path.GetFileName(folderPath) + ".zip";
        archivePath = SelectArchiveSavePath(defaultFileName);
        if (string.IsNullOrEmpty(archivePath))
        {
            return false;
        }

        return CreateArchiveFromFolder(folderPath, archivePath, out errorMessage);
    }

    /// <summary>
    /// Opens a folder dialog to select a folder for archiving
    /// </summary>
    private string SelectFolderToArchive()
    {
        using (var folderDialog = new FolderBrowserDialog())
        {
            folderDialog.Description = "Выберите папку для архивации";
            folderDialog.ShowNewFolderButton = false;

            return folderDialog.ShowDialog() == DialogResult.OK
                ? folderDialog.SelectedPath
                : string.Empty;
        }
    }

    /// <summary>
    /// Opens a save dialog to select where to save the archive
    /// </summary>
    private string SelectArchiveSavePath(string defaultFileName)
    {
        using (var saveDialog = new SaveFileDialog())
        {
            saveDialog.Filter = ZIP_FILTER;
            saveDialog.FileName = defaultFileName;
            saveDialog.Title = "Сохранить архив как";

            return saveDialog.ShowDialog() == DialogResult.OK
                ? saveDialog.FileName
                : string.Empty;
        }
    }

    /// <summary>
    /// Creates an archive from the specified folder
    /// </summary>
    private bool CreateArchiveFromFolder(string folderPath, string archivePath, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (!ValidatePathsForArchiving(folderPath, archivePath, out errorMessage))
            return false;

        try
        {
            using (var archive = ArchiveFactory.Create(ArchiveType.Zip))
            {
                AddFolderToArchive(archive, folderPath, string.Empty);
                archive.SaveTo(archivePath, CompressionType.Deflate);
            }
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = $"Ошибка при создании архива: {ex.Message}";
            return false;
        }
    }

    /// <summary>
    /// Recursively adds folder contents to the archive
    /// </summary>
    private void AddFolderToArchive(IWritableArchive archive, string folderPath, string relativePath)
    {
        foreach (var file in Directory.GetFiles(folderPath))
        {
            string fileName = Path.GetFileName(file);
            string archivePath = string.IsNullOrEmpty(relativePath) ? fileName : Path.Combine(relativePath, fileName);
            archive.AddEntry(archivePath, file);
        }

        foreach (var directory in Directory.GetDirectories(folderPath))
        {
            string dirName = Path.GetFileName(directory);
            string newRelativePath = string.IsNullOrEmpty(relativePath) ? dirName : Path.Combine(relativePath, dirName);
            AddFolderToArchive(archive, directory, newRelativePath);
        }
    }

    /// <summary>
    /// Validates paths for archive creation
    /// </summary>
    private bool ValidatePathsForArchiving(string folderPath, string archivePath, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(folderPath))
        {
            errorMessage = "Не указана папка для архивации";
            return false;
        }

        if (string.IsNullOrWhiteSpace(archivePath))
        {
            errorMessage = "Не указан путь для сохранения архива";
            return false;
        }

        if (!Directory.Exists(folderPath))
        {
            errorMessage = "Папка для архивации не найдена";
            return false;
        }

        string archiveDirectory = Path.GetDirectoryName(archivePath);
        if (!Directory.Exists(archiveDirectory))
        {
            try
            {
                Directory.CreateDirectory(archiveDirectory);
            }
            catch (Exception ex)
            {
                errorMessage = $"Не удалось создать папку для архива: {ex.Message}";
                return false;
            }
        }

        return true;
    }
}

/// <summary>
/// Custom TreeView control for displaying file system structure
/// </summary>
public class ObservationTreeView : TreeView
{
    private const string LOADING_TEXT = "Loading...";
    private const string ACCESS_DENIED_TEXT = "Доступ запрещен";
    private const string ERROR_PREFIX = "Ошибка: ";
    private const string DIRECTORY_NOT_FOUND = "Директория не существует";
    private const string ICONS_LOAD_ERROR = "Ошибка загрузки иконок: {0}";
    private const string EXPAND_ICONS_LOAD_ERROR = "Ошибка загрузки иконок расширения: {0}";

    private readonly string _currentPath;
    private readonly ImageList _nodeImageList;
    private readonly ImageList _expandCollapseImageList;

    /// <summary>
    /// Initializes a new instance of the ObservationTreeView class
    /// </summary>
    public ObservationTreeView()
    {
        _currentPath = OtherMethods.ExamsPath;
        _nodeImageList = new ImageList
        {
            ImageSize = new Size(32, 32),
            ColorDepth = ColorDepth.Depth32Bit
        };
        _expandCollapseImageList = new ImageList
        {
            ImageSize = new Size(16, 16),
            ColorDepth = ColorDepth.Depth32Bit
        };

        InitializeTreeView();
        LoadDirectory(_currentPath);
    }

    /// <summary>
    /// Initializes the TreeView control with default settings
    /// </summary>
    private void InitializeTreeView()
    {
        ConfigureTreeViewAppearance();
        LoadNodeIcons();
        LoadExpandCollapseIcons();
        ConfigureEventHandlers();
    }

    /// <summary>
    /// Configures the basic appearance of the TreeView
    /// </summary>
    private void ConfigureTreeViewAppearance()
    {
        this.Font = new Font("Segoe UI", 13);
        this.ItemHeight = 40;
        this.Indent = 20;
        this.ShowPlusMinus = false;
        this.ShowLines = false;
        this.ShowRootLines = false;
        this.BackColor = Color.Gainsboro;
        this.Dock = DockStyle.Fill;
    }

    /// <summary>
    /// Loads icons for different file types
    /// </summary>
    private void LoadNodeIcons()
    {
        try
        {
            string iconsPath = OtherMethods.IconsPath;
            _nodeImageList.Images.Add("Folder", Image.FromFile(Path.Combine(iconsPath, "FolderIcon.png")));
            _nodeImageList.Images.Add("Unknown", Image.FromFile(Path.Combine(iconsPath, "FileType.png")));
            _nodeImageList.Images.Add("TXT", Image.FromFile(Path.Combine(iconsPath, "FileTypeTXT.png")));
            _nodeImageList.Images.Add("DOC", Image.FromFile(Path.Combine(iconsPath, "FileTypeDOC.png")));
            _nodeImageList.Images.Add("DOCX", Image.FromFile(Path.Combine(iconsPath, "FileTypeDOCX.png")));
            _nodeImageList.Images.Add("PDF", Image.FromFile(Path.Combine(iconsPath, "FileTypePDF.png")));
            _nodeImageList.Images.Add("CSV", Image.FromFile(Path.Combine(iconsPath, "FileTypeCSV.png")));
            _nodeImageList.Images.Add("XLS", Image.FromFile(Path.Combine(iconsPath, "FileTypeXLS.png")));
            _nodeImageList.Images.Add("XLSX", Image.FromFile(Path.Combine(iconsPath, "FileTypeXLSX.png")));
        }
        catch (Exception ex)
        {
            MessageBox.Show(string.Format(ICONS_LOAD_ERROR, ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Loads icons for expand/collapse indicators
    /// </summary>
    private void LoadExpandCollapseIcons()
    {
        try
        {
            string iconsPath = OtherMethods.IconsPath;
            _expandCollapseImageList.Images.Add("plus", Image.FromFile(Path.Combine(iconsPath, "UpArrow.png")));
            _expandCollapseImageList.Images.Add("minus", Image.FromFile(Path.Combine(iconsPath, "DownArrow.png")));
        }
        catch (Exception ex)
        {
            MessageBox.Show(string.Format(EXPAND_ICONS_LOAD_ERROR, ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Configures event handlers for the TreeView
    /// </summary>
    private void ConfigureEventHandlers()
    {
        this.ImageList = _nodeImageList;
        this.StateImageList = _expandCollapseImageList;
        this.BeforeExpand += TreeView_BeforeExpand;
        this.AfterExpand += TreeView_AfterExpand;
        this.AfterCollapse += TreeView_AfterCollapse;
    }

    /// <summary>
    /// Refreshes the TreeView content
    /// </summary>
    public void RefreshTreeView()
    {
        if (!string.IsNullOrEmpty(_currentPath))
        {
            LoadDirectory(_currentPath);
        }
    }

    /// <summary>
    /// Loads the directory structure into the TreeView
    /// </summary>
    /// <param name="path">Path to the directory to load</param>
    public void LoadDirectory(string path)
    {
        this.Nodes.Clear();

        if (!Directory.Exists(path))
        {
            MessageBox.Show(DIRECTORY_NOT_FOUND, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var rootDirectoryInfo = new DirectoryInfo(path);
        var rootNode = new TreeNode(rootDirectoryInfo.Name)
        {
            Tag = rootDirectoryInfo,
            ImageIndex = 0,
            StateImageIndex = _expandCollapseImageList.Images.IndexOfKey("plus")
        };

        this.Nodes.Add(rootNode);
        rootNode.Nodes.Add(new TreeNode(LOADING_TEXT));
    }

    /// <summary>
    /// Loads subdirectories and files for a given node
    /// </summary>
    private void LoadSubDirectories(TreeNode parentNode)
    {
        parentNode.Nodes.Clear();
        var directoryInfo = (DirectoryInfo)parentNode.Tag;

        try
        {
            foreach (var directory in directoryInfo.GetDirectories())
            {
                var directoryNode = new TreeNode(directory.Name)
                {
                    Tag = directory,
                    ImageIndex = 0,
                    SelectedImageIndex = 0,
                    StateImageIndex = _expandCollapseImageList.Images.IndexOfKey("plus")
                };
                directoryNode.Nodes.Add(new TreeNode(LOADING_TEXT));
                parentNode.Nodes.Add(directoryNode);
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                int iconIndex = GetIconIndexForFile(file.Name);

                var fileNode = new TreeNode(file.Name)
                {
                    Tag = file,
                    ImageIndex = iconIndex,
                    SelectedImageIndex = iconIndex,
                    StateImageIndex = -1
                };
                parentNode.Nodes.Add(fileNode);
            }
        }
        catch (UnauthorizedAccessException)
        {
            parentNode.Nodes.Add(ACCESS_DENIED_TEXT);
        }
        catch (Exception ex)
        {
            parentNode.Nodes.Add(ERROR_PREFIX + ex.Message);
        }
    }

    /// <summary>
    /// Gets the appropriate icon index for a file based on its extension
    /// </summary>
    private int GetIconIndexForFile(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLower();

        switch (extension)
        {
            case ".txt": return 2;
            case ".doc": return 3;
            case ".docx": return 4;
            case ".pdf": return 5;
            case ".csv": return 6;
            case ".xls": return 7;
            case ".xlsx": return 8;
            default: return 1;
        }
    }

    private void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
        if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == LOADING_TEXT)
        {
            LoadSubDirectories(e.Node);
            e.Node.StateImageIndex = _expandCollapseImageList.Images.IndexOfKey("minus");
        }
    }

    private void TreeView_AfterExpand(object sender, TreeViewEventArgs e)
    {
        e.Node.StateImageIndex = _expandCollapseImageList.Images.IndexOfKey("minus");
    }

    private void TreeView_AfterCollapse(object sender, TreeViewEventArgs e)
    {
        e.Node.StateImageIndex = _expandCollapseImageList.Images.IndexOfKey("plus");
    }
}

/// <summary>
/// Custom RichTextBox control for displaying various file formats
/// </summary>
public class ObservationRichTextBox : RichTextBox
{
    private const string FILE_NOT_FOUND = "Файл не найден!";
    private const string LOAD_ERROR = "Ошибка загрузки файла: {0}";
    private const string WORD_LOAD_ERROR = "Ошибка загрузки Word: {0}";
    private const string PDF_LOAD_ERROR = "Ошибка загрузки PDF: {0}";
    private const string CSV_LOAD_ERROR = "Ошибка загрузки CSV: {0}";
    private const string EXCEL_LOAD_ERROR = "Ошибка загрузки Excel: {0}";

    private static readonly string[] SpireWarningPatterns = {
        @"Evaluation Warning\s*:\s*The document was created with Spire\.(Doc|PDF|XLS) for \.NET\.",
        @"Evaluation Warning\s*:\s*.*?Spire\..*?",
        @"^\s*Spire\..*?$",
        @"\bEvaluation\b.*?\bSpire\b"
    };

    /// <summary>
    /// Initializes a new instance of the ObservationRichTextBox class
    /// </summary>
    public ObservationRichTextBox()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Initializes the RichTextBox control with default settings
    /// </summary>
    private void InitializeComponent()
    {
        this.Dock = DockStyle.Fill;
        this.DetectUrls = true;
        this.Multiline = true;
        this.WordWrap = true;
        this.ScrollBars = RichTextBoxScrollBars.Both;
        this.ContextMenuStrip = CreateContextMenu();
    }

    /// <summary>
    /// Removes Spire evaluation warnings from content
    /// </summary>
    /// <param name="content">The content to clean</param>
    /// <param name="isRtf">Whether the content is in RTF format</param>
    /// <returns>Cleaned content without Spire warnings</returns>
    private string RemoveSpireWarnings(string content, bool isRtf = false)
    {
        if (string.IsNullOrEmpty(content))
            return content;

        foreach (var pattern in SpireWarningPatterns)
        {
            content = Regex.Replace(content, pattern, "",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        // Специфичная обработка для RTF

        if (isRtf)
        {
            string rtfPattern = @"\\pard(\\\w+)*\\qc.*?Evaluation Warning.*?\\par";
            content = Regex.Replace(content, rtfPattern, @"\pard",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        return content.Trim();
    }

    /// <summary>
    /// Displays the contents of a file in the RichTextBox
    /// </summary>
    /// <param name="filePath">Path to the file to display</param>
    public void DisplayFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            this.Text = FILE_NOT_FOUND;
            return;
        }

        string extension = Path.GetExtension(filePath).ToLower();

        try
        {
            switch (extension)
            {
                case ".docx":
                    DisplayWord(filePath);
                    break;
                case ".pdf":
                    DisplayPdfText(filePath);
                    break;
                case ".csv":
                    DisplayCsv(filePath);
                    break;
                case ".xls":
                case ".xlsx":
                    DisplayExcel(filePath);
                    break;
                default:
                    string text = File.ReadAllText(filePath, Encoding.UTF8);
                    this.Text = RemoveSpireWarnings(text);
                    break;
            }
        }
        catch (Exception ex)
        {
            this.Text = string.Format(LOAD_ERROR, ex.Message);
        }
    }

    /// <summary>
    /// Displays a Word document in the RichTextBox
    /// </summary>
    private void DisplayWord(string filePath)
    {
        try
        {
            using (var doc = new Spire.Doc.Document())
            {
                doc.LoadFromFile(filePath, Spire.Doc.FileFormat.Docx, XHTMLValidationType.None);
                doc.IsUpdateFields = false;

                using (MemoryStream ms = new MemoryStream())
                {
                    doc.SaveToStream(ms, Spire.Doc.FileFormat.Rtf);
                    ms.Seek(0, SeekOrigin.Begin);

                    using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        string rtfContent = reader.ReadToEnd();
                        rtfContent = RemoveSpireWarnings(rtfContent, true);

                        using (MemoryStream fixedMs = new MemoryStream(Encoding.UTF8.GetBytes(rtfContent)))
                        {
                            this.LoadFile(fixedMs, RichTextBoxStreamType.RichText);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.Text = string.Format(WORD_LOAD_ERROR, ex.Message);
        }
    }

    /// <summary>
    /// Displays a PDF document in the RichTextBox
    /// </summary>
    private void DisplayPdfText(string filePath)
    {
        try
        {
            using (var pdf = new PdfDocument())
            {
                pdf.LoadFromFile(filePath);

                StringBuilder sb = new StringBuilder();

                foreach (PdfPageBase page in pdf.Pages)
                {
                    PdfTextExtractor extractor = new PdfTextExtractor(page);
                    PdfTextExtractOptions option = new PdfTextExtractOptions
                    {
                        IsExtractAllText = true
                    };

                    string text = extractor.ExtractText(option);
                    sb.AppendLine(text);
                }

                string cleanedText = RemoveSpireWarnings(sb.ToString());
                this.Text = cleanedText;
            }
        }
        catch (Exception ex)
        {
            this.Text = string.Format(PDF_LOAD_ERROR, ex.Message);
        }
    }

    /// <summary>
    /// Displays a CSV file in the RichTextBox
    /// </summary>
    private void DisplayCsv(string filePath)
    {
        try
        {
            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            StringBuilder sb = new StringBuilder();

            foreach (var line in lines)
            {
                var columns = line.Split(',');
                sb.AppendLine(string.Join("\t", columns));
            }

            string cleanedText = RemoveSpireWarnings(sb.ToString());
            this.Text = cleanedText;
            this.Font = new Font("Courier New", 10);
        }
        catch (Exception ex)
        {
            this.Text = string.Format(CSV_LOAD_ERROR, ex.Message);
        }
    }

    /// <summary>
    /// Displays an Excel file in the RichTextBox
    /// </summary>
    private void DisplayExcel(string filePath)
    {
        try
        {
            using (var workbook = new Spire.Xls.Workbook())
            {
                workbook.LoadFromFile(filePath);

                var sheet = workbook.Worksheets[0];
                var dataTable = sheet.ExportDataTable();

                StringBuilder sb = new StringBuilder();
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (var cell in row.ItemArray)
                    {
                        sb.Append(cell?.ToString()?.PadRight(20) + "\t");
                    }
                    sb.AppendLine();
                }

                string cleanedText = RemoveSpireWarnings(sb.ToString());
                this.Text = cleanedText;
                this.Font = new Font("Courier New", 10);
            }
        }
        catch (Exception ex)
        {
            this.Text = string.Format(EXCEL_LOAD_ERROR, ex.Message);
        }
    }

    /// <summary>
    /// Creates a context menu for the RichTextBox
    /// </summary>
    private ContextMenuStrip CreateContextMenu()
    {
        var menu = new ContextMenuStrip();
        var copyItem = new ToolStripMenuItem("Копировать");
        copyItem.Click += (s, e) => this.Copy();
        var selectAllItem = new ToolStripMenuItem("Выделить всё");
        selectAllItem.Click += (s, e) => this.SelectAll();
        menu.Items.AddRange(new ToolStripItem[] { copyItem, selectAllItem });
        return menu;
    }
}