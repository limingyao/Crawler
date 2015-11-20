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

namespace DianpingShopCrawler
{
    public partial class DianpingShopCrawler : Form
    {
        //保存抓取过的Shop ID
        private static Queue<string> queue = new Queue<string>();
        private static string current = "";
        private static int pageNo = 1;
        private static string shopID = "";
        //处理队列
        private static Queue<string> processingQueue = new Queue<string>();

        public DianpingShopCrawler()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            //初始化店铺数据
            DataTable dt = MySQLUnit.getInstance().query("SELECT shopid FROM shoplist WHERE flag='1' OR batchflag='1'");
            foreach (DataRow row in dt.Rows)
            {
                queue.Enqueue(row["shopid"].ToString());
            }
            webBrowser.Navigate("www.dianping.com/shop/" + queue.Peek());
            textBox.Text = "www.dianping.com/shop/" + queue.Peek();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString() != webBrowser.Url.ToString())
            {
                return;
            }
            if (webBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                return;
            }
            string url = webBrowser.Url.ToString();
            if (current.Equals(""))
            {
                //解析餐厅信息
                string html = webBrowser.Document.Body.InnerHtml;
                Document doc = NSoup.NSoupClient.Parse(html);
                Elements eles = doc.GetElementsByAttributeValue("class", "basic-info default nug_shop_ab_pv-a");
                shopID = url.Substring(url.LastIndexOf('/') + 1);
                string isV = Shop.isVShop(eles.Html())==true?"1":"0";

                string briefInfo = Shop.getShopBriefInfo(eles.Html());
                string[] infos = briefInfo.Split('\t');
                string shopscore = Shop.getShopScore(infos[0].Trim());
                string percapita = Shop.getShopPerCapita(infos[2].Trim());
                string taste = Shop.getShopTaste(infos[3].Trim());
                string ambience = Shop.getShopAmbience(infos[4].Trim());
                string service = Shop.getShopService(infos[5].Trim());

                string address = Shop.getShopAddress(eles.Html());
                string tel = Shop.getShopTel(eles.Html());
                string special = Shop.getSpecial(eles.Html()) + " = " + Shop.getShopExpendInfo(eles.Html());

                string feature = Shop.getShopFeature(eles.Html());//该字段(服务)通过点击，然后再通过JS加载，暂不抓取

                string more = Shop.getShopMoreInfo(eles.Html());
                string tag = more.Split('\t')[0];
                string introduction = more.Split('\t')[1];

                string catalog = Shop.getShopCatalog(html);//餐厅位置菜系类别
                
                eles = doc.GetElementsByAttributeValue("id", "sales");
                string favourable = "";//餐厅优惠信息暂时不抓

                //string output = shopID + "\t" + isV + "\t" + briefInfo + "\t" + address + "\t" + tel + "\t" + special + "\t" + feature + "\t" + more + "\t" + catalog + "\t" + favourable;
                //File.AppendAllText(shopPath, output + "\n");
                MySQLUnitParameter.getInstance().insertOrUpdataShopDetial(shopID, isV, shopscore, percapita, taste, ambience, service, address, tel, special, feature, tag, introduction, catalog, favourable);
                //MessageBox.Show(output);
                pageNo = 1;
                current = "www.dianping.com/shop/" + shopID + "/review_all?pageno=" + pageNo;
                webBrowser.Navigate(current);
            }
            else
            {
                //解析评论
                string html = webBrowser.Document.Body.InnerHtml;
                Document doc = NSoup.NSoupClient.Parse(html);
                Elements eles = doc.GetElementsByAttributeValue("class", "comment-list");
                string comments = Shop.getShopComment(eles.Html());
                if (comments == null || comments.Length == 0 || comments.Trim().Length == 0 || comments.Equals(""))
                {
                    current = "";
                    pageNo = 1;
                    queue.Dequeue();
                    MySQLUnit.getInstance().update("UPDATE shoplist SET flag='0' where shopid='" + shopID + "'");
                    if (queue.Count > 0)
                    {
                        webBrowser.Navigate("www.dianping.com/shop/" + queue.Peek());
                    }
                }
                else
                {
                    foreach (string line in comments.Split('\n'))
                    {
                        if (line.Trim().Length == 0)
                        {
                            continue;
                        }
                        string[] items = line.Split('\t');
                        MySQLUnitParameter.getInstance().insertOrUpdataUser(items[1], items[2], items[3]);
                        MySQLUnitParameter.getInstance().insertOrUpdataShopComment(items[0], shopID, items[1], items[4], items[5], items[6], items[7], items[8], items[9], items[10], items[11]);
                    }
                    //File.AppendAllText(commentPath, shopID + "\t" + comments + "\n");
                    //MessageBox.Show("|" + comments + "|");
                    ++pageNo;
                    current = url.Substring(0, url.LastIndexOf("=") + 1) + pageNo;
                    webBrowser.Navigate(current);
                }
            }
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            textBox.Text = webBrowser.Url.ToString();
        }
    }
}
