#define ContinuousOperation

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDimension.Weibo;
using System.IO;
using NetDimension.Weibo.Entities.user;
using System.Data;
using System.Threading;
using log4net;
using System.Reflection;
using SinaDal;
using HTTPUnit;
using System.Collections;

namespace CrawlerSina
{
    public class Task
    {
        //创建日志记录组件实例
        private ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //微博操作类
        private Client client;   
        //线程编号，用于分配任务
        private int threadNo;    
        public int ThreadNo
        {
            get { return threadNo; }
            set { threadNo = value; }
        }
        //线程总数
        private int threadCount;  
        public int ThreadCount
        {
            get { return threadCount; }
            set { threadCount = value; }
        }

        private List<string> list = new List<string>();  //uid列表
        public List<string> UserList
        {
            get { return list; }
            set { list = value; }
        }

        /// <summary>
        /// 微博用户登录
        /// </summary>
        private void login()
        {
            string appKey = System.Configuration.ConfigurationManager.AppSettings["appKey"];
            string appSecret = System.Configuration.ConfigurationManager.AppSettings["appSecret"];
            string userName = System.Configuration.ConfigurationManager.AppSettings["userName"];
            string password = System.Configuration.ConfigurationManager.AppSettings["userPassword"];
            OAuth oauth = new OAuth(appKey, appSecret);
            oauth.ClientLogin(userName, password);
            client = new Client(oauth);
        }

        /// <summary>
        /// 任务入口
        /// </summary>
        public void Run()
        {
            //登录新浪微博
            login();
            log.Info("线程" + threadNo.ToString() + "：开始:" + DateTime.Now.ToString());
            //analyzeUidList();
            //analyzeUidRandom();
            //crawlerUserByScreenName();
            crawlerUserByUserID();
            //crawlerUserIDBreadthFirst();
            log.Info("线程" + threadNo.ToString() + "：结束:" + DateTime.Now.ToString());
        }

        /// <summary>
        /// 根据uid list 分析用户微博数
        /// </summary>
        /// <param name="Layer"></param>
        private void analyzeUidList()
        {

            string sql = "";
            bool flag = true;
            CheckID checkID = new CheckID();
            checkID.ThreadNo = threadNo;
            for (int i = 0; i < list.Count && flag; ++i)
            {
                if (i % threadCount == threadNo)
                {
                    checkID.SubThreadNo = i;
                    string uid = list[i];

                    #if ContinuousOperation
                    bool isRunOver = false;
                    #endif

                    var SinaClient = client;
                    #region 采用异步方式
                    SinaClient.AsyncInvoke<NetDimension.Weibo.Entities.user.Entity>(() =>
                    {
                        log.Info("线程" + threadNo.ToString() + "：开始:" + DateTime.Now.ToString() + "  抓取用户ID:" + uid);
                        return SinaClient.API.Entity.Users.Show(uid);
                    }, (callback) =>
                    {
                        if (callback.IsSuccess)
                        {
                            string userID = callback.Data.ID;
                            if (callback.Data.StatusesCount >= 50 && callback.Data.Status != null)
                            {
                                string statusID = callback.Data.Status.ID;
                                if (callback.Data.StatusesCount >= 50 && checkID.checkIDByStatuses(SinaClient, userID, statusID))
                                {
                                    sql = "UPDATE sinauser SET needtocrawler='1' WHERE uid = '" + uid + "'";
                                    db_SQLServer.SQLServerUnit.getInstance().insert(sql);
                                }
                                else
                                {
                                    sql = "UPDATE sinauser SET needtocrawler='-1' WHERE uid = '" + uid + "'";
                                    db_SQLServer.SQLServerUnit.getInstance().update(sql);
                                }
                            }
                            else
                            {
                                sql = "UPDATE sinauser SET needtocrawler='-1' WHERE uid = '" + uid + "'";
                                db_SQLServer.SQLServerUnit.getInstance().update(sql);
                            }
                            log.Info("线程" + threadNo.ToString() + "：开始:" + DateTime.Now.ToString() + "  更新用户ID:" + uid);
                        }
                        else
                        {
                            #region 报错处理
                            int ret = getErrorCode(callback);
                            switch (ret)
                            {
                                case 0:
                                    sql = "UPDATE sinauser SET needtocrawler='-1' WHERE uid = '" + uid + "'";
                                    db_SQLServer.SQLServerUnit.getInstance().update(sql);
                                    log.Info("用户 " + uid + "不存在    " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    break;
                                case 1:
                                    log.Info("频次超过上限......");
                                    Thread.Sleep(65 * 60 * 1000);
                                    break;
                                case 2:
                                    log.Info("IP请求超过上限......");
                                    Thread.Sleep(65 * 60 * 1000);
                                    break;
                                case 3: break;
                                case 4: break;
                                case 5: break;
                                case 6: break;
                                case 7: break;
                                case 8: break;
                                default:
                                    db_SQLServer.SQLServerUnit.getInstance().insertLog(threadNo.ToString(), uid, callback.Error.ToString(), callback.Error.Message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    log.Info("异常{0} :\n[" + uid + "]\n[" + callback.Error + "]\n[" + callback.Error.Message + "]");
                                    break;
                            }
                            #endregion
                        }

                        #if ContinuousOperation
                        isRunOver = true;
                        #endif

                    });
                    #endregion

                    #if ContinuousOperation
                    //判断异步回调是否结束
                    while (!isRunOver)
                    {
                        System.Threading.Thread.Sleep(200);
                    }
                    #endif

                }
            }
        }

        /// <summary>
        /// 根据随机uid 分析用户微博数
        /// </summary>
        /// <param name="Layer"></param>
        private void analyzeUidRandom()
        {
            bool flag = true;
            CheckID checkID = new CheckID();
            while (flag)
            {
                string uid =CommonLib.GenerateID.getUid();
                while (list.Contains(uid))
                {
                    uid = CommonLib.GenerateID.getUid();
                }
                list.Add(uid);
                string sql1 = "INSERT INTO sinauserid(`uid`,`ischeck`) VALUES('" + uid + "','-1') ";
                db_MySQL.MySQLUnit.getInstance().insert(sql1);
                log.Info(sql1);

                var SinaClient = client;
                #region 采用异步方式
                SinaClient.AsyncInvoke<NetDimension.Weibo.Entities.user.Entity>(() =>
                {
                    log.Info("线程" + threadNo.ToString() + "：开始:" + DateTime.Now.ToString() + "  抓取用户ID:" + uid);
                    return SinaClient.API.Entity.Users.Show(uid);
                }, (callback) =>
                {
                    if (callback.IsSuccess)
                    {
                        string userID = callback.Data.ID;
                        if (callback.Data.StatusesCount >= 50 && callback.Data.Status != null)
                        {
                            string statusID = callback.Data.Status.ID;
                            if (callback.Data.StatusesCount >= 50 && checkID.checkIDByStatuses(SinaClient, userID, statusID))
                            {
                                string sql2 = "INSERT INTO sinauserid50(`uid`,`statuses`) VALUES('" + userID + "','" + callback.Data.StatusesCount + "') ";
                                db_MySQL.MySQLUnit.getInstance().insert(sql2);
                                log.Info(sql2);
                            }
                        }
                        string sql3 = "UPDATE sinauserid SET ischeck='1' WHERE uid='" + uid + "'";
                        db_MySQL.MySQLUnit.getInstance().update(sql3);
                        log.Info(sql3);
                    }
                    else
                    {
                        int indexOfNotExist = callback.Error.Message.IndexOf("用户不存在");
                        int indexOfOutOfVisitTimes = callback.Error.Message.IndexOf("频次超过上限");
                        int indexOfNotInstantiation = callback.Error.Message.IndexOf("未将对象引用设置到对象的实例");
                        int indexOfOperateTimeOut = callback.Error.Message.IndexOf("操作超时");
                        int indexOfUnableToConnectToTheRemoteServer = callback.Error.Message.IndexOf("无法连接到远程服务器");

                        if (indexOfNotExist >= 0)
                        {
                            string sql4 = "UPDATE sinauserid SET ischeck='0' WHERE uid='" + uid + "'";
                            db_MySQL.MySQLUnit.getInstance().update(sql4);
                            log.Info(sql4);
                        }
                        else if (indexOfOutOfVisitTimes >= 0)
                        {
                            flag = false;
                        }
                        else if (indexOfNotInstantiation >= 0)
                        {
                        }
                        else if (indexOfOperateTimeOut >= 0)
                        {
                            flag = false;
                        }
                        else if (indexOfUnableToConnectToTheRemoteServer >= 0)
                        {
                            flag = false;
                        }
                        else
                        {
                            log.Info("异常{0} :\n[" + uid + "]\n[" + callback.Error + "]\n[" + callback.Error.Message + "]");
                            db_MySQL.MySQLUnitParameter.getInstance().insertLog(threadNo.ToString(), uid, callback.Error + "", callback.Error.Message, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        }
                    }
                });
                #endregion
            }
        }

        /// <summary>
        /// 根据用户 昵称list 抓取用户微博
        /// </summary>
        /// <param name="Layer"></param>
        private void crawlerUserByScreenName()
        {
            bool flag = true;
            TopList tp = new TopList();
            for (int i = 0; i < list.Count && flag; ++i)
            {
                if (i % threadCount == threadNo)
                {
                    string[] strs = list[i].Split(' ');
                    string screen_name = strs[0];
                    string uid = strs[1];
                    //根据用户ID获取用户昵称
                    string name = tp.getUserName(uid);
                    if (name != null)
                    {
                        screen_name = name;
                    }

                    #if ContinuousOperation
                    bool isRunOver = false;
                    #endif

                    var SinaClient = client;
                    #region 采用异步方式
                    SinaClient.AsyncInvoke<NetDimension.Weibo.Entities.user.Entity>(() =>
                    {
                        log.Info("线程" + threadNo.ToString() + "：开始:" + DateTime.Now.ToString() + "  分析用户:" + screen_name);
                        return SinaClient.API.Entity.Users.Show("", screen_name);
                    }, (callback) =>
                    {
                        if (callback.IsSuccess)
                        {
                            var user = callback.Data;
                            string userID = user.ID;

                            log.Info("线程" + threadNo.ToString() + "：开始:" + DateTime.Now.ToString() + "  抓取用户:" + screen_name);
                            SinaDal.UserAction action = new SinaDal.UserAction();
                            action.ThreadNo = threadNo;
                            action.SubThreadNo = i;
                            action.operation(SinaClient, user);
                        }
                        else
                        {                        
                            #region 报错处理                  
                            int ret = getErrorCode(callback);
                            switch (ret)
                            {
                                case 0:
                                    log.Info("用户 " + uid + "不存在    " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                    break;
                                case 1:
                                    log.Info("频次超过上限......");
                                    Thread.Sleep(65 * 60 * 1000);
                                    break;
                                case 2:
                                    log.Info("IP请求超过上限......");
                                    Thread.Sleep(65 * 60 * 1000);
                                    break;
                                case 3: break;
                                case 4: break;
                                case 5: break;
                                default:
                                    log.Info("异常{0} :\n[" + uid + "]\n[" + callback.Error + "]\n[" + callback.Error.Message + "]");
                                    break;
                            }
                            #endregion
                        }

                        #if ContinuousOperation
                        isRunOver = true;
                        #endif

                    });
                    #endregion

                    #if ContinuousOperation
                    //判断异步回调是否结束
                    while (!isRunOver)
                    {
                        System.Threading.Thread.Sleep(200);
                    }
                    #endif

                }
            }
        }

        /// <summary>
        /// 根据用户 id list 抓取用户微博
        /// </summary>
        /// <param name="Layer"></param>
        private void crawlerUserByUserID()
        {
            bool flag = true;
            for (int i = 0; i < list.Count && flag; ++i)
            {
                if (i % threadCount == threadNo)
                {

                    string uid = list[i];

#if ContinuousOperation
                    bool isRunOver = false;
#endif

                    var SinaClient = client;
                    #region 采用异步方式
                    SinaClient.AsyncInvoke<NetDimension.Weibo.Entities.user.Entity>(() =>
                    {
                        log.Info("线程" + threadNo.ToString() + "：开始:" + DateTime.Now.ToString() + "  分析用户:" + uid);
                        return SinaClient.API.Entity.Users.Show(uid);
                    }, (callback) =>
                    {
                        if (callback.IsSuccess)
                        {
                            var user = callback.Data;
                            string userID = user.ID;

                            log.Info("线程" + threadNo.ToString() + "：开始:" + DateTime.Now.ToString() + "  抓取用户:" + uid);                           

                            SinaDal.UserAction action = new SinaDal.UserAction();
                            
                            action.ThreadNo = threadNo;
                            action.SubThreadNo = i;
                            action.operation(SinaClient, user);

                        }
                        else
                        {
                            #region 报错处理
                            int ret = getErrorCode(callback);
                            switch (ret)
                            {
                                case 0:
                                    log.Info("用户 " + uid + "不存在    " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    break;
                                case 1:
                                    log.Info("频次超过上限......");
                                    Thread.Sleep(65 * 60 * 1000);
                                    break;
                                case 2:
                                    log.Info("IP请求超过上限......");
                                    Thread.Sleep(65 * 60 * 1000);
                                    break;
                                case 3: break;
                                case 4: break;
                                case 5: break;
                                case 6: break;
                                case 7: break;
                                case 8: break;
                                default:
                                    db_SQLServer.SQLServerUnit.getInstance().insertLog(threadNo.ToString(), uid, callback.Error.ToString(), callback.Error.Message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    log.Info("异常{0} :\n[" + uid + "]\n[" + callback.Error + "]\n[" + callback.Error.Message + "]");
                                    break;
                            }
                            #endregion
                        }

#if ContinuousOperation
                        isRunOver = true;
#endif

                    });
                    #endregion

#if ContinuousOperation
                    //判断异步回调是否结束
                    while (!isRunOver)
                    {
                        System.Threading.Thread.Sleep(200);
                    }
#endif

                }
            }
        }

        /// <summary>
        /// 根据用户 id list 通过广度优先抓取用户id
        /// </summary>
        /// <param name="Layer"></param>
        private void crawlerUserIDBreadthFirst()
        {
            if (threadNo != 0)
            {
                return;
            }

            bool flag = true;

            Queue<string> Q = new Queue<string>(); 
            foreach(string uid in list)
            {
                Q.Enqueue(uid+" 1");

                /*#region 保存用户
                string[] strs = uid.Split(' ');
                string uuid = strs[0];
                string sql = "select * FROM sinauser WHERE uid = '" + uuid + "'";
                bool isexist = db_SQLServer.SQLServerUnit.getInstance().isExist(sql);
                if (!isexist)
                {
                    sql = "INSERT INTO sinauser(uid,statuses,depth) VALUES('" + uuid + "','-1','" + 0 + "')";
                    db_SQLServer.SQLServerUnit.getInstance().insert(sql);
                }
                #endregion*/
            }

            while (Q.Count != 0 && flag)
            {
                log.Info(Q.Peek());
                string[] strs = Q.Dequeue().Split(' ');
                string uid = strs[0];
                int depth = Convert.ToInt32(strs[1]);

#if ContinuousOperation
                bool isRunOver = false;
#endif

                var SinaClient = client;
                #region 采用异步方式
                SinaClient.AsyncInvoke<NetDimension.Weibo.Entities.user.Entity>(() =>
                {
                    log.Info("线程" + threadNo.ToString() + "：开始:" + DateTime.Now.ToString() + "  分析用户:" + uid);
                    return SinaClient.API.Entity.Users.Show(uid);
                }, (callback) =>
                {
                    if (callback.IsSuccess)
                    {
                        var user = callback.Data;
                        string userID = user.ID;

                        log.Info("线程" + threadNo.ToString() + "：开始:" + DateTime.Now.ToString() + "  抓取用户:" + uid);

                        SinaDal.UserFollowersAction action = new SinaDal.UserFollowersAction();

                        action.ThreadNo = threadNo;
                        action.SubThreadNo = 0;
                        if (depth < 3)
                        {
                            List<string> slist = action.operation(SinaClient, user, depth,"娱乐");
                            foreach (string str in slist)
                            {
                                Q.Enqueue(str);
                            }
                        }
                    }
                    else
                    {
                        #region 报错处理
                        int indexOfNotExist = callback.Error.Message.IndexOf("用户不存在");
                        int indexOfOutOfVisitTimes = callback.Error.Message.IndexOf("频次超过上限");
                        int indexOfIPRequestOutOfVisitTimes = callback.Error.Message.IndexOf("IP请求超过上限");
                        int indexOfNotInstantiation = callback.Error.Message.IndexOf("未将对象引用设置到对象的实例");
                        int indexOfOperateTimeOut = callback.Error.Message.IndexOf("操作超时");
                        int indexOfUnableToConnectToTheRemoteServer = callback.Error.Message.IndexOf("无法连接到远程服务器");
                        int indexOfOperateTimeOutEnglish = callback.Error.Message.IndexOf("The operation has timed out");
                        int indexOfUnableToConnectToTheRemoteServerEnglish = callback.Error.Message.IndexOf("Unable to connect to the remote server");
                        int indexOfUnexpectedCharacterEncountered = callback.Error.Message.IndexOf("Unexpected character encountered while parsing value");

                        if (indexOfNotExist >= 0)
                        {
                            log.Info("用户 " + uid + "不存在    " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        }
                        else if (indexOfOutOfVisitTimes >= 0)
                        {
                            //flag = false;
                            log.Info("频次超过上限......");
                            Thread.Sleep(65 * 60 * 1000);
                        }
                        else if (indexOfIPRequestOutOfVisitTimes >= 0)
                        {
                            //flag = false;
                            log.Info("IP请求超过上限......");
                            Thread.Sleep(65 * 60 * 1000);
                        }
                        else if (indexOfNotInstantiation >= 0)
                        {
                        }
                        else if (indexOfOperateTimeOut >= 0)
                        {
                        }
                        else if (indexOfUnableToConnectToTheRemoteServer >= 0)
                        {
                        }
                        else
                        {
                            log.Info("异常{0} :\n[" + uid + "]\n[" + callback.Error + "]\n[" + callback.Error.Message + "]");
                        }
                        #endregion
                    }

#if ContinuousOperation
                    isRunOver = true;
#endif

                });
                #endregion

#if ContinuousOperation
                //判断异步回调是否结束
                while (!isRunOver)
                {
                    System.Threading.Thread.Sleep(200);
                }
#endif

            }
        }

        /// <summary>
        /// 根据callback返回异常编号
        /// 0 用户不存在
        /// 1 频次超过上限
        /// 2 IP请求超过上限
        /// 3 未将对象引用设置到对象的实例
        /// 4 操作超时
        /// 5 无法连接到远程服务器
        /// 6 The operation has timed out
        /// 7 Unable to connect to the remote server
        /// 8 Unexpected character encountered while parsing value
        /// -1 其它错误
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>
        /// </returns>
        private int getErrorCode(AsyncCallback<Entity> callback)
        {
            int indexOfNotExist = callback.Error.Message.IndexOf("用户不存在");
            int indexOfOutOfVisitTimes = callback.Error.Message.IndexOf("频次超过上限");
            int indexOfIPRequestOutOfVisitTimes = callback.Error.Message.IndexOf("IP请求超过上限");
            int indexOfNotInstantiation = callback.Error.Message.IndexOf("未将对象引用设置到对象的实例");
            int indexOfOperateTimeOut = callback.Error.Message.IndexOf("操作超时");
            int indexOfUnableToConnectToTheRemoteServer = callback.Error.Message.IndexOf("无法连接到远程服务器");

            int indexOfOperateTimeOutEnglish = callback.Error.Message.IndexOf("The operation has timed out");
            int indexOfUnableToConnectToTheRemoteServerEnglish = callback.Error.Message.IndexOf("Unable to connect to the remote server");
            int indexOfUnexpectedCharacterEncountered = callback.Error.Message.IndexOf("Unexpected character encountered while parsing value");

            if (indexOfNotExist >= 0)
            {
                return 0;
            }
            else if (indexOfOutOfVisitTimes >= 0)
            {
                return 1;
            }
            else if (indexOfIPRequestOutOfVisitTimes >= 0)
            {
                return 2;
            }
            else if (indexOfNotInstantiation >= 0)
            {
                return 3;
            }
            else if (indexOfOperateTimeOut >= 0)
            {
                return 4;
            }
            else if (indexOfUnableToConnectToTheRemoteServer >= 0)
            {
                return 5;
            }
            else if (indexOfOperateTimeOutEnglish >= 0)
            {
                return 6;
            }
            else if (indexOfUnableToConnectToTheRemoteServerEnglish >= 0)
            {
                return 7;
            }
            else if (indexOfUnexpectedCharacterEncountered >= 0)
            {
                return 8;
            }
            return -1;
        }
    }
}
