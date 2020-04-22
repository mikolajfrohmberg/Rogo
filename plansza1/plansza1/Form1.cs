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

        public Form1()
        {
            if (Funkcja() == false)
            {
                //Application.Exit();
                return;
            } 
            InitializeComponent(nRows, nColumns);
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
                    using (Font font1 = new Font("Times New Roman", 32, FontStyle.Bold, GraphicsUnit.Pixel))
                    {
                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;
                        e.Graphics.DrawString(listArrays[e.Row][e.Column].ToString(), font1, Brushes.Black, e.CellBounds, sf);
                    }
                }
            }
                
        }
    }
}
