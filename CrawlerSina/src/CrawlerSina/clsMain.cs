using HTTPUnit;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace CrawlerSina
{
    public class clsMain
    {
        //创建日志记录组件实例
        private ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Run()
        {
            string strThread = System.Configuration.ConfigurationManager.AppSettings["threadNum"];
            string strThreadNo = System.Configuration.ConfigurationManager.AppSettings["threadNo"];

            //线程总数
            int intThread = 0;
            //线程编号
            string[] threadNo = strThreadNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            #region " 多线程参数合法性检查 "
            if (!int.TryParse(strThread, out intThread))
            {
                CommonLib.Log.WriteLog("参数配置错误：thread不是有效的数字");
                return;
            }
            if (intThread > threadNo.Count())
            {
                CommonLib.Log.WriteLog("参数配置错误：总线程数量不能大于threadNo数量");
                return;
            }
            for (int i = 0; i < threadNo.Count(); i++)
            {
                int tmp = 0;
                if (!int.TryParse(threadNo[i],out tmp))
                {
                    CommonLib.Log.WriteLog("参数配置错误：threadNo必须是数字列表，以逗号隔开");
                    return;
                }
            }
            #endregion

            List<string> list = new List<string>();

            #region 从数据库初始化ID
            /*log.Info("从数据库初始化数据......");
            //string sql = "SELECT uid FROM sinauserid WHERE ischeck='0'";
            //top 500000 
            string sql = "SELECT uid FROM sinauser WHERE ischeck='-1'";
            //DataTable dt = db_MySQL.DBUnit.getInstance().query(sql);
            DataTable dt = db_SQLServer.SQLServerUnit.getInstance().query(sql);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                list.Add(dt.Rows[i][0].ToString());
            }
            log.Info("从数据库初始化数据结束......");*/
            
            /*log.Info("从数据库初始化数据......");
            string sql = "SELECT DISTINCT(uid) FROM sinauser WHERE needtocrawler50 = '1' AND iscrawler = '0'";
            DataTable dt = db_SQLServer.SQLServerUnit.getInstance().query(sql);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                list.Add(dt.Rows[i][0].ToString());
            }
            log.Info("从数据库初始化数据结束......");
            log.Info("抓取用户数为" + list.Count + "......");*/

            /*log.Info("从数据库初始化数据......");
            string sql = "SELECT uid FROM sinauser WHERE depth='0' AND needtocrawler='0';";
            DataTable dt = db_SQLServer.SQLServerUnit.getInstance().query(sql);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                list.Add(dt.Rows[i][0].ToString());
            }
            sql = "SELECT TOP 50000 uid FROM sinauser WHERE depth!=0 AND needtocrawler='0' ORDER BY statuses DESC;";
            dt = db_SQLServer.SQLServerUnit.getInstance().query(sql);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                list.Add(dt.Rows[i][0].ToString());
            }
            log.Info("从数据库初始化数据结束......");*/

            
            /*log.Info("从数据库初始化数据......");
            string sql = "SELECT uid FROM sinauser WHERE needtocrawler='0' ";
            DataTable dt = db_SQLServer.SQLServerUnit.getInstance().query(sql);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                list.Add(dt.Rows[i][0].ToString());
            }
            log.Info("从数据库初始化数据结束......");*/

            log.Info("从数据库初始化数据......");
            string sql = "SELECT uid FROM dbo.Users";
            DataTable dt = db_SQLServer.SQLServerUnit.getInstance().query(sql);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                list.Add(dt.Rows[i][0].ToString());
            }
            log.Info("从数据库初始化数据结束......");
            log.Info("抓取用户数为" + list.Count + "......");

            #endregion

            #region 根据人气榜获取用户昵称
            /*TopList tp = new TopList();
            list = tp.getTopHotUserList();
            //list.Add("信shin8989 jUyGKwA5sxyG5lRzNMEt8g==");*/
            #endregion

            #region 根据领域获取用户ID
            /*TopList tp = new TopList();
            //string[] types = { "kejiyenei", "ITchengxuyuan", "kejiqiyegaoguan", "kejiqitaqita" };
            string[] types = {"yule_wangluohongren","yanchuhuodong","yule_yulegaoguan","yuleqita"};
            foreach (string type in types)
            {
                list.AddRange(tp.getITTopUserList(0, type));
            }*/
            #endregion

            Thread[] thread = new Thread[intThread];
            Task[] task = new Task[intThread];

            for (int i = 0; i < intThread; i++)
            {
                task[i] = new Task();
                int tmp = 0;
                int.TryParse(threadNo[i], out tmp);
                task[i].ThreadNo = tmp;
                task[i].ThreadCount = intThread;
                task[i].UserList = list;
                thread[i] = new Thread(task[i].Run);
                thread[i].Start();
            }
            //判断子线程是否结束
            bool flag = true;
            while (flag)
            {
                for (int i = 0; i < intThread && flag; i++)
                {

                    if (thread[i].IsAlive)
                    {
                        flag = true; 
                    }
                    else
                    {
                        log.Info("线程"+i+"抓取结束，等待其它线程结束......");
                        flag = false;
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
