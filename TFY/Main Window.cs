using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace TFY
{
    public partial class Main_Window : Form
    {
        private struct TabOpts
        {
            public bool New;
            public List<string> UndoBuffer;
            public int BufferIndex;
        }

        private List<Form> Forms = new List<Form>();

        private Lexem_analyzier Lexem_analyzer = new Lexem_analyzier();
        private Recursive_descent RD = new Recursive_descent();
        private Goto_analyzier GA = new Goto_analyzier();
        private List<TabOpts> TabsOpts = new List<TabOpts>();
        private bool SysModified = false;
        private bool DoRegex = false;

        public Main_Window()
        {
            InitializeComponent();
            Init_Forms();
            openFileD.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileD.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            stop_item.Visible = false;
        }

        private void Init_Forms()
        {
            Form TaskForm = new Form();
            TaskForm.Size = new Size(1050, 760);
            Label label = new Label();
            TaskForm.Controls.Add(label);
            label.Text = "Задание для лабораторной 3:\nКонечный автомат";
            label.Location = new Point(500, 10);
            label.Size = new Size(300, 40);
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile("StateMachineGraph.png");
            TaskForm.Controls.Add(pictureBox);
            pictureBox.Location = new Point(10, 50);
            pictureBox.Size = new Size(978, 670);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(1430, 920);
            label = new Label();
            TaskForm.Controls.Add(label);
            label.Text = "Задание для лабораторной 4:\nЛексический анализатор";
            label.Location = new Point(650, 10);
            label.Size = new Size(300, 40);
            pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile("StateDiagramLab4.png");
            TaskForm.Controls.Add(pictureBox);
            pictureBox.Location = new Point(10, 50);
            pictureBox.Size = new Size(1390, 909);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(680, 230);
            pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile("CodeTableLab4.png");
            TaskForm.Controls.Add(pictureBox);
            pictureBox.Location = new Point(10, 10);
            pictureBox.Size = new Size(650, 200);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(650, 450);
            pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile("ExampleLab4.png");
            TaskForm.Controls.Add(pictureBox);
            pictureBox.Location = new Point(10, 10);
            pictureBox.Size = new Size(680, 480);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(200, 190);
            label = new Label();
            TaskForm.Controls.Add(label);
            label.Text = "Грамматика G[E]:\nE → TA\nA → ε | + TA | - TA\nT → ОВ\nВ → ε | *ОВ | /ОВ\nО → num | id | (E)\n\nnum – числовая константа Ц{Ц}\nid – идентификатор Б{Б|Ц}\nБ – [A, B, …, Z], Ц – [0, 1, …, 9]";
            label.Location = new Point(10, 10);
            label.Size = new Size(180, 170);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(200, 110);
            label = new Label();
            TaskForm.Controls.Add(label);
            label.Text = "Язык:\n L(G[E]) = {a + b; 9.231 – b * c; 65 * (b – 3.2); (34 / (2 * c) – (d + e / f)); …}";
            label.Location = new Point(10, 10);
            label.Size = new Size(180, 90);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(320, 210);
            label = new Label();
            TaskForm.Controls.Add(label);
            label.Text = "Классификация грамматики:\nПРАВОЛИНЕЙНАЯ КОНТЕКСТНО СВОБОДНАЯ\n\nОбоснование:\nВ левых частях каждого правила стоят только одиночные нетерминалы, а каждая последующая строка получается из предыдущей путем замены по одному из правил самого правого встречающегося в строке нетерминала";
            label.Location = new Point(10, 10);
            label.Size = new Size(300, 190);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(650, 450);
            pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile("ExampleLab5.png");
            TaskForm.Controls.Add(pictureBox);
            pictureBox.Location = new Point(10, 10);
            pictureBox.Size = new Size(680, 480);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(200, 345);
            label = new Label();
            TaskForm.Controls.Add(label);
            label.Text = "Грамматика G[<OPERATOR>]:\n\n<OPERATOR> -> goto <LIST><EXPR>\r\n<LIST> -> (AB)  \r\nB -> e | , AB\r\nA -> label\r\n<EXPR> -> TC\r\nC -> e | + TC | - TC\r\nT -> ОD\r\nD -> e | *ОD | /ОD\r\nО -> num | id | (<EXPR>)\r\n\nlabel – метка Ц{Ц}\r\nnum – числовая константа [+|-]Ц{Ц}[.Ц{Ц}]\r\nid – идентификатор Б{Б|Ц}\r\nБ – [A, B, …, Z], Ц – [0, 1, …, 9]";
            label.Location = new Point(10, 10);
            label.Size = new Size(180, 325);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(320, 210);
            label = new Label();
            TaskForm.Controls.Add(label);
            label.Text = "Классификация грамматики:\nПРАВОЛИНЕЙНАЯ КОНТЕКСТНО СВОБОДНАЯ\n\nОбоснование:\nВ левых частях каждого правила стоят только одиночные нетерминалы, а каждая последующая строка получается из предыдущей путем замены по одному из правил самого правого встречающегося в строке нетерминала";
            label.Location = new Point(10, 10);
            label.Size = new Size(300, 190);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(400, 120);
            label = new Label();
            TaskForm.Controls.Add(label);
            label.Text = "Метод рекурсивного спуска — алгоритм нисходящего синтаксического анализа, реализуемый путём взаимного вызова процедур, где каждая процедура соответствует одному из правил контекстно-свободной грамматики. Применения правил последовательно поглощают лексемы, полученные от лексического анализатора";
            label.Location = new Point(10, 10);
            label.Size = new Size(380, 100);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(400, 120);
            label = new Label();
            TaskForm.Controls.Add(label);
            label.Text = "Нейтрализация ошибок проводится по методу Айронса\n\nОсновная идея – по контексту без возврата отбрасывать литеры, которые привели к тупиковой ситуации (когда продолжение анализа по грамматике невозможно), и продолжать разбор.";
            label.Location = new Point(10, 10);
            label.Size = new Size(380, 100);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(350, 270);
            pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile("ExampleLab6.png");
            TaskForm.Controls.Add(pictureBox);
            pictureBox.Location = new Point(10, 10);
            pictureBox.Size = new Size(330, 250);

            Forms.Add(TaskForm);

            TaskForm = new Form();
            TaskForm.Size = new Size(620, 170);
            pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile("Language_lab6.png");
            TaskForm.Controls.Add(pictureBox);
            pictureBox.Location = new Point(10, 10);
            pictureBox.Size = new Size(600, 150);

            Forms.Add(TaskForm);
        }

        private void New_tab(string FileName, bool NewState = false)
        {
            TabsOpts.Add(new TabOpts { New = NewState, 
                                       UndoBuffer = new List<string> { "" },
                                       BufferIndex = 0
                                     });
            TabPage NewTabPage = new TabPage(FileName);
            NewTabPage.Name = FileName;
            RichTextBox InputTxtBox = new RichTextBox();
            InputTxtBox.Location = new Point(1, 1);
            InputTxtBox.Size = new Size(801, 365);
            NewTabPage.Controls.Add(InputTxtBox);
            RichTextBox rtb = (RichTextBox)NewTabPage.Controls[0];
            rtb.Anchor = (AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom);
            rtb.Modified = false;
            rtb.TextChanged += new System.EventHandler(this.InputTxtBox_TextChanged);
            tabControl.TabPages.Add(NewTabPage);
            tabControl.SelectedTab = NewTabPage;
        }

        private void Close_tab()
        {
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            TabOpts temp = TabsOpts[tabControl.SelectedIndex];
            if (tb.Modified || temp.New)
            {
                DialogResult Res = MessageBox.Show(
                    $"Желаете сохранить файл {tabControl.TabPages[tabControl.SelectedIndex].Name}?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);

                if (Res == DialogResult.Yes)
                {
                    SaveFile(false);
                }
            };
            TabsOpts.RemoveAt(tabControl.SelectedIndex);
            tabControl.SelectedTab.Dispose();
        }
        private void Find_Regex()
        {
            statusStrip1.Items.Clear();
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            Regex regex = new Regex(@"[^\u0022\|\\/\*\?\<\>\n ]+\.(docx|doc|pdf)",
                                    RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(tb.Text);
            if (matches.Count > 0)
            {
                int SelIndex = tb.SelectionStart;
                tb.SelectAll();
                tb.SelectionBackColor = Color.White;
                OutputTxtBox.Clear();
                foreach (Match match in matches)
                {
                    tb.SelectionStart = match.Index;
                    tb.SelectionLength = match.Length;
                    tb.SelectionBackColor = Color.Yellow;
                    OutputTxtBox.Text += (match + "\n");
                }
                tb.SelectionStart = SelIndex;
                statusStrip1.Items.Add($"Отображено {matches.Count} совпадений");
            }
            else
            {
                statusStrip1.Items.Add("Совпадений не найдено");
            }
        }

        private void Find_Lexemes()
        {
            statusStrip1.Items.Clear();
            if (tabControl.SelectedIndex == -1)
            {
                statusStrip1.Items.Add("Нечего анализировать");
                return;
            };
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            if (tb.Text.Length == 0)
            {
                statusStrip1.Items.Add("Нечего анализировать");
                return;
            }
            List<Lexem> lexemes = Lexem_analyzer.Analyse(tb.Text);
            foreach (Lexem lexem in lexemes)
            {
                if (lexem.id != LexType.Undef)
                    OutputTxtBox.Text += ("(" + lexem.pos.Item1 + ", " + lexem.pos.Item2 + ") " + lexem.id + "  \"" + lexem.val + "\"\n");
                else
                    OutputTxtBox.Text += ("(" + lexem.pos.Item1 + ", " + lexem.pos.Item2 + ") Нераспознанная лексема  \"" + lexem.val + "\"\n");
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < tabControl.TabCount; i++)
            {
                tabControl.SelectedIndex = i;
                RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
                TabOpts temp = TabsOpts[tabControl.SelectedIndex];
                if (tb.Modified || temp.New)
                {
                    DialogResult Res = MessageBox.Show(
                        $"Желаете сохранить файл {tabControl.TabPages[i].Name}?",
                        "Подтверждение",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);

                    if (Res == DialogResult.Yes)
                    {
                        SaveFile(false);
                    }
                }
            }
        }

        #region WorkWithFiles

        private void SaveFile(bool As)
        {
            statusStrip1.Items.Clear();
            RichTextBox tb = (RichTextBox)(tabControl.SelectedTab.Controls[0]);
            TabOpts temp = TabsOpts[tabControl.SelectedIndex];
            if (As || temp.New)
            {
                if (saveFileD.ShowDialog() == DialogResult.Cancel)
                    return;

                tabControl.SelectedTab.Name = saveFileD.FileName;
                tabControl.SelectedTab.Text = saveFileD.FileName;
                
                using (StreamWriter sw = new StreamWriter(saveFileD.FileName, false))
                {
                    sw.Write(tb.Text);
                }
                statusStrip1.Items.Add($"Файл сохранен как {saveFileD.FileName}");              
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(tabControl.SelectedTab.Name, false))
                {
                    sw.Write(tb.Text);
                }
                statusStrip1.Items.Add($"Файл сохранен как {tabControl.SelectedTab.Name}");
            }

            tb.Modified = false;
            temp.New = false;
            TabsOpts[tabControl.SelectedIndex] = temp;
        }

        private void OpenFile()
        {
            statusStrip1.Items.Clear();
            if (openFileD.ShowDialog() != DialogResult.OK)
                return;

            if (tabControl.TabPages.ContainsKey(openFileD.FileName))
            {
                statusStrip1.Items.Add("Ошибка. Файл с таким именем уже открыт");
                return;
            }

            try
            {
                using (StreamReader sr = new StreamReader(openFileD.FileName))
                {
                    New_tab(openFileD.FileName);
                    tabControl.SelectedTab.Controls[0].Text = sr.ReadToEnd();
                    TabOpts temp = TabsOpts[tabControl.SelectedIndex];
                    temp.UndoBuffer.Clear();
                    temp.UndoBuffer.Add(tabControl.SelectedTab.Controls[0].Text);
                    temp.New = false;
                    temp.BufferIndex = 0;
                    TabsOpts[tabControl.SelectedIndex] = temp;
                    statusStrip1.Items.Add($"Открыт файл {openFileD.FileName}");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void NewFile()
        {
            statusStrip1.Items.Clear();
            int TabIndex = 1;
            while (tabControl.TabPages.ContainsKey("NewFile_" + TabIndex.ToString()))
                TabIndex++;
            statusStrip1.Items.Add($"Создан файл {"NewFile_" + TabIndex.ToString()}");

            New_tab("NewFile_" + TabIndex.ToString(), true);
        }

        #endregion

        #region MenuItemsHandlers

        private void Task_item_Click(object sender, EventArgs e)
        {
            Forms[0].ShowDialog();
        }

        private void диаграммаПереходовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms[1].ShowDialog();
        }

        private void таблицаКодовToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms[2].ShowDialog();
        }

        private void тестовыйПримерToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Forms[3].ShowDialog();
        }

        private void грамматикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms[4].ShowDialog();
        }

        private void языкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms[5].ShowDialog();
        }

        private void классификацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms[6].ShowDialog();
        }

        private void тестовыйПримерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms[7].ShowDialog();
        }

        private void грамматикаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms[8].ShowDialog();
        }

        private void классификацияГрамматикиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms[9].ShowDialog();
        }

        private void методАнализаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms[10].ShowDialog();
        }

        private void диагностикаИНейтрализацияОшибокToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms[11].ShowDialog();
        }

        private void тестовыйПримерToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms[12].ShowDialog();
        }


        private void языкToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms[13].ShowDialog();
        }

        private void Stop_item_Click(object sender, EventArgs e)
        {
            statusStrip1.Items.Clear();
            if (tabControl.SelectedIndex == -1 || DoRegex == false)
                return;

            DoRegex = false;
            statusStrip1.Items.Add($"Остановлен поиск совпадений");
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            int SelIndex = tb.SelectionStart;
            tb.SelectAll();
            SysModified = true;
            tb.SelectionBackColor = Color.White;
            OutputTxtBox.Clear();
            stop_item.Visible = false;
            tb.SelectionStart = SelIndex;
        }

        private void синтаксическийАнализаторFORTRANGOTOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
            {
                statusStrip1.Items.Add("Нечего анализировать");
                return;
            };
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            if (tb.Text.Length == 0)
            {
                statusStrip1.Items.Add("Нечего анализировать");
                return;
            }

            OutputTxtBox.Clear();
            (List<string>, List<List<string>>) Result = GA.Start(Lexem_analyzer.Analyse(tb.Text));
            for (int i = 0; i < Result.Item1.Count; i++)
            {
                if (Result.Item1[i] != "")
                    OutputTxtBox.Text += (Result.Item1[i] + '\n');
                foreach (string err in Result.Item2[i])
                    OutputTxtBox.Text += (err + '\n');
            }
                
        }

        private void лексическийАнализаторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutputTxtBox.Clear();
            Find_Lexemes();
        }

        private void рекурсивныйСпускАрифметическоеВыражениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
            {
                statusStrip1.Items.Add("Нечего анализировать");
                return;
            };
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            if (tb.Text.Length == 0)
            {
                statusStrip1.Items.Add("Нечего анализировать");
                return;
            }

            OutputTxtBox.Clear();        
            OutputTxtBox.Text = RD.Start(Lexem_analyzer.Analyse(tb.Text));
        }

        private void SaveFile_item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;
            SaveFile(false);
        }

        private void SaveFileAs_item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;
            SaveFile(true);
        }

        private void OpenFile_item_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void NewFile_item_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void CloseFile_item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;

            Close_tab();
        }

        private void Exit_item_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Undo_item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;

            TabOpts temp = TabsOpts[tabControl.SelectedIndex];
            if (temp.BufferIndex == 0) return;

            SysModified = true;
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];

            int CaretPos = tb.SelectionStart;
            temp.BufferIndex--;
            tb.Text = temp.UndoBuffer[temp.BufferIndex];
            if (CaretPos < tb.Text.Length)
                tb.SelectionStart = CaretPos;
            else
                tb.SelectionStart = tb.Text.Length;

            TabsOpts[tabControl.SelectedIndex] = temp;
        }

        private void Redo_item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;

            TabOpts temp = TabsOpts[tabControl.SelectedIndex];
            if (temp.BufferIndex == temp.UndoBuffer.Count - 1) return;

            SysModified = true;
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];

            int CaretPos = tb.SelectionStart;
            temp.BufferIndex++;
            tb.Text = temp.UndoBuffer[temp.BufferIndex];
            if (CaretPos < tb.Text.Length)
                tb.SelectionStart = CaretPos;
            else
                tb.SelectionStart = tb.Text.Length;

            TabsOpts[tabControl.SelectedIndex] = temp;
        }

        private void Cut_item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            tb.Cut();
        }

        private void Copy_item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            tb.Copy();
        }

        private void Paste_item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            tb.Paste();
        }

        private void Delete_item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            tb.Clear();
        }

        private void Highlight_item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;
            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            tb.SelectAll();
        }

        private void Help_Item_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "HELP.chm");
        }

        private void About_Item_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                    "Программа Redactor была создана в рамках лабораторных работ и курсового проекта " +
                    "по дисциплине 'Теория формальных языков и компиляторов'\n" +
                    "Выполнил: Лизунов Матвей Иванович, АВТ-913, 3 курс\n\n" +
                    "Новосибирский Государственный Технологический Университет",
                    "Информация о программе"
                );
        }

        private void InputTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (SysModified == true)
            {
                SysModified = false;
                return;
            }

            RichTextBox tb = (RichTextBox)tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
            TabOpts temp = TabsOpts[tabControl.SelectedIndex];

            if (temp.BufferIndex != temp.UndoBuffer.Count - 1)
                temp.UndoBuffer.RemoveRange(temp.BufferIndex + 1, temp.UndoBuffer.Count - 1 - temp.BufferIndex);

            temp.UndoBuffer.Add(tb.Text);
            temp.BufferIndex = temp.UndoBuffer.Count - 1;
            TabsOpts[tabControl.SelectedIndex] = temp;

            if (DoRegex)
                Find_Regex();
        }

        private void Regex_Item_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
                return;
            Find_Regex();
            DoRegex = true;
            stop_item.Visible = true;
        }

        #endregion
    }
}
