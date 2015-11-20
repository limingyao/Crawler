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
            MySQLUnitParameter.getInstance().insertOrUpdataShopList("1", "3", "3", "3", "3", "3");
            
            Console.ReadLine();
        }
    }
}
