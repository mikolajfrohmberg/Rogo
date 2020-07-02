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
        void file_changer(string fileName, int nRows, int nColumns, int max_steps, string problem_array)//Zmienia plik, ktory wywoluje Minizinc
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
        }

        void read_cmd_output(ref int sum_points, ref int[] x_array, ref int[] y_array, ref int[] points_array)
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


        List<List<int>> load_from_file(string fileName)
        {
            List<List<int>> list = new List<List<int>>();
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
    }
}
