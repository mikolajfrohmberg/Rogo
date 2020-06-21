using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace plansza1
{
    public partial class Form1 : Form
    {
        int player_points = 0, player_steps = 0;
        int max_points = 0;
        int[] x_array = new int[] { };
        int[] y_array = new int[] { };
        int[] points_array = new int[] { };

        int sum_points = 0;
        int max_steps = 10;
        bool is_solution_displayed = false;
        List<List<int>> listArrays = new List<List<int>>();
        List<Tuple<int, int>> listClick = new List<Tuple<int, int>>(); // Przechowuje klikniete przez gracza pola w sesji [i,j]
        int nRows = 0;
        int nColumns = 0;
        string source_file = "7x9.txt"; // plik z planszą

        public void file_changer(string fileName, int nRows, int nColumns, int max_steps, string problem_array)//Zmienia plik, ktory wywoluje Minizinc
        {
            Regex regex1 = new Regex(@"(int: rows(=\d+)?;)");
            Regex regex2 = new Regex(@"(int: cols(=\d+)?;)");
            Regex regex3 = new Regex(@"(int: max_steps(=\d+)?;)");
            Regex regex4 = new Regex(@"(array\[1..rows, 1..cols\] of int: problem(=.+)?;)");

            string[] arrLine = File.ReadAllLines(fileName);
            int line_count = 0;
            foreach (string line in arrLine)
            {
                if (regex1.IsMatch(line))
                    arrLine[line_count] = "int: rows=" + nRows.ToString() + ";";
                if (regex2.IsMatch(line))
                    arrLine[line_count] = "int: cols=" + nColumns.ToString() + ";";
                if (regex3.IsMatch(line))
                    arrLine[line_count] = "int: max_steps=" + max_steps.ToString() + ";";
                if (regex4.IsMatch(line))
                    arrLine[line_count] = "array[1..rows, 1..cols] of int: problem=" + problem_array + ";";

                line_count++;
            }

            File.WriteAllLines(fileName, arrLine);
            Console.WriteLine("After file_changer");
        }


        public void cmd_exec()
        {
            //Console.WriteLine(nRows);
            int limit = nRows * nColumns * max_steps * 10;
            Console.WriteLine(limit);
            string solver = "--solver Chuffed";
            string strCmdText = "/K minizinc "+solver+" --time-limit "+limit+" rogo.mzn > output.txt";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = strCmdText;
            process.StartInfo = startInfo;
            process.Start();
            Thread.Sleep(limit + 500);
            Console.WriteLine("process_exited");
        }

        
        public void read_cmd_output(ref int sum_points, ref int[] x_array, ref int[] y_array, ref int[] points_array)
        {
            int[] x_array_1 = new int[] { };
            string fileName = "output.txt";
            string x_table_string = "", y_table_string = "", points_table_string = "", sum_points_string = "";
            string[] arrLine = File.ReadAllLines(fileName);
            Regex regex1 = new Regex(@"(^x.+(\])$)");
            Regex regex2 = new Regex(@"(^y.+(\])$)");
            Regex regex3 = new Regex(@"(^(points).+(\])$)");
            Regex regex4 = new Regex(@"(sum_points: \d+)");

            Regex r_table = new Regex(@"((\d(, )?)+)");
            Regex r_number = new Regex(@"(\d+)");
            foreach (string line in arrLine)
            {
                if (regex1.IsMatch(line))
                    x_table_string = line;
                if (regex2.IsMatch(line))
                    y_table_string = line;
                if (regex3.IsMatch(line))
                    points_table_string = line;
                if (regex4.IsMatch(line))
                {
                    sum_points_string = line;
                    Console.WriteLine(sum_points_string);
                }
                    
            }
            x_table_string = r_table.Match(x_table_string).Value;
            y_table_string = r_table.Match(y_table_string).Value;
            points_table_string = r_table.Match(points_table_string).Value;
            sum_points_string = r_number.Match(sum_points_string).Value;

            sum_points = Int32.Parse(sum_points_string);
            x_table_string = x_table_string.Replace(" ", "");
            y_table_string = y_table_string.Replace(" ", "");
            points_table_string = points_table_string.Replace(" ", "");

            x_array = x_table_string.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            y_array = y_table_string.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            points_array = points_table_string.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
        }
        void draw_board()
        {
            int startposition = 100;
            int endposition = 10;

            for (int i = 0; i < listArrays.Count; i++)
            {
                for (int j = 0; j < listArrays[0].Count; j++)
                {
                    Label l = addlabel(j, i, startposition, endposition, false);
                    tableLayoutPanel1.Controls.Add(l, j, i);
                    endposition += 100;
                    l.Click += new System.EventHandler(this.labelClick);
                }

            }
        }

        void clear_board()
        {
            for (int x = 0; x < listArrays.Count; x++)
            {
                for (int y = 0; y < listArrays[0].Count; y++)
                {
                    Control control = tableLayoutPanel1.GetControlFromPosition(y, x);
                    if (control != null)
                    {
                        if(control.BackColor!=Color.Black)
                        {
                            control.BackColor = Color.White;
                        }
                    }
                }

            }
            player_points = 0;
            player_steps = 0;
            labelPoints1.Text = "0";
            labelSteps1.Text = "0";
            listClick.Clear();

        }


        List<List<int>> load_from_file(string fileName)
        {
            List<List<int>> list = new List<List<int>> ();
            var fileContent = string.Empty; // Zawartość pliku

            try
            {   // Otwarcie pliku za pomoca StreamReadera
                using (StreamReader sr = new StreamReader(fileName))
                {
                    // Wczytanie zawartości pliku
                    fileContent = sr.ReadToEnd();
                }
            }
            catch (IOException e) // Nie można otworzyć pliku
            {
                MessageBox.Show("Bląd wczytywania pliku");
            }
                

            char rc = (char)10;
            // Linie w pliku
            String[] listLines = fileContent.Split(rc);

            if (listLines.Length == 0) // Jeżeli brak pliku/plik był pusty...
            {
                MessageBox.Show("Bląd wczytywania pliku");
            }


            for (int i = 0; i < listLines.Length; i++)
            {
                List<int> array = new List<int>();
                String[] listInts = listLines[i].Split(' ');
                for (int j = 0; j < listInts.Length; j++)
                {
                    if (listInts[j] != "\r")
                    {
                        array.Add(Convert.ToInt32(listInts[j]));
                    }
                }
                list.Add(array);
            }
            nRows = list.Count;
            nColumns = list[0].Count;
            return list;
        }

        string convert_list_arr_to_string(List<List<int>> list)
        {
            string problem_string = "[|";
            for(int i=0; i<list.Count; i++)
            {
                for(int j=0; j<list[0].Count; j++)
                {
                    problem_string += list[i][j] + ",";
                }
                problem_string += "|";
            }
            problem_string += "]";
            return problem_string;
        }
        //--------------------
        public Form1()
        {
            listArrays = load_from_file(source_file);

            InitializeComponent(nRows, nColumns);
            draw_board();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            restartbutton.Click += new System.EventHandler(this.restart_Click);
            backbutton.Click += new System.EventHandler(this.back_Click);
            solutionbutton.Click += new System.EventHandler(this.solution_Click);
            labelSteps1.Text = player_steps.ToString();
            labelPoints1.Text = sum_points.ToString();
            labelmax_points.Text = max_points.ToString();

            boardBox.Items.Add("10x10");
            boardBox.Items.Add("9x9");
            boardBox.Items.Add("8x8");
            boardBox.Items.Add("7x9");
            boardBox.Items.Add("7x7");
            boardBox.Items.Add("6x6");
            boardBox.Items.Add("5x5");
            boardBox.SelectedIndex = 3;

            max_stepsBox.Items.Add("4");
            max_stepsBox.Items.Add("6");
            max_stepsBox.Items.Add("8");
            max_stepsBox.Items.Add("10");
            max_stepsBox.Items.Add("12");
            max_stepsBox.Items.Add("14");
            max_stepsBox.Items.Add("16");
            max_stepsBox.Items.Add("18");
            max_stepsBox.Items.Add("20");
            max_stepsBox.SelectedIndex = 3;
        }

        void restart_Click(object sender, EventArgs e)
        {
            clear_board();
        }
        
        void back_Click(object sender, EventArgs e)
        {
            back_move();
        }

        void ComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        void solution_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            clear_board();
            List<List<int>> list = load_from_file(source_file);
            string problem_string = convert_list_arr_to_string(list);
            file_changer("rogo.mzn", nRows, nColumns, max_steps, problem_string);
            cmd_exec();
            read_cmd_output(ref sum_points, ref x_array, ref y_array, ref points_array);
            draw_path(x_array, y_array, points_array);
            is_solution_displayed = true;

            this.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        void back_move()
        {
            if(player_steps>0)
            {
                player_steps--;
                labelSteps1.Text = player_steps.ToString();

                //Console.WriteLine(listClick[listClick.Count-1].Item1 + " " + listClick[listClick.Count-1].Item2);
                Control control = tableLayoutPanel1.GetControlFromPosition(listClick[listClick.Count-1].Item2, listClick[listClick.Count-1].Item1);
                if (control != null)
                {
                    control.BackColor = Color.White;
                    player_points -= listArrays[listClick[listClick.Count - 1].Item1][listClick[listClick.Count - 1].Item2];
                }
                labelPoints1.Text = player_points.ToString();

                listClick.RemoveAt(listClick.Count-1);

            }
        }

        void labelClick(object sender, EventArgs e)
        {
            if (is_solution_displayed == true)
            {
                clear_board();
                is_solution_displayed = false;
            }
                
            Label currentlable = (Label)sender;
            Regex regex = new Regex(@"\d+");
            MatchCollection matches = regex.Matches(currentlable.Name);
            int j = int.Parse(matches[0].Value);
            int i = int.Parse(matches[1].Value);
            if (listClick == null)
                listClick.Add(Tuple.Create(j, i));

            int counter = 0, size = 0;
            var t = Tuple.Create(j, i);
            foreach (var item in listClick)
            {
                if (item.Item1.ToString() == t.Item1.ToString() && item.Item2.ToString() == t.Item2.ToString())
                    counter++;
                size++;
            }


            if (listArrays[j][i] == -1)
            {
                MessageBox.Show("Ruch niedozwolony! (-1)");
            }
            else
            {
                if (counter == 0)
                {
                    if (size > 0)
                    {
                        if (listClick.ElementAt(size - 1).Item1 != t.Item1 && listClick.ElementAt(size - 1).Item2 != t.Item2) //ruch na skos
                        {
                            MessageBox.Show("Ruch niedozwolony! (na skos)");
                        }
                        else
                        {
                            if ((listClick.ElementAt(size - 1).Item1 - t.Item1 > 1 || listClick.ElementAt(size - 1).Item1 - t.Item1 < -1) || (listClick.ElementAt(size - 1).Item2 - t.Item2 > 1 || listClick.ElementAt(size - 1).Item2 - t.Item2 < -1))// mozna sprobowac skrocic pozniej
                            {// warunek na nie przeskakiwanie o wiecej niz 1 pole w wysokosci lub w bok
                                MessageBox.Show("Ruch niedozwolony! (wiecej niz 1 pole)");
                            }
                            else
                            {
                                listClick.Add(Tuple.Create(j, i));
                                player_points += listArrays[j][i];
                                labelPoints1.Text = player_points.ToString();

                                player_steps++;
                                labelSteps1.Text = player_steps.ToString();

                                Console.WriteLine(listClick.ElementAt(size).Item1 + " "+ listClick.ElementAt(size).Item2);
                                if (player_steps == 1)
                                {
                                    currentlable.BackColor = Color.FromArgb(0, 255, 0);
                                }
                                else
                                {
                                    if(player_steps>max_steps)
                                        currentlable.BackColor = System.Drawing.Color.Red;
                                    else
                                    {
                                        if (player_steps == max_steps)
                                        {
                                            //pole koncowe poziomo z polem startowym
                                            bool valid_path = false;
                                            Console.WriteLine("list Click(0): "+ listClick.ElementAt(0).Item1+"|"+ listClick.ElementAt(0).Item2);
                                            Console.WriteLine("list Click(last): "+ listClick.ElementAt(size).Item1 + "|"+ listClick.ElementAt(size).Item2);
                                            if ((listClick.ElementAt(0).Item1 == listClick.ElementAt(size).Item1) && (listClick.ElementAt(0).Item2 == (listClick.ElementAt(size).Item2 - 1)))
                                                valid_path = true;
                                            if ((listClick.ElementAt(0).Item1 == listClick.ElementAt(size).Item1) && (listClick.ElementAt(0).Item2 == (listClick.ElementAt(size).Item2 + 1)))
                                                valid_path = true;

                                            if ((listClick.ElementAt(0).Item2 == listClick.ElementAt(size).Item2) && (listClick.ElementAt(0).Item1 == (listClick.ElementAt(size).Item1 - 1)))
                                                valid_path = true;
                                            if ((listClick.ElementAt(0).Item2 == listClick.ElementAt(size).Item2) && (listClick.ElementAt(0).Item1 == (listClick.ElementAt(size).Item1 + 1)))
                                                valid_path = true;

                                            currentlable.BackColor = Color.FromArgb(0, 255 - player_steps * 10, 0);
                                            if (player_points > max_points && valid_path)
                                                max_points = player_points;
                                            labelmax_points.Text = max_points.ToString();
                                        }
                                        else
                                        {
                                            currentlable.BackColor = Color.FromArgb(0, 255 - player_steps * 10, 0);
                                        }    
                                    }     
                                }
                            }
                        }
                    }
                    else
                    {
                        listClick.Add(Tuple.Create(j, i));
                        player_points += listArrays[j][i];
                        labelPoints1.Text = player_points.ToString();

                        player_steps++;
                        labelSteps1.Text = player_steps.ToString();
                        
                        currentlable.BackColor = System.Drawing.Color.LightGreen;
                    }
                }
                else
                    MessageBox.Show("Pole zostalo odwiedzone!");
            }

        }

        Label addlabel(int i, int j, int start, int end, bool path)
        {

            Label l = new Label();
            l.Name = "j:" + j.ToString() + "i:" + i.ToString();
            if(path)
            {
                l.BackColor = Color.Green;
            }
            else
            {
                if (listArrays[j][i] == -1)
                {//czarny
                    l.BackColor = Color.Black;
                }
                else
                {
                    l.BackColor = Color.White;
                    if (listArrays[j][i] != 0)
                    {
                        l.Text = listArrays[j][i].ToString();
                    }
                }
            }
            l.ForeColor = Color.Black;

            l.Font = new Font("Serif", 24, FontStyle.Bold);
            l.Width = 65;
            l.Height = 65;
            l.Location = new Point(start, end);
            l.TextAlign = ContentAlignment.MiddleCenter;
            l.Margin = new Padding(2);

            return l;
        }

        void max_stepsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            max_steps = Int32.Parse(max_stepsBox.SelectedItem.ToString());
            max_points = 0;
            labelmax_points.Text = max_points.ToString();
        }

        void boardBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string board_selected = boardBox.SelectedItem.ToString();
            int digit1=0, digit2=0;
            player_points = 0;
            labelPoints1.Text = player_points.ToString();
            if(board_selected[0] == '1')
            {
                digit1 = 10 + Int32.Parse(board_selected[1].ToString());
                if (board_selected[3] == '1')
                    digit2 = 10 + Int32.Parse(board_selected[4].ToString());
            }
            else
            {
                digit1 = Int32.Parse(board_selected[0].ToString());
                digit2 = Int32.Parse(board_selected[2].ToString());
            }
            clear_board();

            source_file = digit1.ToString()+"x"+digit2.ToString()+".txt";
            tableLayoutPanel1.RowCount = digit1;
            tableLayoutPanel1.ColumnCount = digit2;


            listArrays = load_from_file(source_file);
            while (tableLayoutPanel1.Controls.Count > 0)
            {
                tableLayoutPanel1.Controls[0].Dispose();
            } 
            draw_board();

        }

        void draw_path(int[] x_array, int[] y_array, int[] points_array)
        {//x_array i y_array to tablice przechowujące punkty (x,y) - mają ten sam rozmiar
            int startposition = 100;
            int endposition = 10;
            int counter=0;
            for (int i = 0; i < x_array.Length; i++)
            {
                for (int x = 0; x < listArrays.Count; x++)
                {
                    for (int y = 0; y < listArrays[0].Count; y++)
                    {
                        if ((x == x_array[i]-1) && (y == y_array[i]-1))
                        {//Console.WriteLine("x = " + x + " y = " + y + ", x_array[i] = " + x_array[i] + " y_array[j] = " + y_array[i]);
                            Control control = tableLayoutPanel1.GetControlFromPosition(y_array[i] - 1, x_array[i] - 1);
                            if (control != null)
                            {
                                //control.BackColor = Color.Green;
                                control.BackColor = Color.FromArgb(0, 255 - counter * 10, 0);
                                player_points += points_array[i];
                                counter++;
                            }
                        }

                    }

                }
                labelPoints1.Text = player_points.ToString();
                labelSteps1.Text = x_array.Length.ToString();
            }
        }

        private void Form1_Resize_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Width = Convert.ToInt32(this.Width - 20);
            flowLayoutPanel1.Height = Convert.ToInt32(this.Height - 50);
            // Poszerzanie kafelków
        }
    }
}
