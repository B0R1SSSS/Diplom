using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using DEP.Controls;
using System.Linq;

namespace DEP
{
    /// <summary>
    /// Оффлайн-форма для работы с учебными материалами без подключения к базе данных.
    /// </summary>
    public partial class OfflineForm : Form
    {
        private ObservationTreeView _examTreeView;
        private ObservationRichTextBox _enhancedRichTextBox;
        private string _currentPath = OtherMethods.GetDefaultPath();

        public OfflineForm()
        {
            this.MaximizeBox = false;
            InitializeComponent();
            InitializeOfflineComponents();
            
            // Subscribe to form closing event
            this.FormClosing += OfflineForm_FormClosing;
        }

        private void InitializeOfflineComponents()
        {
            _examTreeView = new ObservationTreeView();
            _examTreeView.Dock = DockStyle.Fill;
            TreeViewPanel.Controls.Add(_examTreeView);

            _enhancedRichTextBox = new ObservationRichTextBox();
            _enhancedRichTextBox.Dock = DockStyle.Fill;
            FileViewerPanel.Controls.Add(_enhancedRichTextBox);

            _examTreeView.AfterSelect += ExamTreeView_AfterSelect;
        }

        private void ExamTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is FileInfo fileInfo)
                _enhancedRichTextBox.DisplayFile(fileInfo.FullName);
            else
                _enhancedRichTextBox.Clear();
        }

        private void OpenFolderButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(_currentPath))
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = _currentPath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show($"Корневая папка не найдена:\n{_currentPath}",
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

        /// <summary>
        /// Handles form closing event
        /// Returns user to the StartingForm when form is closed
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void OfflineForm_FormClosing(object sender, FormClosingEventArgs e)
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