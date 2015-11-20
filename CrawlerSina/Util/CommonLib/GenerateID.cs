using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class GenerateID
    {
        private static Random ra = new Random();
        //获取随机id
        public static string getUid()
        {
            string uid = "000000000";
            int i1 = 0;
            int i2 = 0;
            try
            {
                i1 = ra.Next(0, 1000000000);//左开右闭
                i2 = ra.Next(1, 3);         //左开右闭
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }
            string uid1 = i2.ToString();
            string uid2 = i1.ToString();
            uid = uid1 + uid;
            int len = uid2.Length;
            uid = uid.Substring(0, 10 - len) + uid2;
            return uid;
        }
    }
}
