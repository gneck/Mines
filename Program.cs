using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mines
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void run2()
        {
            DB db = new DB();
            db.tah();
            db.hraciPlocha(1, 1, 1);
            db.vlozMina(1, 1);
        }
    }
}
