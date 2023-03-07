using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class ColorTheme : Form
    {
        /// <summary>
        /// Запускает конструктор формы.
        /// </summary>
        public ColorTheme()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Выбранный цвет темы.
        /// </summary>
        public string ColorChoice { get; set; }        

        /// <summary>
        /// Выбор светлой темы.
        /// </summary>
        private void ButtonLight_Click(object sender, EventArgs e)
        {
            ColorChoice = "l";
        }

        /// <summary>
        /// Выбор тёмной темы.
        /// </summary>
        private void ButtonDark_Click(object sender, EventArgs e)
        {
            ColorChoice = "d";
        }

        /// <summary>
        /// Выбор синей темы.
        /// </summary>
        private void ButtonBlue_Click(object sender, EventArgs e)
        {
            ColorChoice = "b";
        }
        
        /// <summary>
        /// Закрывает форму при нажатии на кнопку "ОК".
        /// </summary>
        private void ButtonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
