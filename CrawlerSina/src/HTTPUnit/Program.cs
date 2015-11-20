using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;


namespace HTTPUnit
{
    class Program
    {
        //创建日志记录组件实例
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            /*string content = "\u64CD\u4F5C\u6210\u529F";
            string result = Uri.UnescapeDataString(content);
            Console.WriteLine(result);//操作成功*/
            //

            TopList tp = new TopList();

            List<String> list = new List<string>();
            string[] types = { "kejiyenei", "ITchengxuyuan", "kejiqiyegaoguan", "kejiqitaqita" };
            //yule_wangluohongren
            //yanchuhuodong
            //yule_yulegaoguan
            //yuleqita
            foreach (string type in types)
            {
                list.AddRange(tp.getITTopUserList(0, type));
            }
            
            foreach (string str in list)
            {
                //log.Info(str);
            }
            Console.ReadLine();
            //string shtml = Util.getJsonPost("http://huati.weibo.com/aj_topic/list", "http://huati.weibo.com/883375?from=home_content_topic&type=ori&filter=mining", "_pv=1&keyword=马航飞机失联&topicName=马航飞机失联&ori=0&hasv=0&atten=0&match_area=0&mining=1&istag=2&is_olympic=0&_t=0&__rnd=1395296116000");
            /*string shtml = Util.getJsonGet("http://huati.weibo.com/aj_topic/list", "http://huati.weibo.com/883375?from=home_content_topic&type=ori&filter=mining", "_pv=1&keyword=马航飞机失联&topicName=马航飞机失联&ori=0&hasv=0&atten=0&match_area=0&mining=1&istag=2&is_olympic=0&_t=0&__rnd=1395296116000");
            JObject jo = JObject.Parse(shtml);
            JToken msg = jo["msg"];
            Console.WriteLine("MSG: " + msg.ToString());
            JToken code = jo["code"];
            //Console.WriteLine(code.ToString());
            JToken data = jo["data"];
            jo = JObject.Parse(data.ToString());
            JToken html = jo["html"];
            string htmlstring = html.ToString();
            Console.Write(htmlstring.Substring(0, 100));*/
        }
    }
}
