using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class DataTranslate
    {
        /// <summary>
        /// 把新浪微博中的日期转换为日期类型
        /// </summary>
        /// <param name="SinaDate">新浪微博的日期</param>
        /// <returns></returns>
        public static DateTime SinaDateToDateTime(string sinaDate)
        {
            //"Thu Jun 27 13:19:21 +0800 2013"
            return DateTime.ParseExact(sinaDate, "ddd MMM d HH:mm:ss zzz yyyy", CultureInfo.CreateSpecificCulture("en-US"));
            //return Convert.ToDateTime(sinaDate.Substring(0, 10));
        }
        /// <summary>
        /// 把新浪微博中的日期转换为日期类型
        /// </summary>
        /// <param name="SinaDate">新浪微博的日期</param>
        /// <returns></returns>
        public static string SinaDateToString(string sinaDate)
        {
            //"Thu Jun 27 13:19:21 +0800 2013"
            return DateTime.ParseExact(sinaDate, "ddd MMM d HH:mm:ss zzz yyyy", CultureInfo.CreateSpecificCulture("en-US")).ToString("yyyy-MM-dd HH:mm:ss");
            //return Convert.ToDateTime(sinaDate.Substring(0, 10));
        }
    }
}
