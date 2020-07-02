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
        private void Form1_Load_1(object sender, EventArgs e)
        {
            restartbutton.Click += new System.EventHandler(this.restart_Click);
            backbutton.Click += new System.EventHandler(this.back_Click);
            solutionbutton.Click += new System.EventHandler(this.solution_Click);
            labelSteps1.Text = player_steps.ToString();
            labelPoints1.Text = sum_points.ToString();
            labelmax_points.Text = max_points.ToString();

            boardBox.Items.Add("8x8");
            boardBox.Items.Add("7x9");
            boardBox.Items.Add("7x7");
            boardBox.Items.Add("6x6");
            boardBox.Items.Add("5x5");
            boardBox.SelectedIndex = 0;

            max_stepsBox.Items.Add("4");
            max_stepsBox.Items.Add("6");
            max_stepsBox.Items.Add("8");
            max_stepsBox.Items.Add("10");
            max_stepsBox.Items.Add("12");
            max_stepsBox.Items.Add("14");
            max_stepsBox.Items.Add("16");
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

            max_points = 0;
            labelmax_points.Text = max_points.ToString();
            draw_board();
        }
    }
}
