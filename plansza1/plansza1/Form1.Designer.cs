using System.Drawing;
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
            this.tableLayoutPanel1.ColumnCount = nRows;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = nColumns;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddRows;
            this.Controls.Add(tableLayoutPanel1);
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;

            // label for name1
            this.labelName1 = new System.Windows.Forms.Label();
            this.labelName1.BackColor = System.Drawing.SystemColors.Window;
            this.labelName1.Location = new System.Drawing.Point(600, 60);
            this.labelName1.Text = "Punkty";
            // label with Points
            // 
            this.labelPoints1 = new System.Windows.Forms.Label();
            this.labelPoints1.BackColor = System.Drawing.SystemColors.Window;
            this.labelPoints1.Location = new System.Drawing.Point(600, 100);
            this.labelPoints1.Width = 60;
            this.labelPoints1.Height = 60;
            this.labelPoints1.TextAlign = ContentAlignment.MiddleCenter;
            this.labelPoints1.Font = new Font("Arial", 20);

            // label for name2
            this.labelName2 = new System.Windows.Forms.Label();
            this.labelName2.BackColor = System.Drawing.SystemColors.Window;
            this.labelName2.Location = new System.Drawing.Point(600, 170);
            this.labelName2.Text = "Kroki";
            // label with steps
            this.labelSteps1 = new System.Windows.Forms.Label();
            this.labelSteps1.BackColor = System.Drawing.SystemColors.Window;
            this.labelSteps1.Location = new System.Drawing.Point(600, 200);
            this.labelSteps1.Width = 60;
            this.labelSteps1.Height = 60;
            this.labelSteps1.TextAlign = ContentAlignment.MiddleCenter;
            this.labelSteps1.Font = new Font("Arial", 20);
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 650);
            this.Controls.Add(this.tableLayoutPanel1);

            this.Controls.Add(this.labelName1);
            this.Controls.Add(this.labelName2);
            this.Controls.Add(this.labelPoints1);
            this.Controls.Add(this.labelSteps1);
            this.Text = "Points";

            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Label labelPoints1, labelSteps1, labelName1, labelName2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}

