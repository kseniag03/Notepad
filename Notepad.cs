using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class Notepad : Form
    {
        // Флаг для закрытия окна.
        private static bool s_closed = true;
        // Определяет язык интерфейса.
        private static string s_language = "rus";
        // Отслеживают время таймера.
        private int _timeCntFirst;
        private int _timeCntSecond;
        // Счётчик для имени файла.
        private int _cnt = default;
        // Папка для хранения версий файлов.
        private DirectoryInfo _directory = Directory.CreateDirectory("../loggs");
        // Список окон Notepad.
        private List<Notepad> _formList;

        /// <summary>
        /// Частота сохранения файлов.
        /// </summary>
        private int AutosaveFrequency { get; set; }
        /// <summary>
        /// Частота сохранения версий файла.
        /// </summary>
        private int AutosaveLoggsFrequency { get; set; }

        /// <summary>
        /// Запускает конструктор формы.
        /// </summary>
        public Notepad()
        {
            InitializeComponent();
            _formList = new();
            _formList.Add(this);
            LightTheme();
            // 1000 миллисекунд.   
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += Timer_Tick;
            timer.Start();
            // 60 секунд.
            AutosaveFrequency = 60;
            AutosaveLoggsFrequency = 60;            
        }

        /// <summary>
        /// Открывает файл в новой вкладке.
        /// </summary>
        private void OpenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == String.Empty) return;
            using (openFileDialog1)
            {
                try
                {
                    FileInfo fileInfo = new(openFileDialog1.FileName);
                    TabTextPage newTab = new(fileInfo.Name, contextMenuStrip);
                    tabControl.TabPages.Add(newTab);
                    tabControl.SelectedTab = newTab;

                    (tabControl.SelectedTab as TabTextPage).FileInfo = fileInfo;
                    tabControl.SelectedTab.Text = (tabControl.SelectedTab as TabTextPage).FileInfo.Name;

                    var Reader = new StreamReader(openFileDialog1.FileName, Encoding.UTF8);

                    TabPage currentTab = tabControl.SelectedTab;
                    RichTextBox currentTextBox = (tabControl.SelectedTab as TabTextPage).TextBox;

                    if (Path.GetExtension((tabControl.SelectedTab as TabTextPage).FileInfo.FullName).Equals(".rtf"))
                    {
                        currentTextBox.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.RichText);
                    }
                    else
                    {
                        currentTextBox.Text = Reader.ReadToEnd();
                    }

                    Reader.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Запускает сохранение файла.
        /// </summary>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;
            if (!(tabControl.SelectedTab as TabTextPage).IsSaved && tabControl.SelectedTab.Name == String.Empty)
            {
                SaveAsToolStripMenuItem1_Click(sender, e);
                return;
            }
            else
            {
                using (saveFileDialog1)
                {
                    Save(tabControl.SelectedTab, (tabControl.SelectedTab as TabTextPage).FileInfo.FullName);
                }
            }
        }

        /// <summary>
        /// Запускает сохранение файла под новым именем.
        /// </summary>
        private void SaveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;
            using (saveFileDialog1)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Save(tabControl.SelectedTab, saveFileDialog1.FileName);
                    (tabControl.SelectedTab as TabTextPage).FileInfo = new FileInfo(saveFileDialog1.FileName);
                    tabControl.SelectedTab.Text = (tabControl.SelectedTab as TabTextPage).FileInfo.Name;
                }
            }
        }

        /// <summary>
        /// Сохраняет все файлы с открытых файлов.
        /// </summary>
        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;
            foreach (TabPage page in tabControl.TabPages)
            {
                if (!(page as TabTextPage).IsSaved && tabControl.SelectedTab.Name == String.Empty)
                {
                    using (saveFileDialog1)
                    {
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            Save(page, saveFileDialog1.FileName);
                            (page as TabTextPage).FileInfo = new(saveFileDialog1.FileName);
                            page.Text = (page as TabTextPage).FileInfo.Name;
                        }
                    }
                }
                else if (saveFileDialog1.FileName != String.Empty)
                {
                    using (saveFileDialog1)
                    {
                        Save(page, (page as TabTextPage).FileInfo.FullName);
                    }
                }
            }
        }

        /// <summary>
        /// Сохраняет файл.
        /// </summary>
        /// <param name="page"> Текущая вкладка </param>
        /// <param name="path"> Путь к фалу </param>
        private void Save(TabPage page, string path)
        {
            if (tabControl.SelectedTab == null) return;
            try
            {
                RichTextBox currentTextBox = (page as TabTextPage).TextBox;
                if (Path.GetExtension(path).Equals(".rtf"))
                {
                    currentTextBox.SaveFile(path);
                }
                else
                {
                    var Writer = new StreamWriter(path, false, Encoding.UTF8);
                    Writer.Write(currentTextBox.Text);
                    Writer.Close();
                }
                (page as TabTextPage).IsSaved = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Запускает сохранение файлов по таймеру.
        /// </summary>  
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;
            _timeCntFirst++;
            _timeCntSecond++;
            ++_cnt;
            if (AutosaveFrequency <= _timeCntFirst)
            {
                _timeCntFirst = 0;
                try
                {
                    foreach (TabPage page in tabControl.TabPages)
                    {
                        if ((page as TabTextPage).FileInfo != null)
                        {
                            Save(page, (page as TabTextPage).FileInfo.FullName);
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            if (AutosaveLoggsFrequency <= _timeCntSecond)
            {
                _timeCntSecond = 0;
                foreach (TabPage page in tabControl.TabPages)
                {
                    if ((page as TabTextPage).FileInfo != null)
                    {
                        Logging(page, _directory.FullName, (_cnt / AutosaveLoggsFrequency).ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Сохраняет файлы в папку с логами.
        /// </summary>
        /// <param name="page"> Текущая вкладка </param>
        /// <param name="path"> Путь к файлу </param>
        /// <param name="text"> Текст для сохранения разных версий файла </param>
        private void Logging(TabPage page, string path, string text)
        {
            if ((page as TabTextPage).FileInfo != null)
            {
                try
                {
                    RichTextBox currentTextBox = (page as TabTextPage).TextBox;
                    path = Path.Combine(path, text + Path.GetExtension((page as TabTextPage).FileInfo.FullName));
                    if (Path.GetExtension(path).Equals(".rtf"))
                    {
                        currentTextBox.SaveFile(path);
                    }
                    else
                    {
                        var Writer = new StreamWriter(path, false, Encoding.UTF8);
                        Writer.Write(currentTextBox.Text);
                        Writer.Close();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Закрывает текущую вкладку.
        /// </summary>
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;
            if (!(tabControl.SelectedTab as TabTextPage).IsSaved)
            {
                DialogResult result = MessageBox.Show(s_language == "eng" ? "Save changed file?" : "Сохранить изменения?",
                    "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                switch (result)
                {
                    case DialogResult.Yes:
                        SaveAsToolStripMenuItem1_Click(sender, e);
                        break;
                    case DialogResult.No:
                        tabControl.TabPages.RemoveAt(tabControl.SelectedIndex);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                tabControl.TabPages.RemoveAt(tabControl.SelectedIndex);
            }
        }

        /// <summary>
        /// Предлагает сохранить изменённые файлы.
        /// </summary>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;
            if (!s_closed)
            {
                if (!(tabControl.SelectedTab as TabTextPage).IsSaved)
                {
                    DialogResult result = MessageBox.Show(s_language == "eng" ? "Save changed file?" : "Сохранить изменения?",
                        "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            SaveAsToolStripMenuItem1_Click(sender, e);
                            break;
                        case DialogResult.No:
                            s_closed = true;
                            Close();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    s_closed = true;
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Устанавливает фильтр для открытия и сохранения файлов.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.FileName = @"text.txt";
            openFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt|" +
                "Текстовые файлы (*.rtf)|*.rtf|" +
                "Файлы Visual C# (*.cs; *.csprog)|*.cs; *.csprog|" +
                "Файлы Visual C++ (*.cpp)|*.cpp|" +
                "Веб-файлы (*.html)|*.html|" +
                "Файлы Python (*.py)|*.py";
            saveFileDialog1.FileName = @"text.txt";
            saveFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt|" +
                "Текстовые файлы (*.rtf)|*.rtf|" +
                "Файлы Visual C# (*.cs; *.csprog)|*.cs; *.csprog|" +
                "Файлы Visual C++ (*.cpp)|*.cpp|" +
                "Веб-файлы (*.html)|*.html|" +
                "Файлы Python (*.py)|*.py";
        }

        /// <summary>
        /// Открывает новую вкладку.
        /// </summary>
        private void NewTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            s_closed = false;
            try
            {
                TabTextPage tabPage = new TabTextPage(String.Empty, contextMenuStrip);
                tabControl.TabPages.Add(tabPage);
                (tabControl.SelectedTab as TabTextPage).InterfaceLanguage = "rus";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Отлавливает изменения в тексте.
        /// </summary>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            TabTextPage tabPage = tabControl.SelectedTab as TabTextPage;
            tabPage.IsSaved = false;
        }

        /// <summary>
        /// Закрывает приложение, предварительно предложив сохранить файлы.
        /// </summary>
        private void Notepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!s_closed && tabControl.SelectedTab != null)
            {
                if (!(tabControl.SelectedTab as TabTextPage).IsSaved)
                {
                    DialogResult result = MessageBox.Show(s_language == "eng" ? "Save changed file?" : "Сохранить изменения?",
                        "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            SaveAsToolStripMenuItem1_Click(sender, e);
                            e.Cancel = true;
                            break;
                        case DialogResult.No:
                            e.Cancel = false;
                            break;
                        default:
                            e.Cancel = true;
                            break;
                    }
                }
                else
                {
                    e.Cancel = false;
                }
            }
            else
            {
                e.Cancel = false;
            }
            _directory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        }

        /// <summary>
        /// Открывает новое окно.
        /// </summary>
        private void NewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Notepad notePad = new Notepad();
            _formList.Add(notePad);
            notePad.Show();
        }

        /// <summary>
        /// Устанавливает полужирный стиль текста.
        /// </summary>
        private void BoldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;

            RichTextBox currentTextBox = (tabControl.SelectedTab as TabTextPage).TextBox;
            Font currentFont = currentTextBox.SelectionFont;
            if (currentFont == null) return;
            currentTextBox.SelectionFont = new Font(currentFont, currentFont.Style ^ FontStyle.Bold);
        }

        /// <summary>
        /// Устанавливает курсивный стиль текста.
        /// </summary>
        private void ItalicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;

            RichTextBox currentTextBox = (tabControl.SelectedTab as TabTextPage).TextBox;
            Font currentFont = currentTextBox.SelectionFont;
            if (currentFont == null) return;
            currentTextBox.SelectionFont = new Font(currentFont, currentFont.Style ^ FontStyle.Italic);
        }

        /// <summary>
        /// Устанавливает подчеркнутый стиль текста.
        /// </summary>
        private void UnderlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;

            RichTextBox currentTextBox = (tabControl.SelectedTab as TabTextPage).TextBox;
            Font currentFont = currentTextBox.SelectionFont;
            if (currentFont == null) return;
            currentTextBox.SelectionFont = new Font(currentFont, currentFont.Style ^ FontStyle.Underline);
        }

        /// <summary>
        /// Устанавливает перечеркнутый стиль текста.
        /// </summary>
        private void StrikeThroughToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;

            RichTextBox currentTextBox = (tabControl.SelectedTab as TabTextPage).TextBox;
            Font currentFont = currentTextBox.SelectionFont;
            if (currentFont == null) return;
            currentTextBox.SelectionFont = new Font(currentFont, currentFont.Style ^ FontStyle.Strikeout);
        }

        /// <summary>
        /// Запускает смену шрифта.
        /// </summary>
        private void ChangeFont(object sender, EventArgs e, string fontName)
        {
            if (tabControl.SelectedTab == null) return;

            RichTextBox currentTextBox = (tabControl.SelectedTab as TabTextPage).TextBox;
            currentTextBox.Font = new Font(fontName, currentTextBox.SelectionFont.Size);
        }

        /// <summary>
        /// Устанавливает шрифт Arial.
        /// </summary>
        private void ArialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFont(sender, e, "Arial");
        }

        /// <summary>
        /// Устанавливает шрифт Calibri.
        /// </summary>
        private void CalibriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFont(sender, e, "Calibri");
        }

        /// <summary>
        /// Устанавливает шрифт Comic Sans MS.
        /// </summary>
        private void ComicSansMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFont(sender, e, "Comic Sans MS");
        }

        /// <summary>
        /// Устанавливает шрифт Segoe UI.
        /// </summary>
        private void SegoeUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFont(sender, e, "Segoe UI");
        }

        /// <summary>
        /// Устанавливает шрифт Times New Roman.
        /// </summary>
        private void TimesNewRomanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFont(sender, e, "Times New Roman");
        }

        /// <summary>
        /// Открывает окно для выбора шрифта.
        /// </summary>
        private void OtherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;

            fontDialog.ShowColor = true;

            RichTextBox currentTextBox = (tabControl.SelectedTab as TabTextPage).TextBox;

            fontDialog.Font = currentTextBox.Font;
            fontDialog.Color = currentTextBox.ForeColor;

            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                currentTextBox.Font = fontDialog.Font;
                currentTextBox.ForeColor = fontDialog.Color;
            }
        }

        /// <summary>
        /// Отменяет последнее действие.
        /// </summary>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;
            try
            {
                RichTextBox currentTextBox = (tabControl.SelectedTab as TabTextPage).TextBox;
                this.SuspendLayout();
                currentTextBox.Undo();
                this.ResumeLayout();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Возвращает отменённое действие.
        /// </summary>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;
            try
            {
                RichTextBox currentTextBox = (tabControl.SelectedTab as TabTextPage).TextBox;
                this.SuspendLayout();
                currentTextBox.Redo();
                this.ResumeLayout();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Переводит текст интерфейса на русский язык.
        /// </summary>
        private void RussianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            s_language = "rus";
            fileToolStripMenuItem2.Text = "Файл";
            editToolStripMenuItem.Text = "Правка";
            formatToolStripMenuItem.Text = "Формат";
            settingsToolStripMenuItem.Text = "Настройки";
            newTabToolStripMenuItem.Text = "Создать";
            newWindowToolStripMenuItem.Text = "Новое окно";
            openToolStripMenuItem.Text = "Открыть";
            saveToolStripMenuItem.Text = "Сохранить";
            saveAsToolStripMenuItem.Text = "Сохранить как";
            saveAllToolStripMenuItem.Text = "Сохранить все";
            closeToolStripMenuItem.Text = "Закрыть вкладку";
            exitToolStripMenuItem.Text = "Выход";
            undoToolStripMenuItem.Text = "Отменить";
            redoToolStripMenuItem.Text = "Вернуть";
            boldToolStripMenuItem.Text = "Полужирный";
            italicToolStripMenuItem.Text = "Курсив";
            underlineToolStripMenuItem.Text = "Подчеркнутый";
            strikeThroughToolStripMenuItem.Text = "Зачеркнутый";
            fontToolStripMenuItem.Text = "Шрифт";
            otherToolStripMenuItem.Text = "Другие";
            autosaveFrequencyToolStripMenuItem.Text = "Частота автосохранения";
            colorThemeToolStripMenuItem.Text = "Цветовая тема";
            interfaceLanguageToolStripMenuItem.Text = "Язык интерфейса";
            russianToolStripMenuItem.Text = "Русский";
            englishToolStripMenuItem.Text = "Английский";
            selectAllToolStripMenuItem.Text = "Выделить все";
            cutToolStripMenuItem.Text = "Вырезать";
            copyToolStripMenuItem.Text = "Копировать";
            pasteToolStripMenuItem.Text = "Вставить";
            formatToolStripMenuItem1.Text = "Формат";
        }

        /// <summary>
        /// Переводит текст интерфейса на английский язык.
        /// </summary>
        private void EnglishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            s_language = "eng";
            fileToolStripMenuItem2.Text = "File";
            editToolStripMenuItem.Text = "Edit";
            formatToolStripMenuItem.Text = "Format";
            settingsToolStripMenuItem.Text = "Settings";
            newTabToolStripMenuItem.Text = "Create";
            newWindowToolStripMenuItem.Text = "New Window";
            openToolStripMenuItem.Text = "Open";
            saveToolStripMenuItem.Text = "Save";
            saveAsToolStripMenuItem.Text = "Save as";
            saveAllToolStripMenuItem.Text = "Save all";
            closeToolStripMenuItem.Text = "Close the tab";
            exitToolStripMenuItem.Text = "Exit";
            undoToolStripMenuItem.Text = "Undo";
            redoToolStripMenuItem.Text = "Redo";
            boldToolStripMenuItem.Text = "Bold";
            italicToolStripMenuItem.Text = "Italic";
            underlineToolStripMenuItem.Text = "Underline";
            strikeThroughToolStripMenuItem.Text = "Strike Through";
            fontToolStripMenuItem.Text = "Font";
            otherToolStripMenuItem.Text = "Other";
            autosaveFrequencyToolStripMenuItem.Text = "Autosave frequency";
            colorThemeToolStripMenuItem.Text = "Color theme";
            interfaceLanguageToolStripMenuItem.Text = "Interface language";
            russianToolStripMenuItem.Text = "Russian";
            englishToolStripMenuItem.Text = "English";
            selectAllToolStripMenuItem.Text = "Select All";
            cutToolStripMenuItem.Text = "Cut";
            copyToolStripMenuItem.Text = "Copy";
            pasteToolStripMenuItem.Text = "Paste";
            formatToolStripMenuItem1.Text = "Format";
        }

        /// <summary>
        /// Выбирает частоту автосохранения.
        /// </summary>
        private void AutosaveFrequencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Frequency window = new();
                window.ShowDialog();
                AutosaveFrequency = window.ChosenValue;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AutosaveLoggsFrequencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Frequency window = new();
                window.ShowDialog();
                AutosaveLoggsFrequency = window.ChosenValue;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Выделяет весь текст на текущей вкладке.
        /// </summary>
        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                (tabControl.SelectedTab as TabTextPage).TextBox.SelectAll();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Вырезает выделенный фрагмент текста.
        /// </summary>
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                (tabControl.SelectedTab as TabTextPage).TextBox.Cut();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Копирует выделенный фрагмент текста.
        /// </summary>
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                (tabControl.SelectedTab as TabTextPage).TextBox.Copy();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Вставляет фрагмент текста из буфера обмена.
        /// </summary>
        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                (tabControl.SelectedTab as TabTextPage).TextBox.Paste();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Устанавливает формат (шрифт, размер) выделенного фрагмента текста.
        /// </summary>
        private void FormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;
            try
            {
                if (fontDialog.ShowDialog(this) == DialogResult.OK)
                {
                    (tabControl.SelectedTab as TabTextPage).TextBox.SelectionFont = fontDialog.Font;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Запускает смену цветовой темы.
        /// </summary>
        private void СolorThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ColorTheme window = new();
                window.ShowDialog();
                string color = window.ColorChoice;
                switch (color)
                {
                    case "d":
                        DarkTheme();
                        break;
                    case "b":
                        BlueTheme();
                        break;
                    default:
                        LightTheme();
                        break;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, s_language == "eng" ? "Error" : "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Светлая тема (тема по умолчанию).
        /// </summary>
        private void LightTheme()
        {
            foreach (Notepad notepad in _formList)
            {
                notepad.BackColor = Color.Azure;
                notepad.ForeColor = Color.Black;
                foreach (TabPage page in tabControl.TabPages)
                {
                    if ((page as TabTextPage).TextBox != null)
                    {
                        (page as TabTextPage).TextBox.BackColor = Color.White;
                        (page as TabTextPage).TextBox.ForeColor = Color.Black;
                    }
                }
            }
        }

        /// <summary>
        /// Тёмная тема.
        /// </summary>
        private void DarkTheme()
        {
            foreach (Notepad notepad in _formList)
            {
                notepad.BackColor = Color.Black;
                notepad.ForeColor = Color.Lavender;
                foreach (TabPage page in tabControl.TabPages)
                {
                    if ((page as TabTextPage).TextBox != null)
                    {
                        (page as TabTextPage).TextBox.BackColor = Color.DarkSlateBlue;
                        (page as TabTextPage).TextBox.ForeColor = Color.Lavender;
                    }
                }
            }
        }

        /// <summary>
        /// Синяя тема.
        /// </summary>
        private void BlueTheme()
        {
            foreach (Notepad notepad in _formList)
            {
                notepad.BackColor = Color.LightSlateGray;
                notepad.ForeColor = Color.AliceBlue;
                foreach (TabPage page in tabControl.TabPages)
                {
                    if ((page as TabTextPage).TextBox != null)
                    {
                        (page as TabTextPage).TextBox.BackColor = Color.LightSteelBlue;
                        (page as TabTextPage).TextBox.ForeColor = Color.Snow;
                    }
                }
            }
        }
    }
}
