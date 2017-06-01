using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Mines
{
    // vlastní komponenta dědící z tlačítka obsahující navíc informaci o souřadnicích a přepsaný vzhled
    public partial class MineButton : Button
    {
        public int _x;
        public int _y;

        public MineButton(int x, int y)
        {
            _x = x;
            _y = y;

            base.FlatAppearance.BorderSize = 0;
            base.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            base.BackColor = Color.CadetBlue;

            base.FlatAppearance.MouseOverBackColor = Color.LightSkyBlue;
            base.FlatAppearance.MouseDownBackColor = Color.LightGray;

            base.Margin = Padding.Empty;
            base.Dock = DockStyle.Fill;
        }
    }
}
