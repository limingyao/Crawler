using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace FinalHtml
{
    /// <summary>
    /// 获取js执行之后的网页html标签body部分的代码
    /// </summary>
    public class NHtmlUnit
    {
        private String url;
        private bool success; // 是否成功运行
        private int timeOut;
        private Dictionary<String, String> dic;

        // 获得html title标签的内容
        private String htmlTitle;
        public String HtmlTitle
        {
            get
            {
                if (success == false) return null;
                return htmlTitle;
            }
        }

        /// <summary>
        /// 获得网页所有链接的链表， 一定要在Run之后进行
        /// </summary>
        private List<String> linkList;
        public List<String> LinkList
        {
            get
            {
                if (success == false) return null;
                return linkList;
            }
        }
        
        /// <summary>
        /// 获得所有图像的标签， 一定要在Run之后进行
        /// </summary>
        private List<String> imageList;
        public List<String> ImageList
        {
            get
            {
                if (success == false) return null;
                return imageList;
            }
        }
        
        /// <summary>
        /// 获得执行完js之后的网页body 部分的html代码
        /// </summary>
        private String htmlString;
        public String HtmlBody
        {
            get
            {
                if (success == false) return null;
                return htmlString;
            }
        }
        
        public NHtmlUnit()
        {
            linkList = new List<String>();
            imageList = new List<String>();
            htmlString = "";
            success = false;
        }

        /// <summary>
        /// 检查并补充设置url
        /// </summary>
        /// <param name="url"></param>
        private void CheckURL(String url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://") && !url.StartsWith("file:///"))
                url = "http://" + url;
            this.url = url;
        }
        
        /// <summary>
        /// 加载指定文件
        /// </summary>
        /// <param name="url">文件URL</param>
        /// <param name="timeOut">超时时限</param>
        /// <returns>是否成功运行，没有超时</returns>
        public bool Run(String url, Dictionary<String, String> dic, int timeOut = 30*1000)
        {
            CheckURL(url);
            this.dic = dic;
            this.timeOut = timeOut - 100;
            Thread newThread = new Thread(NewThread);
            // 为了创建WebBrowser类的实例 必须将对应线程设为单线程单元
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
            //监督子线程运行时间
            while (newThread.IsAlive && timeOut > 0)
            {
                Thread.Sleep(100);
                timeOut -= 100;
                if (success)
                {
                    newThread.Abort();
                    return true;
                }
            }
            // 超时处理
            if (newThread.IsAlive)
            {
                newThread.Abort();
                return false;
            }
            return true;
        }

        private void NewThread()
        {
            new FinalHtmlPerThread(this);
            // 循环等待webBrowser 加载完毕 调用 DocumentCompleted 事件
            Application.Run();
        }
        /// <summary>
        ///  用于处理一个url的核心类
        /// </summary>
        class FinalHtmlPerThread : IDisposable
        {
            NHtmlUnit master;
            WebBrowser web;
            int timeOut;
            Dictionary<String, String> dic;

            public FinalHtmlPerThread(NHtmlUnit master)
            {
                this.master = master;
                this.timeOut = master.timeOut;
                this.dic = master.dic;
                DealWithUrl();
            }
            private void DealWithUrl()
            {
                String url = master.url;
                web = new WebBrowser();
                web.ScriptErrorsSuppressed = true;
                bool success = false;
                try
                {
                    web.Url = new Uri(url);
                    web.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(web_DocumentCompleted); 
                    success = true;
                }
                finally
                {
                    if (!success)
                    {
                        Dispose();
                    }
                }
            }
            public void Dispose()
            {
                if (!web.IsDisposed)
                {
                    web.Dispose();
                }
            }
            private void ToList(HtmlElementCollection collection, List<String> list)
            {
                System.Collections.IEnumerator it = collection.GetEnumerator();
                while (it.MoveNext())
                {
                    HtmlElement htmlElement = (HtmlElement)it.Current;
                    list.Add(htmlElement.OuterHtml);
                }
            }
            private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                if (web.ReadyState != WebBrowserReadyState.Complete)
                {
                    return;
                }
                //跨线程调用,判断所需要的数据是否加载成功
                web.BeginInvoke(new System.EventHandler(UpdateData), null);
            }
            private void UpdateData(object o, System.EventArgs e)
            {
                try
                {
                    foreach (KeyValuePair<string, string> kvp in dic)
                    {
                        while (web.Document == null && web.Document.Body == null && web.Document.Body.InnerHtml==null && timeOut > 0)
                        {
                            Thread.Sleep(50);
                            timeOut -= 50;
                        }
                        if (web.Document != null)
                        {
                            Document doc = NSoup.NSoupClient.Parse(web.Document.Body.InnerHtml);
                            Elements divs = doc.GetElementsByAttributeValue(kvp.Key, kvp.Value);
                            while (divs == null && timeOut > 0)
                            {
                                doc = NSoup.NSoupClient.Parse(web.Document.Body.InnerHtml);
                                divs = doc.GetElementsByAttributeValue(kvp.Key, kvp.Value);
                                if (divs == null)
                                {
                                    Thread.Sleep(50);
                                    timeOut -= 50;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (dic.Count == 0)
                    {
                        Thread.Sleep(3000);
                    }
                    if (web.Document != null)
                    {
                        master.htmlTitle = web.Document.Title;
                        ToList(web.Document.Links, master.linkList);
                        ToList(web.Document.Images, master.imageList);
                        master.htmlString = web.Document.Body.InnerHtml;
                    }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    master.success = true;
                }
            }
        }
    }
}
