using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Threading;
using System.Text.RegularExpressions;

namespace plansza1
{

    public partial class Form1 : Form
    {
        int player_points = 0, player_steps = 0;
        List<List<int>> listArrays = new List<List<int>>();
        List<Tuple<int, int>> listClick = new List<Tuple<int, int>>(); // Przechowuje klikniete przez gracza pola w sesji [i,j]
        int nRows = 0;
        int nColumns = 0;

        public void file_changer(string fileName, int rows, int cols, int max_steps, string problem_array)//Zmienia plik, ktory wywoluje Minizinc
        {
            // Read the file and display it line by line.  
            Regex regex1 = new Regex(@"(int: rows(=\d+)?;)");
            Regex regex2 = new Regex(@"(int: cols(=\d+)?;)");
            Regex regex3 = new Regex(@"(int: max_steps(=\d+)?;)");
            Regex regex4 = new Regex(@"(array\[1..rows, 1..cols\] of int: problem(=.+)?;)");

            string[] arrLine = File.ReadAllLines(fileName);
            int line_count = 0;
            foreach (string line in arrLine)
            {
                if (regex1.IsMatch(line))
                    arrLine[line_count] = "int: rows=" + rows.ToString() + ";";
                if (regex2.IsMatch(line))
                    arrLine[line_count] = "int: cols=" + cols.ToString() + ";";
                if (regex3.IsMatch(line))
                    arrLine[line_count] = "int: max_steps=" + max_steps.ToString() + ";";
                if (regex4.IsMatch(line))
                    arrLine[line_count] = "array[1..rows, 1..cols] of int: problem=" + problem_array + ";";

                line_count++;
            }

            File.WriteAllLines(fileName, arrLine);

        }


        public void cmd_exec()
        {
            string strCmdText = "/K minizinc --time-limit 3000 rogo_test.mzn > output.txt";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = strCmdText;
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine("cmd exec()");
        }


        List<List<int>> set_listArray(int[] x_array, int[] y_array, int[] points_array)//moze zostac uzyta do zaznaczenia sciezki oraz planszy
        {
            int x_max = x_array.Max();  int y_max = y_array.Max();
            int[][] list_arr = new int[x_array.Max()][];
            for(int i=0; i<x_max; i++)
                list_arr[i] = new int[y_max];


            for(int i=0; i<x_array.Length; i++)
            {
                list_arr[x_array[i]-1][y_array[i]-1] = points_array[i];
                Console.WriteLine("x[" + x_array[i] + "]" + " y[" + y_array[i] + "]=" + " points[i]=" + list_arr[x_array[i]-1][y_array[i]-1]);
            }
            List<List<int>> lst = list_arr.Select(a => a.ToList()).ToList();
            return lst; 
        }


        void draw_path(int[] x_array, int[] y_array, ref TableLayoutPanel t)
        {//x_array i y_array to tablice przechowujące punkty (x,y) - mają ten sam rozmiar
            Console.WriteLine("draw_path()");
            int startposition = 100;
            int endposition = 10;
            string label_name = "";
            //tableLayoutPanel1.Controls.Clear();
            
            for (int i = 0; i < x_array.Length; i++)
            {
                label_name = "j:" + y_array[i] + "i:" + x_array[i];
                for (int x = 0; x < listArrays.Count; x++)
                {
                    for (int y = 0; y < listArrays[0].Count; y++)
                    {
                        if((x==x_array[i])&&(y==y_array[i]))
                        {//Console.WriteLine("x = " + x + " y = " + y + ", x_array[i] = " + x_array[i] + " y_array[j] = " + y_array[i]);
                            foreach (Control ctrl in tableLayoutPanel1.Controls)
                            {
                                string ctrlname = ctrl.Name;
                                Console.WriteLine(ctrlname);
                                if (ctrlname == label_name)
                                    ctrl.BackColor = Color.Green;
                            }
                        }
                            
                    }

                }
                
                    //Label l = (Label)tableLayoutPanel1.Controls[label_name];
                    //l.BackColor = Color.Green;
            }
            Console.WriteLine("draw_path()");
            //c.BackColor = System.Drawing.Color.Green;
            //currentlable.BackColor = 

        }
        //public void read_cmd_output(ref int sum_points, ref List<int> x_array)
        public void read_cmd_output(ref int sum_points, ref int[] x_array, ref int[] y_array, ref int[] points_array)
        {
            int[] x_array_1 = new int[] { };
            string fileName = "output.txt";
            string x_table_string = "", y_table_string = "", points_table_string = "", sum_points_string = "";
            string[] arrLine = File.ReadAllLines(fileName);
            int line_count = 0;
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
                    sum_points_string = line;
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

        bool Funkcja()
        {
            var fileContent = string.Empty; // Zawartość pliku
            var filePath = "plansza.txt"; // Ścieżka do pliku

            try
            {   // Otwarcie pliku za pomoca StreamReadera
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Wczytanie zawartości pliku
                    fileContent = sr.ReadToEnd();
                }
            }
            catch (IOException e) // Nie można otworzyć pliku
            {
                MessageBox.Show("Bląd wczytywania pliku");
                return false;
            }

            char rc = (char)10;
            // Linie w pliku
            String[] listLines = fileContent.Split(rc);

            if (listLines.Length == 0) // Jeżeli brak pliku/plik był pusty...
            {
                MessageBox.Show("Bląd wczytywania pliku");
                return false;
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
                listArrays.Add(array);
            }
            // Ustawienie zmiennych zawierających ilości kolumn i rzędów
            nRows = listArrays.Count;
            nColumns = listArrays[0].Count;

            // Założenie -> ilość kolumn i wierszy > 3 && < 9
            if (nColumns <= 3 || nColumns >= 9 || nRows <= 3 || nRows >= 9) // Jeżeli założenie nie zostało spełnione...
                return false;

            return true;
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
            if (Funkcja() == false)
            {
                //Application.Exit();
                return;
            }

            
            int sum_points = 0;
            int rows = 5;
            int cols = 6;
            int max_steps = 6;
            int[] x_array = new int[] { };
            int[] y_array = new int[] { };
            int[] points_array = new int[] { };

            //string problem_array = "[|5,0,0,5,0,0,4,|0,8,-1,0,0,-1,0,|0,0,2,0,0,8,0,|0,-1,0,4,0,0,0,|4,0,2,0,2,0,5,|0,0,0,5,0,-1,0,|0,8,0,0,8,0,0,|0,-1,0,0,-1,4,0,|5,0,0,4,0,0,2|]";
            List<List<int>> list = load_from_file("plansza.txt");
            string problem_string = convert_list_arr_to_string(list);

            file_changer("rogo_test.mzn", rows, cols, max_steps, problem_string);
            cmd_exec();
            read_cmd_output(ref sum_points, ref x_array, ref y_array, ref points_array);
            Console.WriteLine("Sum points: " + sum_points);
            //listArrays = set_listArray(x_array, y_array, points_array);

            InitializeComponent(nRows, nColumns);
            draw_path(x_array, y_array, ref tableLayoutPanel1);
            foreach(Control l in tableLayoutPanel1.Controls)
            {
                Console.WriteLine(l.ToString());
            }

            
            //Thread.Sleep(2000);

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            int startposition = 100;
            int endposition = 10;
            for (int i = 0; i < listArrays.Count; i++)
            {
                for (int j = 0; j < listArrays[0].Count; j++)
                {
                    Label l = addlabel(i, j, startposition, endposition, false);
                    tableLayoutPanel1.Controls.Add(l, i, j);
                    endposition += 100;
                    l.Click += new System.EventHandler(this.labelClick);
                }

            }
        }

        void labelClick(object sender, EventArgs e)
        {
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


            if (listArrays[i][j] == -1)
            {
                MessageBox.Show("Ruch niedozwolony!");
            }
            else
            {
                if (counter == 0)
                {
                    if (size > 0)
                    {
                        if (listClick.ElementAt(size - 1).Item1 != t.Item1 && listClick.ElementAt(size - 1).Item2 != t.Item2) //ruch na skos
                        {
                            MessageBox.Show("Ruch niedozwolony!");
                        }
                        else
                        {
                            if ((listClick.ElementAt(size - 1).Item1 - t.Item1 > 1 || listClick.ElementAt(size - 1).Item1 - t.Item1 < -1) || (listClick.ElementAt(size - 1).Item2 - t.Item2 > 1 || listClick.ElementAt(size - 1).Item2 - t.Item2 < -1))// mozna sprobowac skrocic pozniej
                            {// warunek na nie przeskakiwanie o wiecej niz 1 pole w wysokosci lub w bok
                                MessageBox.Show("Ruch niedozwolony!");
                                //Console.WriteLine("listClick: " + listClick.ElementAt(size - 1).Item1 + " " + listClick.ElementAt(size - 1).Item2);
                                //Console.WriteLine("t: " + t.Item1 + " " + t.Item2);
                            }
                            else
                            {
                                listClick.Add(Tuple.Create(j, i));
                                player_points += listArrays[i][j];
                                labelPoints1.Text = player_points.ToString();

                                player_steps++;
                                labelSteps1.Text = player_steps.ToString();
                                currentlable.BackColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                    else
                    {
                        listClick.Add(Tuple.Create(j, i));
                        player_points += listArrays[i][j];
                        labelPoints1.Text = player_points.ToString();

                        player_steps++;
                        labelSteps1.Text = player_steps.ToString();
                        currentlable.BackColor = System.Drawing.Color.Green;
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
                if (listArrays[i][j] == -1)
                {//czarny
                    l.BackColor = Color.Black;
                }
                else
                {
                    l.BackColor = Color.White;
                    if (listArrays[i][j] != 0)
                    {
                        l.Text = listArrays[i][j].ToString();
                    }
                }
            }
            l.ForeColor = Color.Black;

            l.Font = new Font("Serif", 24, FontStyle.Bold);
            l.Width = 80;
            l.Height = 80;
            l.Location = new Point(start, end);
            l.TextAlign = ContentAlignment.MiddleCenter;
            l.Margin = new Padding(2);

            return l;
        }

        private void Form1_Resize_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Width = Convert.ToInt32(this.Width - 20);
            flowLayoutPanel1.Height = Convert.ToInt32(this.Height - 50);
            // Poszerzanie kafelków
        }
    }
}
