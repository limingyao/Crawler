using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class StringUtil
    {
        public static string getProcessedString(string stext)
        {
            string text = stext;

            while (text.IndexOf("'") > 0)
                text = text.Replace("\'", "\"");

            return text;
        }
    }
}
