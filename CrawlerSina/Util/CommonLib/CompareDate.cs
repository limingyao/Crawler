using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class CompareDate
    {
        public static Boolean checkTime(string ctime)
        {
            DateTime startTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["startTime"]);
            DateTime endTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["endTime"]);
            DateTime tmpTime = DateTime.Parse(ctime);
            if (DateTime.Compare(startTime, tmpTime) <= 0 && DateTime.Compare(endTime, tmpTime) >= 0)
            {
                return true;
            }
            return false;
        }
        public static Boolean gEndTime(string ctime)
        {
            DateTime endTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["endTime"]);
            DateTime tmpTime = DateTime.Parse(ctime);
            if (DateTime.Compare(tmpTime, endTime) > 0)
            {
                return true;
            }
            return false;
        }
        public static Boolean gEEndTime(string ctime)
        {
            DateTime endTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["endTime"]);
            DateTime tmpTime = DateTime.Parse(ctime);
            if (DateTime.Compare(tmpTime, endTime) >= 0)
            {
                return true;
            }
            return false;
        }
        public static Boolean lStartTime(string ctime)
        {
            DateTime startTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["startTime"]);
            DateTime tmpTime = DateTime.Parse(ctime);
            if (DateTime.Compare(tmpTime, startTime) < 0)
            {
                return true;
            }
            return false;
        }
        public static Boolean lEStartTime(string ctime)
        {
            DateTime startTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["startTime"]);
            DateTime tmpTime = DateTime.Parse(ctime);
            if (DateTime.Compare(tmpTime, startTime) <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
