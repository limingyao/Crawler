using log4net;
using NetDimension.Weibo;
using SinaDal.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SinaDal
{
    public class UserAction
    {
        //创建日志记录组件实例
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private UserService userService = new UserService();
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

        public void operation(Client client, NetDimension.Weibo.Entities.user.Entity user)
        {
            string userID = user.ID;
            try
            {
                var SinaClient = client;
                
                /*
                //保存用户信息
                string sql = "select * FROM dbo.Users WHERE uid = '" + userID + "'";
                bool isexist = db_SQLServer.SQLServerUnit.getInstance().isExist(sql);
                if (!isexist)
                {
                    userService.insertUser(user);
                    log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 保存用户信息: " + userID);
                }

                //保存用户标签
                var usertags = SinaClient.API.Entity.Tags.Tags(user.ID);
                saveUserTags(user, usertags);
                log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 保存用户标签: " + userID);
                */

                string statusID = "-1";
                if (user.Status != null)
                {
                    statusID = user.Status.ID;
                }
                bool endFlag = false;
                int pageNum = 1;
                int statusNum = 0;
                List<string> weibo_id_flag = new List<string>();
                while (!endFlag && !statusID.Equals("-1"))
                {
                    var statusCollection = client.API.Entity.Statuses.UserTimeline(userID, "", "", statusID, 100, 1, false, 0, true);
                    log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 保存用户" + userID + "第" + (pageNum++) + "页微博,总" + statusCollection.Statuses.Count() + "条微博");
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
                                log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 用户" + userID + "抓取不在时间范围内，正在退出......");
                                break;
                            }
                            /*if (CommonLib.CompareDate.gEndTime(CommonLib.DataTranslate.SinaDateToString(status.CreatedAt)))
                            {
                                continue;
                            }*/
                            if (!saveUserBlog(user, status))
                            {
                                endFlag = true;
                                break;
                            }
                            statusNum++;
                            log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 保存用户" + userID + "微博,微博ID: " + statusID);
                        }
                    }
                }//while
                log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 用户" + userID + "微博抓取结束,总" + statusNum + "条微博......");

                //sql = "UPDATE sinauser SET iscrawler = '1' WHERE uid = '" + userID + "'";
                //db_SQLServer.SQLServerUnit.getInstance().update(sql);
            }
            catch (WeiboException e)
            {
                log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 异常 {0} :\n[" + e.Message + "]");
                
            }
            catch (Exception e)
            {
                log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 异常 {0} :\n[" + e.Message + "]");   
            }   
        }

        private Boolean saveUserBlog(NetDimension.Weibo.Entities.user.Entity user, NetDimension.Weibo.Entities.status.Entity blog_data)
        {
            //获取指定微博的信息
            string retweeted_status = "0";
            //处理微博
            string sText = blog_data.Text;
            //处理转发微博
            if (blog_data.RetweetedStatus != null)
            {
                retweeted_status = blog_data.RetweetedStatus.ID;
                sText = blog_data.RetweetedStatus.Text;
            }
            string sql = "select * from dbo.User_Blog_IDS where uid='" + user.ID + "' AND blogID='" + blog_data.ID + "'";
            bool isexist = db_SQLServer.SQLServerUnit.getInstance().isExist(sql);
            //不存在
            if (!isexist)
            {
                //写入用户微博总表
                userService.insertUserBlogIDS(user, blog_data, retweeted_status);
                //写入用户微博数据
                sql = "SELECT * FROM dbo.Blog WHERE blogid = '" + blog_data.ID + "'";
                isexist = db_SQLServer.SQLServerUnit.getInstance().isExist(sql);
                if (!isexist)
                {
                    userService.insertUserBlog(blog_data, sText);
                }
                return true;
            }
            return false;
        }

        public Boolean saveUserTags(NetDimension.Weibo.Entities.user.Entity user, IEnumerable<NetDimension.Weibo.Entities.tag.Tag> usertags)
        {
            for (int i = 0; i < usertags.Count(); i++)
            {
                string sql = "SELECT * FROM dbo.User_tags WHERE uid='"+user.ID+"' AND tagid='"+usertags.ElementAt(i).ID+"'";
                bool isexist = db_SQLServer.SQLServerUnit.getInstance().isExist(sql);
                if (!isexist)
                {
                    userService.insertUserTags(user.ID, usertags.ElementAt(i).ID, usertags.ElementAt(i).Name, Convert.ToInt32(usertags.ElementAt(i).Weight));
                }
            }
            return true;
        }
    }
}
