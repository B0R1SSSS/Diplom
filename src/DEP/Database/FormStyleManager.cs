using System;
using System.Drawing;
using System.Windows.Forms;

namespace DEP.Classes
{
    /// <summary>
    /// Manages form styling across the application
    /// </summary>
    public static class FormStyleManager
    {
        // Color constants
        public static readonly Color MainBackgroundColor = Color.FromArgb(240, 240, 240);
        public static readonly Color PanelBackgroundColor = Color.White;
        public static readonly Color TextColor = Color.FromArgb(64, 64, 64);
        public static readonly Color PrimaryButtonColor = Color.FromArgb(0, 122, 204);
        public static readonly Color PrimaryButtonHoverColor = Color.FromArgb(0, 102, 184);
        public static readonly Color DangerButtonColor = Color.FromArgb(220, 53, 69);
        public static readonly Color DangerButtonHoverColor = Color.FromArgb(200, 35, 51);

        // Font constants
        public static readonly Font TitleFont = new Font("Segoe UI", 16, FontStyle.Bold);
        public static readonly Font ButtonFont = new Font("Segoe UI", 10, FontStyle.Bold);
        public static readonly Font LabelFont = new Font("Segoe UI", 10);
        public static readonly Font TextBoxFont = new Font("Segoe UI", 10);

        /// <summary>
        /// Applies standard form styling
        /// </summary>
        /// <param name="form">Form to style</param>
        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = MainBackgroundColor;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Applies standard button styling
        /// </summary>
        /// <param name="button">Button to style</param>
        /// <param name="isDanger">Whether this is a danger button (red)</param>
        public static void ApplyButtonStyle(Button button, bool isDanger = false)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = isDanger ? DangerButtonColor : PrimaryButtonColor;
            button.ForeColor = Color.White;
            button.Font = ButtonFont;
            button.Cursor = Cursors.Hand;

            // Add hover effects
            button.MouseEnter += (s, e) => 
                button.BackColor = isDanger ? DangerButtonHoverColor : PrimaryButtonHoverColor;
            button.MouseLeave += (s, e) => 
                button.BackColor = isDanger ? DangerButtonColor : PrimaryButtonColor;
        }

        /// <summary>
        /// Applies standard label styling
        /// </summary>
        /// <param name="label">Label to style</param>
        public static void ApplyLabelStyle(Label label)
        {
            label.Font = LabelFont;
            label.ForeColor = TextColor;
            label.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Applies standard textbox styling
        /// </summary>
        /// <param name="textBox">TextBox to style</param>
        public static void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.Font = TextBoxFont;
        }

        /// <summary>
        /// Applies standard panel styling
        /// </summary>
        /// <param name="panel">Panel to style</param>
        public static void ApplyPanelStyle(Panel panel)
        {
            panel.BackColor = PanelBackgroundColor;
            panel.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Applies standard combobox styling
        /// </summary>
        /// <param name="comboBox">ComboBox to style</param>
        public static void ApplyComboBoxStyle(ComboBox comboBox)
        {
            comboBox.Font = TextBoxFont;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Applies standard datagridview styling
        /// </summary>
        /// <param name="dataGridView">DataGridView to style</param>
        public static void ApplyDataGridViewStyle(DataGridView dataGridView)
        {
            dataGridView.BackgroundColor = PanelBackgroundColor;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Font = TextBoxFont;
        }
    }
} 