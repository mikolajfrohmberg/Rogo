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
namespace plansza1
{

    public partial class Form1 : Form
    {
        List<List<int>> listArrays = new List<List<int>>();
        int nRows = 0;
        int nColumns = 0;
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
        //--------------------
        public Form1()
        {
            if (Funkcja() == false)
            {
                //Application.Exit();
                return;
            } 
            InitializeComponent(nRows, nColumns);
            //Thread.Sleep(2000);

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            int startposition = 100;
            int endposition = 10;
            for(int i=0; i< listArrays.Count; i++)
            {
                for (int j = 0; j < listArrays[0].Count; j++)
                {
                    Label l = addlabel(i, j, startposition, endposition);
                    tableLayoutPanel1.Controls.Add(l, i, j);
                    endposition += 100;
                    l.Click += new System.EventHandler(this.labelClick);
                }
                
            }
        }

        void labelClick(object sender, EventArgs e)
        {
            Label currentlable = (Label)sender;
            MessageBox.Show(currentlable.Name);
        }

        Label addlabel(int i, int j, int start, int end)
        {
            Label l = new Label();
            l.Name = "label" + i.ToString();
            if (listArrays[i][j]==-1)
            {//czarny
                l.BackColor = Color.Black;
            }
            else
            {
                l.BackColor = Color.White;
                if (listArrays[i][j] !=0)
                {
                    l.Text = listArrays[i][j].ToString();
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
