using System;
using System.Windows.Forms;

namespace DEP_BETA.DEP.DEP.Forms
{
    public partial class AddInfoForm : Form
    {
        public AddInfoForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Применяем стиль к форме
            FormStyleManager.ApplyFormStyle(this);
            this.Text = "Дополнительная информация";

            // Настройка заголовка
            TitleLabel.Font = FormStyleManager.TitleFont;
            FormStyleManager.ApplyLabelStyle(TitleLabel);

            // Настройка меток
            FormStyleManager.ApplyLabelStyle(InfoLabel);

            // Настройка текстовых полей
            FormStyleManager.ApplyTextBoxStyle(InfoTextBox);

            // Настройка кнопок
            FormStyleManager.ApplyButtonStyle(SaveButton);
            FormStyleManager.ApplyButtonStyle(ExitButton, true);

            // Настройка панели
            FormStyleManager.ApplyPanelStyle(panel1);
        }
    }
} 