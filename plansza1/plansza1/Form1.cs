using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace plansza1
{

    public partial class Form1 : Form
    {
        List<List<int>> listArrays = new List<List<int>>();
        void Funkcja()
        {
            System.IO.StreamReader myFile =
              new System.IO.StreamReader("C:\\Users\\adudz\\Desktop\\6semestr\\SI\\kod\\tabela.txt");
            string myString = myFile.ReadToEnd();

            myFile.Close();

            char rc = (char)10;
            String[] listLines = myString.Split(rc);
            
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
        }

        public Form1()
        {
            InitializeComponent();
            Funkcja();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void tableLayoutPanel1_CellPaint_1(object sender, TableLayoutCellPaintEventArgs e)
        {
            if(listArrays[e.Row][e.Column] == -1)
                e.Graphics.FillRectangle(Brushes.Black, e.CellBounds);
            else
            {
                e.Graphics.FillRectangle(Brushes.White, e.CellBounds);
                if (listArrays[e.Row][e.Column] !=0)
                {
                    using (Font font1 = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel))
                    {
                        e.Graphics.DrawString(listArrays[e.Row][e.Column].ToString(), font1, Brushes.Black, e.CellBounds);
                    }
                }
            }
                
        }
    }
}
