using System;
using System.Windows.Forms;
using DEP.Database;
using DEP.Forms;
using System.Drawing;
using System.IO;
namespace DEP
{
    public partial class StartingForm : Form
    {
        string IconsPath = OtherMethods.IconsPath;
        private Form activeForm = null;
        public int currentUserId;
        private string currentUserRole;
        private string currentUserName;

        /// <summary>
        /// Initializes a new instance of the StartingForm
        /// </summary>
        public StartingForm()
        {
            this.MaximizeBox = false;
            InitializeComponent();
            InitializeForm();
            InitializeLogo();
        }

        /// <summary>
        /// Initializes form components and sets up event handlers
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                

                // Настраиваем поля ввода
                LoginTextBox.KeyPress += (s, e) => 
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        e.Handled = true;
                        PassTextBox.Focus();
                    }
                };

                PassTextBox.KeyPress += (s, e) => 
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        e.Handled = true;
                        PerformLogin();
                    }
                };

                // Устанавливаем фокус на поле логина
                LoginTextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации формы: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Initializes the logo image for the form
        /// </summary>
        private void InitializeLogo()
        {
            try
            {
                string logoPath = Path.Combine(IconsPath, "Logo.png");
                if (File.Exists(logoPath))
                {
                    LogoPictureBox.Image = Image.FromFile(logoPath);
                    LogoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    MessageBox.Show("Файл логотипа не найден", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке логотипа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles login button click
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            PerformLogin();
        }

        /// <summary>
        /// Performs the login operation
        /// </summary>
        private void PerformLogin()
        {
            try
            {
                string username = LoginTextBox.Text.Trim();
                string password = PassTextBox.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Пожалуйста, введите существующий логин и пароль", 
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var authResult = DatabaseManager.Instance.AuthenticateUser(username, password);
                if (authResult.HasValue)
                {
                    currentUserId = authResult.Value.UserId;
                    currentUserRole = authResult.Value.Role;
                    currentUserName = authResult.Value.FullName;

                    Form mainForm;
                    if (currentUserRole.ToLower() == "student")
                    {
                        mainForm = new PrepareModForm(currentUserRole, currentUserId);
                    }
                    else // Для admin и teacher
                    {
                        mainForm = new ReviewTaskForm(currentUserRole, currentUserId);
                    }

                    OpenForm(mainForm);
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", 
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    PassTextBox.Clear();
                    PassTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе в систему: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles exit button click
        /// </summary>
        private void ExitButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите выйти?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выходе: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens a new form and closes the current one
        /// </summary>
        /// <param name="form">Form to open</param>
        private void OpenForm(Form form)
        {
            try
            {
                if (activeForm != null)
                {
                    activeForm.Close();
                }
                activeForm = form;
                form.FormClosed += (s, args) => this.Show();
                form.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens the SubmitTaskForm for submitting a new task
        /// </summary>
        /// <param name="taskId">The ID of the task</param>
        /// <param name="taskTitle">The title of the task</param>
        public void OpenSubmitTaskForm(int taskId, string taskTitle)
        {
            try
            {
                var submitForm = new SubmitTaskForm(currentUserId, taskId, taskTitle);
                OpenForm(submitForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы отправки задания: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens the DownloadReviewTaskForm for downloading and reviewing tasks
        /// </summary>
        

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (MessageBox.Show("Вы уверены, что хотите выйти?",
                        "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                base.OnFormClosing(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при закрытии формы: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OfflineButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            OfflineForm PrepareModForm = new OfflineForm();
            PrepareModForm.FormClosed += (s, args) => this.Show();
            PrepareModForm.Show();
        }
    }
}