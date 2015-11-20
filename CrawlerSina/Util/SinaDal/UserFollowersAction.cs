using log4net;
using NetDimension.Weibo;
using NetDimension.Weibo.Entities.tag;
using SinaDal.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SinaDal
{
    public class UserFollowersAction
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

        public List<string> operation(Client client, NetDimension.Weibo.Entities.user.Entity user,int depth,string tagname)
        {
            List<string> list = new List<string>();
            string userID = user.ID;
            
            try
            {
                var SinaClient = client;
                var userCollection = SinaClient.API.Entity.Friendships.FollowersInActive(userID, 200);
                for (int i = 0; i < userCollection.Count(); i++)
                {
                    var followeruser = userCollection.ElementAt(i);
                    string sql = "select * FROM sinauser WHERE uid = '" + followeruser.ID + "'";
                    bool isexist = db_SQLServer.SQLServerUnit.getInstance().isExist(sql);
                    if (!isexist)
                    {
                        var usertags = SinaClient.API.Entity.Tags.Tags(followeruser.ID);
                        if (checkTags(usertags, tagname))
                        {
                            list.Add(followeruser.ID + " " + (depth + 1));
                            sql = "INSERT INTO sinauser(uid,statuses,depth) VALUES('" + followeruser.ID + "','" + followeruser.StatusesCount + "','" + (depth + 1) + "')";
                            db_SQLServer.SQLServerUnit.getInstance().insert(sql);
                        }
                    }

                }  
            }
            catch (WeiboException e)
            {
                log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 异常 {0} :\n[" + e.Message + "]");

            }
            catch (Exception e)
            {
                log.Info("线程: " + threadNo.ToString() + "-" + subThreadNo.ToString() + " 异常 {0} :\n[" + e.Message + "]");
            }
            return list;
        }

        public Boolean checkTags(IEnumerable<Tag> tags,string value)
        {
            for (int i = 0; i < tags.Count(); i++)
            {
                
                if (tags.ElementAt(i).Name.Contains(value))
                {
                    //log.Info(tags.ElementAt(i).Name);
                    return true;
                }
            }
            return false;
        }
    }

}
