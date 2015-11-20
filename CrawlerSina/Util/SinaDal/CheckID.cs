using log4net;
using NetDimension.Weibo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SinaDal
{
    public class CheckID
    {
        //创建日志记录组件实例
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int threadNo = -1;    //线程编号，用于分配任务
        public int ThreadNo
        {
            get { return threadNo; }
            set { threadNo = value; }
        }

        private int subThreadNo = -1; //线程编号，用于分配任务
        public int SubThreadNo
        {
            get { return subThreadNo; }
            set { subThreadNo = value; }
        }


        public Boolean checkIDByStatuses(Client client, string userID,string startStatusID, int sumNum = 50)
        {
            int statusNum = 0;
            try
            {
                string statusID = startStatusID;
                bool endFlag = false;
                int pageNum = 1;
                List<string> weibo_id_flag = new List<string>();
                while (!endFlag && statusNum < sumNum)
                {
                    var statusCollection = client.API.Entity.Statuses.UserTimeline(userID, "", "", statusID, 100, 1, false, 0, true);
                    log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 分析用户" + userID + "第" + (pageNum++) + "页微博,总" + statusCollection.Statuses.Count() + "条微博");
                    endFlag = true;
                    List<NetDimension.Weibo.Entities.status.Entity> statuslist = new List<NetDimension.Weibo.Entities.status.Entity>();
                    foreach (NetDimension.Weibo.Entities.status.Entity status in statusCollection.Statuses)
                    {
                        statusID = status.ID;
                        if (!weibo_id_flag.Contains(statusID))
                        {
                            weibo_id_flag.Add(statusID);
                            endFlag = false;
                            if (CommonLib.CompareDate.lStartTime(CommonLib.DataTranslate.SinaDateToString(status.CreatedAt)))
                            {
                                endFlag = true;
                                break;
                            }
                            statusNum++;
                        }
                    }
                }//while
                log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 分析用户" + userID + "结束......");
            }
            catch (WeiboException wex)
            {
                log.Info(wex.Message);
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
            }
            return statusNum >= sumNum ? true : false;
        }
    }
}