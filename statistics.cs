using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

namespace Mines
{
    class statistics
    {
        DB db = new DB();

        public string getStatistika(int uroven) {

            string vysledek = db.select("jmeno, cas", "statistka", "uroven=" + uroven, "LIMIT 3");

            return vysledek;
        }

        public static implicit operator statistics(string v)
        {
            throw new NotImplementedException();
        }
    }
}
