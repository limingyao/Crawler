using Newtonsoft.Json.Linq;
using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HTTPUnit
{
    public class Util
    {
        public static string getJsonPost(string RequestUrl, string Referer, string FormData)
        {
            string result = "";
            CookieContainer MyCookieContainer = new CookieContainer();
            HttpWebRequest http = WebRequest.Create(RequestUrl) as HttpWebRequest;
            http.Referer = Referer;
            http.Method = "POST";
            http.ContentType = "application/x-www-form-urlencoded";
            http.AllowAutoRedirect = true;
            http.KeepAlive = true;
            http.CookieContainer = MyCookieContainer;
            string postBody = string.Format(FormData);
            byte[] postData = Encoding.Default.GetBytes(postBody);
            http.ContentLength = postData.Length;
            using (Stream request = http.GetRequestStream())
            {
                try
                {
                    request.Write(postData, 0, postData.Length);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    request.Close();
                }
            }
            try
            {
                using (HttpWebResponse response = http.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            try
                            {
                                result = reader.ReadToEnd();
                            }
                            catch { }
                            finally
                            {
                                reader.Close();
                            }
                        }
                    }
                    response.Close();
                }
            }
            catch { }
            return result;
        }

        public static string getJsonGet(string RequestUrl, string Referer, string postDataStr)
        {
            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(RequestUrl + (postDataStr == "" ? "" : "?") + postDataStr);
            CookieContainer MyCookieContainer = new CookieContainer();
            http.CookieContainer = MyCookieContainer;
            http.Method = "GET";
            http.ContentType = "application/x-www-form-urlencoded";
            http.Referer = Referer;
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
    }
}
