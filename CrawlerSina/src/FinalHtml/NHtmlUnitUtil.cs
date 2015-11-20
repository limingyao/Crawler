using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FinalHtml
{
    public class NHtmlUnitUtil
    {
        private string url = "";
        private int timeout = 30 * 1000;
        private string filename = "";
        private Dictionary<String, String> dic;

        private string htmlstring = "";
        public string HtmlString
        {
            get { return htmlstring; }
        }

        private Boolean success = false;
        public Boolean Success
        {
            get { return success; }
        }

        public NHtmlUnitUtil(string url, string filename, Dictionary<String, String> dic, int timeout)
        {
            this.url = url;
            this.filename = filename;
            this.dic = dic;
            this.timeout = timeout;
        }

        public NHtmlUnitUtil(string url, string filename, Dictionary<String, String> dic)
        {
            this.url = url;
            this.filename = filename;
            this.dic = dic;
        }

        public NHtmlUnitUtil(string url, string filename, int timeout)
        {
            this.url = url;
            this.filename = filename;
            this.dic = new Dictionary<string,string>();
            this.timeout = timeout;
        }

        public NHtmlUnitUtil(string url, string filename)
        {
            this.url = url;
            this.filename = filename;
            this.dic = new Dictionary<string,string>();
        }

        public void Run()
        {

            NHtmlUnit html = new NHtmlUnit();
            if (html.Run(url, dic, timeout))
            {
                htmlstring += "<title>" + html.HtmlTitle + "</title>\n";

                StringBuilder sb = new StringBuilder();
                sb.Append("<linkList>");
                foreach (String link in html.LinkList)
                {
                    sb.Append(link);
                }
                sb.Append("</linkList>");
                htmlstring += sb.ToString();

                sb = new StringBuilder();
                sb.Append("<imageList>");
                foreach (String img in html.ImageList)
                {
                    sb.Append(img);
                }
                sb.Append("</imageList>");
                htmlstring += sb.ToString();

                htmlstring += html.HtmlBody;

                success = true;
                //保存数据到文件
                StreamWriter writer = new StreamWriter(File.OpenWrite(filename));
                writer.WriteLine(htmlstring);
                writer.Close();
                Console.WriteLine("data save in " + filename);
            }
        }
    }
}
