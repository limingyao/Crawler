using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace db_MySQL
{
    class Program
    {
        static void Main(string[] args)
        {
            /*DBUnit db = new DBUnit();
            DataTable table = db.query("select * from test");
            DataRowCollection rows = table.Rows;
            for (int i = 0; i <rows.Count; ++i)
            {
                int len = rows[i].ItemArray.Length;
                for (int j = 0; j < len; ++j)
                {
                    Console.Write(rows[i][j] + " ");
                }
                Console.WriteLine();
            }*/
            MySQLUnitParameter.getInstance().insertLog("1", "1234", "haha", "ddd", "1-2-3-4");
            Console.ReadLine();
        }
    }
}
