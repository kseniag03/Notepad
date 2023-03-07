using System.IO;
using System.Windows.Forms;

namespace Notepad
{
    /// <summary>
    /// Класс вкладки с расширенным функционалом, привязанным RichTextBox.
    /// </summary>
    class TabTextPage : TabPage
    {
        // Привязанный к вкладке RichTextBox
        private RichTextBox _richTextBox;

        /// <summary>
        /// Возвращает RichTextBox, привязанный к вкладке.
        /// </summary>
        public RichTextBox TextBox
        {
            get
            {
                return _richTextBox;
            }
        }

        /// <summary>
        /// Возвращает язык интерфейса текущей вкладки.
        /// </summary>
        public string InterfaceLanguage { get; set; }

        /// <summary>
        /// Проверяет, сохранен ли файл.
        /// </summary>
        public bool IsSaved { get; set; }

        /// <summary>
        /// Возвращает информацию о файле с текущей вкладки.
        /// </summary>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Запускает конструктор вкладки.
        /// </summary>
        public TabTextPage(string text, ContextMenuStrip contextMenu): base()
        {
            this.Text = text; 
            _richTextBox = new();
            _richTextBox.Dock = DockStyle.Fill;
            _richTextBox.ContextMenuStrip = contextMenu;
            this.Controls.Add(_richTextBox);
        }
    }
}
