using db_MySQL;
using DianpingLib.Util;
using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DianpingCatalogCrawler
{
    public partial class Dianping : Form
    {
        //控制抓取的页数
        private static int pageNo = 0;

        public Dianping()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            //初始化店铺数据
            pageNo = 0;
            webBrowser.Navigate(textBox.Text);
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string html = webBrowser.Document.Body.InnerHtml;
            Document doc = NSoup.NSoupClient.Parse(html);
            Elements eles = doc.GetElementsByAttributeValue("class", "shop-list J_shop-list");
            doc = NSoup.NSoupClient.Parse(eles.Html());
            eles = doc.GetElementsByTag("li");
            //StringBuilder sb = new StringBuilder();
            foreach (Element ele in eles)
            {
                string url = Shop.getShopURL(ele);
                string id = Shop.getShopID(url);
                string name = Shop.getShopName(ele);
                string nickname = Shop.getShopNickName(ele);
                //sb.Append(id + "\t" + name + "\t" + nickname + "\t" + url + "\n");
                MySQLUnitParameter.getInstance().insertOrUpdataShopList(id, name, nickname, url, "1", "1");
            }
            //File.AppendAllText(path, sb.ToString());
            //MessageBox.Show(sb.ToString());
            ++pageNo;
            pageNo = pageNo % 50;
            if (pageNo == 0)
            {
                textBox.Text = "www.dianping.com/search/category/1/10";
                webBrowser.Navigate("www.dianping.com/search/category/1/10");
            }
            else
            {
                textBox.Text = "www.dianping.com/search/category/1/10/p" + (pageNo + 1);
                webBrowser.Navigate("www.dianping.com/search/category/1/10/p" + (pageNo + 1));
            }
        }
    }
}
