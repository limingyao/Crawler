using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DianpingLib.Util
{
    public class Shop
    {
        public static string getShopURL(Element ele)
        {
            Elements subeles = ele.GetElementsByAttributeValue("class", "shopname");
            if (subeles.Count >= 1)
            {
                return "www.dianping.com" + subeles.Attr("href");
            }
            else
            {
                return "";
            }
        }
        public static string getShopName(Element ele)
        {
            Elements subeles = ele.GetElementsByAttributeValue("class", "big-name");
            return subeles.Text;
        }
        public static string getShopNickName(Element ele)
        {
            Elements subeles = ele.GetElementsByAttributeValue("class", "nick");
            return subeles.Text;
        }
        public static string getShopID(string shopURL)
        {
            int index = shopURL.LastIndexOf("/");
            if (index < 0)
            {
                return "";
            }
            return shopURL.Substring(index + 1);
        }
        public static bool isVShop(string html)
        {
            Document doc = NSoup.NSoupClient.Parse(html);
            Elements eles = doc.GetElementsByAttributeValue("class", "icon v-shop");
            if(eles!=null)
            {
                return true;
            }
            return false;
        }
        public static string getShopBriefInfo(string html)
        {
            StringBuilder sb = new StringBuilder();
            Document doc = NSoup.NSoupClient.Parse(html);
            Elements eles = doc.GetElementsByAttributeValue("class", "brief-info");
            doc = NSoup.NSoupClient.Parse(eles.Html());
            eles = doc.GetElementsByAttribute("title");
            sb.Append(eles.Attr("title"));
            eles = doc.GetElementsByAttributeValue("class", "item");
            int count = 5;
            foreach (Element ele in eles)
            {
                sb.Append("\t" + ele.Text());
                --count;
            }
            while (count > 0)
            {
                sb.Append("\t");
                --count;
            }
            return sb.ToString();
        }
        public static string getShopAddress(string html)
        {
            Document doc = NSoup.NSoupClient.Parse(html);
            Elements eles = doc.GetElementsByAttributeValue("class", "expand-info address");
            return eles.Text;
        }
        public static string getShopTel(string html)
        {
            Document doc = NSoup.NSoupClient.Parse(html);
            Elements eles = doc.GetElementsByAttributeValue("class", "expand-info tel");
            return eles.Text;
        }
        public static string getShopExpendInfo(string html)
        {
            StringBuilder sb = new StringBuilder();
            Document doc = NSoup.NSoupClient.Parse(html);
            Elements eles = doc.GetElementsByAttributeValue("class", "expand-info J-service nug-shop-ab-special_a");
            doc = NSoup.NSoupClient.Parse(eles.Html());
            eles = doc.GetElementsByTag("a");
            foreach (Element ele in eles)
            {
                sb.Append("," + ele.Attr("class"));
            }
            if (sb.Length >= 1)
            {
                return sb.ToString().Substring(1);
            }
            return sb.ToString();
        }    
        public static string getShopFeature(string html)
        {
            Document doc = NSoup.NSoupClient.Parse(html);
            Elements eles = doc.GetElementsByAttributeValue("class", "info J-feature Hide");
            return eles.Text;
        }
        public static string getShopMoreInfo(string html)
        {
            StringBuilder sbTag = new StringBuilder();
            StringBuilder sbIntroduction = new StringBuilder();
            Document doc = NSoup.NSoupClient.Parse(html);
            Elements eles = doc.GetElementsByAttributeValue("class", "info info-indent");
            foreach (Element ele in eles)
            {
                if (ele.Text().Contains("营业时间"))
                {
                    continue;
                }
                else if (ele.Text().Contains("分类标签"))
                {
                    Elements subeles = ele.GetElementsByAttributeValue("class", "item");
                    foreach (Element subele in subeles)
                    {
                        sbTag.Append("," + subele.Text());
                    }
                }
                else if (ele.Text().Contains("餐厅简介"))
                {
                    sbIntroduction.Append(ele.Text());
                }
            }
            if (sbTag.Length >= 1)
            {
                return sbTag.ToString().Substring(1) + "\t" + sbIntroduction.ToString();
            }
            return sbTag.ToString() + "\t" + sbIntroduction.ToString();
        }    
        public static string getShopCatalog(string html)
        {
            StringBuilder sb = new StringBuilder();
            Document doc = NSoup.NSoupClient.Parse(html);
            Elements eles = doc.GetElementsByAttributeValue("class", "breadcrumb");
            doc = NSoup.NSoupClient.Parse(eles.Html());
            eles = doc.GetElementsByTag("a");
            foreach(Element ele in eles)
            {
                sb.Append("," + ele.Text());
            }
            if(sb.Length>=1)
            {
                return sb.ToString().Substring(1);
            }
            return sb.ToString();
        }
        public static string getShopComment(string html)
        {
            Document doc = NSoup.NSoupClient.Parse(html);
            Elements eles = doc.GetElementsByAttributeValueStarting("id", "rev_");
            StringBuilder sb = new StringBuilder();
            foreach (Element ele in eles)
            {
                string commentid = ele.Attr("data-id");
                string userInfo = getUserInfo(NSoup.NSoupClient.Parse(ele.Html()).GetElementsByAttributeValue("class", "pic").Html());
                string commentInfo = getComment(NSoup.NSoupClient.Parse(ele.Html()).GetElementsByAttributeValue("class", "content").Html());
                sb.Append(commentid + "\t" + userInfo + "\t" + commentInfo + "\n");
            }
            return sb.ToString();
        }
        private static string getUserInfo(string html)
        {
            Document doc = NSoup.NSoupClient.Parse(html);
            string userID = doc.GetElementsByAttribute("user-id").Attr("user-id");
            string userName = doc.GetElementsByAttributeValue("class", "name").Text;
            string userRank = doc.GetElementsByAttributeValueMatching("class", "user-rank-rst").Attr("title");
            return userID + "\t" + userName + "\t" + userRank;
        }
        private static string getComment(string html)
        {
            StringBuilder sb = new StringBuilder();
            Document doc = NSoup.NSoupClient.Parse(html);
            string score = doc.GetElementsByAttributeValueMatching("class", "item-rank-rst").Attr("class");
            if (score.Contains("50"))
            {
                score = "5";
            }
            else if (score.Contains("40"))
            {
                score = "4";
            }
            else if (score.Contains("30"))
            {
                score = "3";
            }
            else if (score.Contains("20"))
            {
                score = "2";
            }
            else if (score.Contains("10"))
            {
                score = "1";
            }
            else
            {
                score = "0";
            }
            string comment_per = doc.GetElementsByAttributeValue("class", "comm-per").Text;
            comment_per = getNumber(reg, comment_per);

            Elements eles = doc.GetElementsByAttributeValue("class", "rst");
            int count = 3;
            foreach (Element ele in eles)
            {
                sb.Append("\t" + getNumber(reg, ele.Text()));
                --count;
            }
            while (count > 0)
            {
                --count;
                sb.Append("\t");
            }
            string comment_rst = sb.ToString().Length >= 1 ? sb.ToString().Substring(1) : sb.ToString();

            eles = doc.GetElementsByAttributeValue("class", "comment-type");
            string comment_type = eles.Text;

            eles = doc.GetElementsByAttributeValue("class", "J_brief-cont");
            string comment_content = eles.Text;
            comment_content = comment_content.Trim();

            eles = doc.GetElementsByAttributeValue("class", "time");
            string comment_time = paserDate(eles.Text);
            return score + "\t" + comment_per + "\t" + comment_rst + "\t" + comment_type + "\t" + comment_content + "\t" + comment_time;
        }
        private static string paserDate(string html)
        {
            DateTime dt = DateTime.Now;
            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            if (html.Contains(" "))
            {
                html = html.Substring(0, html.IndexOf(" "));
            }
            if(html.Length==5)
            {
                dtFormat.ShortDatePattern = "MM-dd";
                dt = Convert.ToDateTime(html, dtFormat);
            }
            else if (html.Length==8)
            {
                dtFormat.ShortDatePattern = "yy-MM-dd";
                dt = Convert.ToDateTime(html, dtFormat);
            }
            else
            {
                return "";
            }
            return dt.ToString("yyyy-MM-dd");
        }
        public static string getShopScore(string str)
        {
            switch (str)
            {
                case "五星商户": return "5";
                case "准五星商户": return "4.5";

                case "四星商户": return "4";
                case "准四星商户": return "3.5";

                case "三星商户": return "3";
                case "准三星商户": return "2.5";

                case "二星商户": return "2";
                case "准二星商户": return "1.5";

                case "一星商户": return "1";
                case "准一星商户": return "0.5";

                default: return "0";
            }
        }
        private static Regex reg = new Regex("[0-9\\.]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static string getShopPerCapita(string str)
        {
            return getNumber(reg, str);
        }
        public static string getShopTaste(string str)
        {
            return getNumber(reg, str);
        }
        public static string getShopAmbience(string str)
        {
            return getNumber(reg, str);
        }
        public static string getShopService(string str)
        {
            return getNumber(reg, str);
        }
        private static string getNumber(Regex key, string arg)
        {
            Match match = key.Match(arg);
            if (match.Success)
            {
                return match.Value;
            }
            return "0";
        }
        public static string getSpecial(string str)
        {
            StringBuilder sb = new StringBuilder();
            if (str.Contains("cu"))
            {
                sb.Append(",促");
            }
            if (str.Contains("tuan"))
            {
                sb.Append(",团");
            }
            if (str.Contains("ding"))
            {
                sb.Append(",订");
            }
            if (str.Contains("wai"))
            {
                sb.Append(",外");
            }
            if (str.Contains("ka"))
            {
                sb.Append(",卡");
            }
            return sb.Length > 0 ? sb.ToString().Substring(1) : sb.ToString();
        }
    }
}