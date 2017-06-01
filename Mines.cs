using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Mines
{
    class Mines
    {
        public int _xLength;
        public int _yLength;
        public int _countOfMines;
        private Form1 _form;

        private MineButton[,] _buttons;
        private bool[,] _minesArray;

        Random rand = new Random();

        public Mines(int xLength, int yLength, int countOfMines, Form1 form)
        {
            _xLength = xLength;
            _yLength = yLength;
            _countOfMines = countOfMines;
            _form = form;
        }

        // určí souřadnice min
        private void generateMines()
        {
            _minesArray = new bool[_xLength, _yLength];
		    int i = 0;
		
		    while (i < _countOfMines)
		    {
                int x = rand.Next(_xLength);
                int y = rand.Next(_yLength);

				if (!_minesArray[x, y])
				{
					_minesArray[x, y] = true;
					i++;
				}				
		    }
        }

        // použití tablelayoutpanelu (obdobné jako Java Grid Layout) aplikace se nehorázně zasekává -> ukázka automatického resizování buňěk
        // imho je lepší použít klasický panel, práce s tablelayoutpanelem je sice jednodušší a nastaví si sám velikost jednotlivých buněk, avšak hrozně se to seká, nedoporučuju používat pro tyto účely (snažil sem se aby byl výsledek shodný ve všech jazycích)
        public void createField()
        {
            _form.field = new TableLayoutPanel();
            _form.field.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            _form.field.AutoSize = true;
            _form.field.Dock = DockStyle.Fill;
            _form.field.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            _form.field.ColumnCount = _xLength;
            _form.field.RowCount = _yLength;

            this.generateMines();
            _buttons = new MineButton[_xLength, _yLength];

            for (int y = 0; y < _yLength; y++)
            {
                // mění velikost řádku dle velikosti celé tabulky
                RowStyle rowstyle = new RowStyle(SizeType.Percent, (float)(100 / _xLength));
                _form.field.RowStyles.Add(rowstyle);

                for (int x = 0; x < _xLength; x++)
                {
                    // mení velikost sloupce v závislosti na celé tabulce
                    ColumnStyle colstyle = new ColumnStyle(SizeType.Percent, (float)(100 / _xLength));
                    _form.field.ColumnStyles.Add(colstyle);

                    _buttons[x, y] = new MineButton(x, y);
                    _buttons[x, y].MouseDown += new MouseEventHandler(Mines_MouseDown);

                    _form.field.Controls.Add(_buttons[x, y], x, y);
                }
            }

            _form.Controls.Add(_form.field);
        }

        // zobrazí celé minové pole (při prohře či výhře)
        public void showField()
        {
            for (int y = 0; y < _yLength; y++)
            {
                for (int x = 0; x < _xLength; x++)
                {
                    if (_minesArray[x, y])
                    {
                        _buttons[x, y].Text = "M";
                        _buttons[x, y].BackColor = Color.Red;
                    }
                    else
                    {
                        _buttons[x, y].Text = checkMinesAround(x, y).ToString();
                        _buttons[x, y].BackColor = Mines.getColor(checkMinesAround(x, y));
                    }
                    _buttons[x, y].Enabled = false;
                    _buttons[x, y].MouseDown -= new MouseEventHandler(Mines_MouseDown);
                }
            }
        }

        // barva podle počtu min
        public static Color getColor(int count_of_mines)
        {
            switch (count_of_mines)
            {
                case 0: return Color.White;
                case 1: return Color.SkyBlue;
                case 2: return Color.GreenYellow;
                default: return Color.Orange;
            }
        }

        // projede okolí stisknutého tlačítka a vrátí počet min kolem něj
        private int checkMinesAround(int x, int y)
        {
            int count = 0;


            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // ošetření přetečení
                    if (((x + i < 0) || (x + i >= _xLength)) || ((y + j < 0) || (y + j >= _yLength)) || ((x + i == x) && (y + j == y)))
                        continue;

                    if (_minesArray[x + i, y + j])
                        count++;
                }
            }
            return count;
        }

        // projede všechny tlačítka od stisknutého, které mají v okolí nulový počet min a zobrazí je
        public void showFree(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // ošetření přetečení
                    if (((x + i < 0) || (x + i >= _xLength)) || ((y + j < 0) || (y + j >= _yLength)))
                        continue;

                    if ((this.checkMinesAround(x + i, y + j) == 0) && (_buttons[x + i, y + j].Enabled == true) && (_buttons[x + i, y + j].Text != "?"))
                    {
                        _buttons[x, y].Text = "0";
                        _buttons[x, y].BackColor = Mines.getColor(0);
                        _buttons[x, y].Enabled = false;
                        this.showFree(x + i, y + j);
                    }
                }
            }
        }

        // pokud je počet aktivních tlačítek roven počtu min (zbývají pouze miny) -> vítězství
        public bool checkVictory()
        {
            int enabled_buttons = 0;
            foreach (Button but in _buttons)
            { 
                if (but.Enabled == true)
                    enabled_buttons++;
            }

            if (enabled_buttons == _countOfMines)
                return true;
            else
                return false;
        }

        // událost stisku tlačítka myši
        private void Mines_MouseDown(object sender, MouseEventArgs e)
        {
            // přetypování objektu který vyvolal tento event (tlačítko) na MineButton (přístup k souřadnícím)
            MineButton minebutton = (MineButton)sender;

            // levé tlačítko - lze vyvolat pokud je tlačítko aktivní a není označeno uživatelem za minové
            if ((e.Button == MouseButtons.Left) && (minebutton.Text != "?"))
            {
                minebutton.Enabled = false;

                // kontrola miny
                if (_minesArray[minebutton._x, minebutton._y])
                {
                    this.showField();
                    _form.timer1.Enabled = false;

                    if (MessageBox.Show("Prohrál jsi, na živu si vydržel po dobu: " + _form.time + "s!\nPak tě mina roztrhala na kusy.\n\nChceš to zkusit znovu?", "Prohra", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Application.Restart();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    minebutton.BackColor = Mines.getColor(this.checkMinesAround(minebutton._x, minebutton._y));
                    minebutton.Text = this.checkMinesAround(minebutton._x, minebutton._y).ToString();

                    // pokud má v okolí nulový počet min najde v jeho okolí další s touto vlastností
                    if (this.checkMinesAround(minebutton._x, minebutton._y) == 0)
                        this.showFree(minebutton._x, minebutton._y);

                    // zkontroluje počet aktivních tlačítek a vyhodnotí vítězství
                    if (this.checkVictory())
                    {
                        this.showField();
                        _form.timer1.Enabled = false;

                        if (MessageBox.Show("Vyhrál jsi v čase: " + _form.time + "s!\nMáš schopnosti superhrdiny.\n\nHrát znovu?", "Výhra", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            Application.Restart();
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)// pravé tlačítko -> označení tlačítka uživatelem za minové
            {
                if (minebutton.Text == "?")
                    minebutton.Text = "";
                else
                    minebutton.Text = "?";
            }
        }

    }
}
