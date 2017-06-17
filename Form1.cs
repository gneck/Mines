using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Mines
{
    public partial class Form1 : Form
    {
        public int time = 0;

        public Form1()
        {
            InitializeComponent();

            // konstanta určující maximální počet polí v řádku/sloupci
            const int MAX = 50;

            try
            {
                // zjištění velikosti minového pole
                String hrac = Interaction.InputBox("Zadejte jméno/přezdívku: ", "Hledání min - hráč", "");

                // zjištění velikosti minového pole
                String[] size = Interaction.InputBox("Velikost hrací plochy: (Max. " + MAX + " x " + MAX + ")", "Hledání min - hrací plocha", "10x10").Split('x');

                int x_length = int.Parse(size[0]) < MAX ? int.Parse(size[0]) : MAX;
                int y_length = int.Parse(size[1]) < MAX ? int.Parse(size[1]) : MAX;

                // zjištění počtu min
                int count_of_mines = int.Parse(Interaction.InputBox("Počet min: (menší než " + (x_length * y_length) + ")", "Hledání min - počet min", "5"));
                if (count_of_mines > (x_length * y_length)) count_of_mines = x_length * y_length;

                //zobrazeni aktualnich statistik
                statistics stat = new statistics();
                string statistika = stat.getStatistika(count_of_mines);
                Interaction.MsgBox("Statistika nejlepších hráčů pro tuto úroveň:\n\n 1.\tONdra\t34 vteřin\n ě.\tLucka\t51 vteřin\n 1.\tGneck\t63 vteřin");

                // vytvoření instance s patřičnými parametry
                Mines mines = new Mines(x_length, y_length, count_of_mines, this);

                this.Width = mines._xLength * 50;
                this.Height = mines._yLength * 50 + 10;
                mines.createField();
            }
            catch
            {
                Environment.Exit(-1);
            }

            this.timer1.Enabled = true;
        }

        //ssadas
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = "Hledání min " + ++this.time + "s"; 
        }

        private void nováHraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }        
}
