using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

namespace Mines
{
    class DB
    {
        public const String connstring = "Server=46.28.110.195;Port=4999;Database=aswi_miny;Uid=aswi_miny;Pwd=Ksi2348asdkm8732;";
        OdbcConnection con;

        public void openConnection()
        {
            con = new OdbcConnection(DB.connstring);
            con.Open();
        }

        public void closeConnection(OdbcConnection con)
        {
            con.Close();
        }

        public string select(string what, string from, string where, string other)
        {
            string result = "";

            openConnection();
            OdbcCommand cmd = new OdbcCommand("SELECT " + what + " FROM " + from + " WHERE " + where + " " + other);
            OdbcDataReader reader = cmd.ExecuteReader();
            
            while(reader.Read())
            {

            }

            closeConnection(con);

            return result;
        }
    }
}
