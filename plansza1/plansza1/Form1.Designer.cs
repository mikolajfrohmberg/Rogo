using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace plansza1
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent(int nRows, int nColumns)
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.ColumnCount = nColumns;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = nRows;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddRows;
            this.Controls.Add(tableLayoutPanel1);
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;

            // label for name1
            this.labelName1 = new System.Windows.Forms.Label();
            this.labelName1.BackColor = System.Drawing.Color.Transparent;
            this.labelName1.Location = new System.Drawing.Point(950, 60);
            this.labelName1.Text = "Punkty";
            // label with Points
            // 
            this.labelPoints1 = new System.Windows.Forms.Label();
            this.labelPoints1.BackColor = System.Drawing.SystemColors.Window;
            this.labelPoints1.Location = new System.Drawing.Point(950, 100);
            this.labelPoints1.Width = 60;
            this.labelPoints1.Height = 60;
            this.labelPoints1.TextAlign = ContentAlignment.MiddleCenter;
            this.labelPoints1.Font = new Font("Arial", 20);

            // label for name2
            this.labelName2 = new System.Windows.Forms.Label();
            this.labelName2.BackColor = System.Drawing.Color.Transparent;
            this.labelName2.Location = new System.Drawing.Point(950, 170);
            this.labelName2.Text = "Kroki";
            // label with steps
            this.labelSteps1 = new System.Windows.Forms.Label();
            this.labelSteps1.BackColor = System.Drawing.SystemColors.Window;
            this.labelSteps1.Location = new System.Drawing.Point(950, 200);
            this.labelSteps1.Width = 60;
            this.labelSteps1.Height = 60;
            this.labelSteps1.TextAlign = ContentAlignment.MiddleCenter;
            this.labelSteps1.Font = new Font("Arial", 20);

            // label for name3
            this.labelName3 = new System.Windows.Forms.Label();
            this.labelName3.BackColor = System.Drawing.Color.Transparent;
            this.labelName3.Location = new System.Drawing.Point(1050, 170);
            this.labelName3.Text = "Cel";
            

            // label for name4
            this.labelName4 = new System.Windows.Forms.Label();
            this.labelName4.BackColor = System.Drawing.Color.Transparent;
            this.labelName4.Location = new System.Drawing.Point(1050, 60);
            this.labelName4.Text = "Najlepszy wynik";
            // label with Max_points
            // 
            this.labelmax_points = new System.Windows.Forms.Label();
            this.labelmax_points.BackColor = System.Drawing.SystemColors.Window;
            this.labelmax_points.Location = new System.Drawing.Point(1050, 100);
            this.labelmax_points.Width = 60;
            this.labelmax_points.Height = 60;
            this.labelmax_points.TextAlign = ContentAlignment.MiddleCenter;
            this.labelmax_points.Font = new Font("Arial", 20);

            //restart button
            this.restartbutton = new System.Windows.Forms.Button();
            this.restartbutton.Name = "button1";
            this.restartbutton.Size = new System.Drawing.Size(81, 74);
            this.restartbutton.TabIndex = 0;
            this.restartbutton.BackColor = Color.White;
            this.restartbutton.UseVisualStyleBackColor = true;
            string filename = "restart_icon.jpg";
            this.restartbutton.Location = new System.Drawing.Point(950, 500);
            this.restartbutton.BackgroundImage = Image.FromFile(filename);
            this.restartbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            //solution button
            this.solutionbutton = new System.Windows.Forms.Button();
            this.solutionbutton.Name = "solution_button";
            this.solutionbutton.Text = "rozwiązanie";
            this.solutionbutton.Size = new System.Drawing.Size(120, 74);
            this.solutionbutton.TabIndex = 0;
            this.solutionbutton.BackColor = Color.White;
            this.solutionbutton.UseVisualStyleBackColor = true;
            this.solutionbutton.Location = new System.Drawing.Point(950, 300);
            this.solutionbutton.Font = new Font("Arial", 10);

            //backbutton
            this.backbutton = new System.Windows.Forms.Button();
            this.backbutton.Name = "button2";
            this.backbutton.Size = new System.Drawing.Size(81, 74);
            this.backbutton.TabIndex = 0;
            this.backbutton.BackColor = Color.White;
            this.backbutton.UseVisualStyleBackColor = true;
            string filename2 = "back_icon.png";
            this.backbutton.Location = new System.Drawing.Point(950, 400);
            this.backbutton.BackgroundImage = Image.FromFile(filename2);
            this.backbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            //combobox with board selection
            this.boardBox = new System.Windows.Forms.ComboBox();
            this.boardBox.FormattingEnabled = true;
            this.boardBox.Location = new System.Drawing.Point(950, 650);
            this.boardBox.Name = "comboBox1";
            this.boardBox.Size = new System.Drawing.Size(100, 60);
            this.boardBox.Font = new Font("Arial", 12);
            this.boardBox.TabIndex = 1;
            this.boardBox.SelectedIndexChanged += new System.EventHandler(this.boardBox_SelectedIndexChanged);
            this.boardBox.DropDownStyle = ComboBoxStyle.DropDownList;
            // Combobox with max_steps
            this.max_stepsBox = new System.Windows.Forms.ComboBox();
            this.max_stepsBox.FormattingEnabled = true;
            this.max_stepsBox.Font = new Font("Arial", 16);
            this.max_stepsBox.Location = new System.Drawing.Point(1050, 200);
            this.max_stepsBox.Name = "comboBox1";
            this.max_stepsBox.Size = new System.Drawing.Size(80, 60);
            this.max_stepsBox.TabIndex = 1;
            this.max_stepsBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.max_stepsBox.SelectedIndexChanged += new System.EventHandler(this.max_stepsBox_SelectedIndexChanged);

            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this.tableLayoutPanel1);

            this.Controls.Add(this.labelName1);
            this.Controls.Add(this.labelName2);
            this.Controls.Add(this.labelName3);
            this.Controls.Add(this.labelName4);

            this.Controls.Add(this.labelmax_points);
            this.Controls.Add(this.labelPoints1);
            this.Controls.Add(this.labelSteps1);

            this.Controls.Add(this.boardBox);    
            this.Controls.Add(this.max_stepsBox);    


            this.Controls.Add(this.restartbutton);
            this.Controls.Add(this.backbutton);
            this.Controls.Add(this.solutionbutton);
            this.Text = "Points";

            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Label labelPoints1, labelSteps1, labelmax_points, labelName1, labelName2, labelName3, labelName4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Button restartbutton, backbutton, solutionbutton;
        private ComboBox boardBox, max_stepsBox;
    }
}

