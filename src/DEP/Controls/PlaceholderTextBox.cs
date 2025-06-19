using System;
using System.Drawing;
using System.Windows.Forms;

namespace DEP.Controls
{
    /// <summary>
    /// Custom TextBox control with placeholder text support
    /// </summary>
    public class PlaceholderTextBox : TextBox
    {
        private string placeholderText;
        private Color placeholderColor = Color.Gray;
        private Color originalForeColor;

        /// <summary>
        /// Gets or sets the placeholder text
        /// </summary>
        public string PlaceholderText
        {
            get { return placeholderText; }
            set
            {
                placeholderText = value;
                if (string.IsNullOrEmpty(Text))
                {
                    ShowPlaceholder();
                }
            }
        }

        /// <summary>
        /// Gets or sets the placeholder text color
        /// </summary>
        public Color PlaceholderColor
        {
            get { return placeholderColor; }
            set { placeholderColor = value; }
        }

        public PlaceholderTextBox()
        {
            originalForeColor = ForeColor;
            GotFocus += PlaceholderTextBox_GotFocus;
            LostFocus += PlaceholderTextBox_LostFocus;
        }

        private void PlaceholderTextBox_GotFocus(object sender, EventArgs e)
        {
            if (Text == placeholderText)
            {
                Text = string.Empty;
                ForeColor = originalForeColor;
            }
        }

        private void PlaceholderTextBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                ShowPlaceholder();
            }
        }

        private void ShowPlaceholder()
        {
            Text = placeholderText;
            ForeColor = placeholderColor;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (Text != placeholderText)
            {
                ForeColor = originalForeColor;
            }
        }
    }
} 