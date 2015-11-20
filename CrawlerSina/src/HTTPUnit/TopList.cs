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
    public class TopList
    {
        //创建日志记录组件实例
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<String> getTopHotUserList()
        {
            List<String> list = new List<string>();
            for (int i = 1; i <= 40; ++i)
            {
                try
                {
                    Console.WriteLine("抓取第" + i + "页信息......");
                    //string shtml = getHtmlPage(i);
                    string shtml = Util.getJsonPost("http://data.weibo.com/top/ajax/hot", "http://data.weibo.com/top/hot/all", "page=" + i + "&class=all&depart=all&_t=0");
                    JObject jo = JObject.Parse(shtml);
                    JToken msg = jo["msg"];
                    Console.WriteLine("MSG: " + msg.ToString());
                    JToken code = jo["code"];
                    //Console.WriteLine(code.ToString());
                    JToken data = jo["data"];
                    jo = JObject.Parse(data.ToString());
                    JToken html = jo["html"];
                    string htmlstring = html.ToString();
                    //Console.Write(htmlstring.Substring(0, 100));
                    Document doc = NSoup.NSoupClient.Parse(htmlstring);
                    Elements trs = doc.GetElementsByTag("tr");
                    foreach (Element element in trs)
                    {
                        Document sdoc = NSoup.NSoupClient.Parse(element.Html());
                        Elements spans = sdoc.GetElementsByAttributeValue("class", "zw_name");
                        string username = spans.Text.Trim();
                        sdoc = NSoup.NSoupClient.Parse(spans.Html());
                        Elements links = sdoc.GetElementsByTag("a");
                        if (username.Length != 0 && username != null && !username.Equals(""))
                        {
                            string uid = links.Attr("uid");
                            list.Add(username + " " + uid);
                        }
                    }
                }
                catch(Exception e)
                {
                    log.Info(e.Message);
                }
            }
            return list;
        }

        public string getUserName(string user_id)
        {
            try
            {
                string shtml = Util.getJsonPost("http://data.weibo.com/top/ajax/user", "http://data.weibo.com/top/hot/all", "user_id=" + Uri.EscapeDataString(user_id) + "&_t=0");
                JObject jo = JObject.Parse(shtml);

                JToken msg = jo["msg"];
                //Console.WriteLine("MSG: " + msg.ToString());

                JToken code = jo["code"];
                //Console.WriteLine(code.ToString());

                JToken data = jo["data"];
                jo = JObject.Parse(data.ToString());
                JToken html = jo["html"];
                string htmlstring = html.ToString();
                Document doc = NSoup.NSoupClient.Parse(htmlstring);
                Elements dts = doc.GetElementsByTag("dt");
                doc = NSoup.NSoupClient.Parse(dts.Html());
                Elements imgs = doc.GetElementsByTag("img");
                return imgs.Attr("title");
            }
            catch (Exception e)
            {
                log.Info(e.Message);
                return null;
            }
        }

        public List<String> getITTopUserList(int page,string type)
        {
            string shtml = Util.getJsonGet("http://verified.weibo.com/aj/getgrouplist","", "g_index=" + page + "&path=http://verified.weibo.com/fame/"+type+"/?srt=4&_t=0&__rnd=138569632396");

            JObject jo = JObject.Parse(shtml);

            JToken msg = jo["msg"];
            //Console.WriteLine("msg: " + msg.ToString());

            JToken code = jo["code"];
            //log.Info(type + "-" + page + "-code: " + code.ToString());

            JToken data = jo["data"];
            //Console.WriteLine(data.ToString());
            string htmlstring = data.ToString();

            List<String> list = new List<string>();
            Document doc = NSoup.NSoupClient.Parse(htmlstring);
            Elements divs = doc.GetElementsByAttributeValue("class", "categories_list");
            int maxIndex = 0;
            foreach (Element element in divs)
            {
                int index = 0;
                if (!int.TryParse(element.Attr("g-index"), out index))
                {
                    index = 0;
                }
                if (index > maxIndex)
                {
                    maxIndex = index;
                }
                if (index < 26)
                {
                    doc = NSoup.NSoupClient.Parse(element.Html());
                    Elements uls = doc.GetElementsByAttributeValue("class", "list clearfix");
                    doc = NSoup.NSoupClient.Parse(uls.Html());
                    Elements inputs = doc.GetElementsByTag("input");
                    foreach (Element input in inputs)
                    {
                        log.Info(input.Attr("value"));
                        list.Add(input.Attr("value")+" 0");
                    }
                }
            }
            if (maxIndex < 26)
            {
                list.AddRange(getITTopUserList(maxIndex,type));
            }
            return list;
        }

    }
}
