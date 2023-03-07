using System;
using System.Windows.Forms;

namespace Notepad
{
    public partial class Frequency : Form
    {
        /// <summary>
        /// Запускает конструктор формы.
        /// </summary>
        public Frequency()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Выбранное значение.
        /// </summary>
        public int ChosenValue { get; set; }

        /// <summary>
        /// Закрывает форму при нажатии на кнопку "ОК".
        /// </summary>
        private void ButtonOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Выбирает частоту – 1 минута.
        /// </summary>
        private void OneMinute_Click(object sender, EventArgs e)
        {
            ChosenValue = 60;
        }

        /// <summary>
        /// Выбирает частоту – 2 минуты.
        /// </summary>
        private void TwoMinutes_Click(object sender, EventArgs e)
        {
            ChosenValue = 120;
        }

        /// <summary>
        /// Выбирает частоту – 5 минут.
        /// </summary>
        private void FiveMinutes_Click(object sender, EventArgs e)
        {
            ChosenValue = 300;
        }
    }
}
