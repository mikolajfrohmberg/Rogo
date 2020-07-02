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
        string source_file = ""; // plik z planszą

        


        void cmd_exec()
        {
            int limit = nRows * nColumns * max_steps * 10;
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
        }

        
        
        void draw_board()
        {
            int startposition = 100;
            int endposition = 10;

            for (int i = 0; i < listArrays.Count; i++)
            {
                for (int j = 0; j < listArrays[0].Count; j++)
                {
                    Label l = addlabel(j, i, startposition, endposition);
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

        Label addlabel(int i, int j, int start, int end)
        {
            Label l = new Label();
            l.Name = "j:" + j.ToString() + "i:" + i.ToString();
            if (listArrays[j][i] == -1)
                l.BackColor = Color.Black;
            else
            {
                l.BackColor = Color.White;
                if (listArrays[j][i] != 0)
                    l.Text = listArrays[j][i].ToString();
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
                        {
                            Control control = tableLayoutPanel1.GetControlFromPosition(y_array[i] - 1, x_array[i] - 1);
                            if (control != null)
                            {
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

        public Form1()
        {
            InitializeComponent(nRows, nColumns);
        }

    }
}
