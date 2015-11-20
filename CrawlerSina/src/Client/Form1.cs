using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        //用于提取信息
        private Regex hrefReg = new Regex("href[^ >]*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Regex titleReg = new Regex("title[^ >]*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Regex usercardReg = new Regex("usercard[^ >]*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Regex numberReg = new Regex("[0-9]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Regex timeReg = new Regex("[0-9]{2}:[0-9]{2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Regex dateReg = new Regex("[0-9]{1,2}月[0-9]{1,2}日\\s+[0-9]{2}:[0-9]{2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Topic topic = new Topic();
        private string getValue(Regex key, string arg)
        {
            Match match = key.Match(arg);
            if (match.Success)
            {
                string value = match.Value;
                value = value.Substring(value.IndexOf("=") + 1).Replace("\"", "");
                return value;
            }
            return "";
        }
        private string getNumber(Regex key, string arg)
        {
            Match match = key.Match(arg);
            if (match.Success)
            {
                return match.Value;
            }
            return "0";
        }
        private string getTime(string arg)
        {
            DateTime dt = DateTime.Now;
            if (arg.Contains("秒"))
            {
                string second = getNumber(numberReg, arg);
                return dt.AddSeconds(-Int32.Parse(second)).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (arg.Contains("分钟"))
            {
                string minute = getNumber(numberReg, arg);
                return dt.AddMinutes(-Int32.Parse(minute)).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (arg.Contains("今天"))
            {
                string hour = getNumber(timeReg, arg);
                return dt.ToString("yyyy-MM-dd ") + hour + ":00";
            }
            else if (arg.Contains("日") && arg.Contains("月"))
            {
                string monthday = getNumber(dateReg, arg);
                monthday = monthday.Replace("月", "-");
                monthday = monthday.Replace("日", "");
                return DateTime.Parse(dt.ToString("yyyy-") + monthday + ":00").ToString("yyyy-MM-dd HH:mm:ss");
            }
            return arg;
        }
        //用于自动抓取
        //string currentUrl = "http://huati.weibo.com/";
        string currentUrl = System.Configuration.ConfigurationManager.AppSettings["currentUrl"];
        Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();

        bool flag;

        public Form1()
        {
            InitializeComponent();
            this.webBrowser1.Navigate(currentUrl);
            queue.Clear();

            this.timer1.Interval = 5000;
            flag = true;      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int elementIndex = 0;
            for (elementIndex = 0; elementIndex < webBrowser1.Document.All.Count; elementIndex++)
            {
                if (webBrowser1.Document.All[elementIndex].TagName.ToLower().Equals("a") && webBrowser1.Document.All[elementIndex].OuterHtml.Contains("最新"))
                {
                    webBrowser1.Document.All[elementIndex].InvokeMember("click");
                    currentUrl = webBrowser1.Url.ToString();
                    break;
                }
            }        
        }

        private void button3_Click(object sender, EventArgs e)
        {

            int HeightMax = 0;
            int WidthMax = 0;
            HeightMax = webBrowser1.Document.Body.ScrollRectangle.Height - webBrowser1.ClientRectangle.Height;
            WidthMax = webBrowser1.Document.Body.ScrollRectangle.Width - webBrowser1.ClientRectangle.Width;
            //滚动到最底端
            webBrowser1.Document.Window.ScrollTo(WidthMax/2, HeightMax);

            int elementIndex = 0;
            for (elementIndex = 0; elementIndex < webBrowser1.Document.All.Count; elementIndex++)
            {
                if (webBrowser1.Document.All[elementIndex].TagName.ToLower().Equals("a") && webBrowser1.Document.All[elementIndex].OuterHtml.Contains("下一页"))
                {
                    //跳转之前处理该页面
                    Document doc = NSoup.NSoupClient.Parse(webBrowser1.Document.Body.InnerHtml);
                    Elements divs = doc.GetElementsByAttributeValue("id", "pl_content_topicFeed");
                    doc = NSoup.NSoupClient.Parse(divs.Html());
                    Elements lis = doc.GetElementsByAttributeValue("node-type", "list-item");
                    foreach (Element li in lis)
                    {
                        doc = NSoup.NSoupClient.Parse(li.GetElementsByAttributeValue("class", "content").OuterHtml());
                        //发微博者消息
                        Elements userinfo = doc.GetElementsByAttribute("usercard");
                        //微博内容
                        Elements context = doc.GetElementsByTag("em");
                        doc = NSoup.NSoupClient.Parse(li.GetElementsByAttributeValue("class", "con_opt clearfix").OuterHtml());
                        //其它内容
                        Elements hrefs = doc.GetElementsByAttribute("href");
                        
                        StringBuilder sb = new StringBuilder();
                        string username = userinfo.Text;
                        string userid = getValue(usercardReg, userinfo.OuterHtml());
                        if (userid.Contains("="))
                        {
                            userid = userid.Substring(userid.IndexOf("=") + 1);
                        }
                        string status = context.Text.Trim();
                        string time = getTime(hrefs[0].Text());
                        string praise = getNumber(numberReg, hrefs[2].Text());
                        string forwarding = getNumber(numberReg, hrefs[3].Text());
                        string comment = getNumber(numberReg, hrefs[4].Text());
                        sb.Append("{");
                        sb.Append("[" + username + "]");
                        sb.Append(",[" + userid + "]");
                        sb.Append(",[" + status + "]");
                        if(hrefs.Count==5)
                        {
                            sb.Append(",[" + time + "]");
                            sb.Append(",[" + praise + "]");
                            sb.Append(",[" + forwarding + "]");
                            sb.Append(",[" + comment + "]");
                        }
                        else
                        {
                            //抛出一个异常
                        }
                        sb.Append("}");
                        this.textBox1.AppendText(sb.ToString() + "\n");
                        Boolean ret = topic.query(userid, status, time.Substring(0, 11));
                        System.Diagnostics.Debug.WriteLine("查询:" + ret);
                        if (!ret)
                        {
                            ret = topic.insert(userid, username, status, time, praise, forwarding, comment);
                            //插入                            
                            System.Diagnostics.Debug.WriteLine("插入:" + ret);
                        }
                        else
                        {
                            //更新,赞、转发、评论
                            ret = topic.update(userid, username, status, time.Substring(0, 11), praise, forwarding, comment);
                            System.Diagnostics.Debug.WriteLine("更新:" + ret);
                        }
                    }
                    
                    webBrowser1.Document.All[elementIndex].InvokeMember("click");                    
                    currentUrl = webBrowser1.Url.ToString();                                

                    break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (flag)
            {
                this.button1.PerformClick();
                flag = false;
            }
            this.button3.PerformClick();
        }

        //抓取话题列表 
        /*private void button1_Click(object sender, EventArgs e)
        {    
                  
            bool flag = true;
            while (flag)
            {
                while (webBrowser1.Document == null)
                {
                    Thread.Sleep(10);
                }
                Document doc = null;
                Elements divs = null;
                while (divs == null || doc == null)
                {
                    Thread.Sleep(10);
                    if (webBrowser1.Document.Body != null && webBrowser1.Document.Body.InnerHtml != null)
                    {
                        doc = NSoup.NSoupClient.Parse(webBrowser1.Document.Body.InnerHtml);
                        if (doc != null)
                        {
                            divs = doc.GetElementsByAttributeValue("class", "interest_topicR");
                            break;
                        }

                    }
                }
                doc = NSoup.NSoupClient.Parse(divs.Html());
                Elements lis = doc.GetElementsByTag("li");
                foreach (Element li in lis)
                {
                    //获得了所有的话题<a></a>
                    string html = li.GetElementsByAttributeValue("class", "name").OuterHtml();
                    queue.Enqueue(new KeyValuePair<string, string>(getValue(titleReg, html), getValue(hrefReg, html)));
                }
                //输出日志
                foreach (KeyValuePair<string, string> kvp in queue)
                {
                    this.textBox1.AppendText(kvp.Key + "\n" + kvp.Value + "\n");
                }
                //处理日志
                while (queue.Count != 0)
                {
                    queue.Dequeue();
                }
                // 跳转到抓取下一个页面
                int elementIndex = 0;
                for (elementIndex = 0; elementIndex < webBrowser1.Document.All.Count; elementIndex++)
                {
                    if (webBrowser1.Document.All[elementIndex].TagName.ToLower().Equals("a") && webBrowser1.Document.All[elementIndex].OuterHtml.Contains("下一页"))
                    {
                        webBrowser1.Document.All[elementIndex].InvokeMember("click");
                        MessageBox.Show(webBrowser1.Url.ToString());
                        currentUrl = webBrowser1.Url.ToString();
                        break;
                    }
                }
                if (elementIndex == webBrowser1.Document.All.Count)
                {
                    flag = false;
                }
            }
         }*/
    }
}
