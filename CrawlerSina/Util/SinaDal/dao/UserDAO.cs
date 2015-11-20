using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using db_SQLServer;
using System.Data.SqlClient;
using log4net;
using System.Reflection;
using System.Data;

namespace SinaDal.service
{
    public class UserDAO
    {
        //创建日志记录组件实例
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Boolean insertUser(string uid, string idstr, string screen_name, string name, int province, int city, string location, string description,
        string url, string profile_image_url, string profile_url, string domain, string weihao, string gender, int followers_count, int friends_count, int statuses_count, int favourites_count,
        string created_at, int following, int allow_all_act_msg, int geo_enabled, int verified, int verified_type, string remark, int allow_all_comment,
        string avatar_large, string verified_reason, int follow_me, int online_status, int bi_followers_count, string lang)
        {
            string sql = "insert into Users(uid,idstr,screen_name,name,province,city,location,description" +
                ",url,profile_image_url,profile_url,domain,weihao,gender,followers_count,friends_count,statuses_count,favourites_count," +
                "created_at,following,allow_all_act_msg,geo_enabled,verified,verified_type,remark,allow_all_comment," +
                "avatar_large,verified_reason,follow_me,online_status,bi_followers_count,lang) values(" + uid + ",'" + idstr + "','" + screen_name + "','" + name + "'," + province + "," + city + ",'" + location + "','" + description +
                "','" + url + "','" + profile_image_url + "','" + profile_url + "','" + domain + "','" + weihao + "','" + gender + "'," + followers_count + "," + friends_count + "," + statuses_count + "," + favourites_count + ",'" +
                created_at + "'," + following + "," + allow_all_act_msg + "," + geo_enabled + "," + verified + "," + verified_type + ",'" + remark + "'," + allow_all_comment + ",'" +
                avatar_large + "','" + verified_reason + "'," + follow_me + "," + online_status + "," + bi_followers_count + ",'" + lang + "')";
            return SQLServerUnit.getInstance().insert(sql);
        }

        public Boolean insertUserTags(string uid, string tagid, string name, int weight)
        {

            string sql = "insert into User_tags(uid,tagid,name,weight) values('" + uid + "','" + tagid + "','" + name + "'," + weight + ")";
            return SQLServerUnit.getInstance().insert(sql);
        }

        public Boolean insertUserBlogIDS(string uid, string blogID, string retweeted_status)
        {

            string sql = "insert into User_Blog_IDS(uid,blogID,retweeted_status) values('" + uid + "','" + blogID + "','" + retweeted_status + "')";
            return SQLServerUnit.getInstance().insert(sql);
        }

        public Boolean insertUserBlog(string created_at, string blogid, string blogmid, string idstr, string text, string source, int favorited,
            int truncated, string in_reply_to_status_id, string in_reply_to_user_id, string in_reply_to_screen_name, string thumbnail_pic, int reposts_count, int comments_count)
        {
            string sql = "insert into Blog(created_at,blogid,blogmid,idstr,text,source,favorited,truncated,in_reply_to_status_id,in_reply_to_user_id," +
               "in_reply_to_screen_name,thumbnail_pic,reposts_count,comments_count) values('" + created_at + "','" + blogid + "','" + blogmid + "','" +
               idstr + "','" + text + "','" + source + "'," + favorited + "," + truncated + ",'" + in_reply_to_status_id + "','" + in_reply_to_user_id + "','" + in_reply_to_screen_name + "','" +
               thumbnail_pic + "'," + reposts_count + "," + comments_count + ")";
            return SQLServerUnit.getInstance().insert(sql);
        }

        public Boolean insertUserBlogParameter(string created_at, string blogid, string blogmid, string idstr, string text, string source, int favorited,
            int truncated, string in_reply_to_status_id, string in_reply_to_user_id, string in_reply_to_screen_name, string thumbnail_pic, int reposts_count, int comments_count)
        {
            string sql = "insert into Blog(created_at,blogid,blogmid,idstr,text,source,favorited,truncated,in_reply_to_status_id,in_reply_to_user_id,in_reply_to_screen_name,thumbnail_pic,reposts_count,comments_count) " +
                         "values(@created_at ,@blogid ,@blogmid ,@idstr ,@text ,@source ,@favorited ,@truncated ,@in_reply_to_status_id ,@in_reply_to_user_id ,@in_reply_to_screen_name ,@thumbnail_pic ,@reposts_count ,@comments_count )";
            SqlConnection conn = SQLServerUnit.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@created_at", created_at), 
                new SqlParameter("@blogid", blogid),
                new SqlParameter("@blogmid", blogmid), 
                new SqlParameter("@idstr", idstr), 
                new SqlParameter("@text", text), 
                new SqlParameter("@source", source), 
                new SqlParameter("@favorited", favorited.ToString()), 
                new SqlParameter("@truncated", truncated.ToString()), 
                new SqlParameter("@in_reply_to_status_id", in_reply_to_status_id), 
                new SqlParameter("@in_reply_to_user_id", in_reply_to_user_id), 
                new SqlParameter("@in_reply_to_screen_name", in_reply_to_screen_name), 
                new SqlParameter("@thumbnail_pic", thumbnail_pic+" "),
                new SqlParameter("@reposts_count", reposts_count.ToString()), 
                new SqlParameter("@comments_count", comments_count.ToString())
            };
            cmd.Parameters.AddRange(paras);
            Boolean flag = false;
            try
            {
                conn.Open();
                flag = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception e)
            {
                log.Info(e.Message.ToString());
                sql = "insert into Blog(created_at,blogid,blogmid,idstr,text,source,favorited,truncated,in_reply_to_status_id,in_reply_to_user_id," +
                       "in_reply_to_screen_name,thumbnail_pic,reposts_count,comments_count) values('" + created_at + "','" + blogid + "','" + blogmid + "','" +
                       idstr + "','" + text + "','" + source + "'," + favorited + "," + truncated + ",'" + in_reply_to_status_id + "','" + in_reply_to_user_id + "','" + in_reply_to_screen_name + "','" +
                       thumbnail_pic + "'," + reposts_count + "," + comments_count + ")";
                log.Info(sql);
            }
            finally
            {
                conn.Close();
            }
            return flag;
        }

        public Boolean insertUserParameter(string uid, string idstr, string screen_name, string name, int province, int city, string location, string description,
        string url, string profile_image_url, string profile_url, string domain, string weihao, string gender, int followers_count, int friends_count, int statuses_count, int favourites_count,
        string created_at, int following, int allow_all_act_msg, int geo_enabled, int verified, int verified_type, string remark, int allow_all_comment,
        string avatar_large, string verified_reason, int follow_me, int online_status, int bi_followers_count, string lang)
        {
            string sql = "insert into Users(uid,idstr,screen_name,name,province,city,location,description,url,profile_image_url,profile_url,domain,"+
                         "weihao,gender,followers_count,friends_count,statuses_count,favourites_count,created_at,following,allow_all_act_msg,geo_enabled,"+
                         "verified,verified_type,remark,allow_all_comment,avatar_large,verified_reason,follow_me,online_status,bi_followers_count,lang)"+
                         " values(@uid,@idstr,@screen_name,@name,@province,@city,@location,@description,@url,@profile_image_url,@profile_url,@domain,"+
                         "@weihao,@gender,@followers_count,@friends_count,@statuses_count,@favourites_count,@created_at,@following,@allow_all_act_msg,"+
                         "@geo_enabled,@verified,@verified_type,@remark,@allow_all_comment,@avatar_large,@verified_reason,@follow_me,@online_status,@bi_followers_count,@lang)";
            SqlConnection conn = SQLServerUnit.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@uid",uid),
                new SqlParameter("@idstr",idstr),
                new SqlParameter("@screen_name",screen_name),
                new SqlParameter("@name",name),
                new SqlParameter("@province",province),
                new SqlParameter("@city",city),
                new SqlParameter("@location",location),
                new SqlParameter("@description",description),
                new SqlParameter("@url",url),
                new SqlParameter("@profile_image_url",profile_image_url),
                new SqlParameter("@profile_url",profile_url),
                new SqlParameter("@domain",domain),
                new SqlParameter("@weihao",weihao),
                new SqlParameter("@gender",gender),
                new SqlParameter("@followers_count",followers_count),
                new SqlParameter("@friends_count",friends_count),
                new SqlParameter("@statuses_count",statuses_count),
                new SqlParameter("@favourites_count",favourites_count),
                new SqlParameter("@created_at",created_at),
                new SqlParameter("@following",following),
                new SqlParameter("@allow_all_act_msg",allow_all_act_msg),
                new SqlParameter("@geo_enabled",geo_enabled),
                new SqlParameter("@verified",verified),
                new SqlParameter("@verified_type",verified_type),
                new SqlParameter("@remark",remark),
                new SqlParameter("@allow_all_comment",allow_all_comment),
                new SqlParameter("@avatar_large",avatar_large),
                new SqlParameter("@verified_reason",verified_reason),
                new SqlParameter("@follow_me",follow_me),
                new SqlParameter("@online_status",online_status),
                new SqlParameter("@bi_followers_count",bi_followers_count),
                new SqlParameter("@lang",lang)
            };
            cmd.Parameters.AddRange(paras);
            Boolean flag = false;
            try
            {
                conn.Open();
                flag = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception e)
            {
                log.Info(e.Message.ToString());
                sql = "insert into Users(uid,idstr,screen_name,name,province,city,location,description" +
                ",url,profile_image_url,profile_url,domain,weihao,gender,followers_count,friends_count,statuses_count,favourites_count," +
                "created_at,following,allow_all_act_msg,geo_enabled,verified,verified_type,remark,allow_all_comment," +
                "avatar_large,verified_reason,follow_me,online_status,bi_followers_count,lang) values(" + uid + ",'" + idstr + "','" + screen_name + "','" + name + "'," + province + "," + city + ",'" + location + "','" + description +
                "','" + url + "','" + profile_image_url + "','" + profile_url + "','" + domain + "','" + weihao + "','" + gender + "'," + followers_count + "," + friends_count + "," + statuses_count + "," + favourites_count + ",'" +
                created_at + "'," + following + "," + allow_all_act_msg + "," + geo_enabled + "," + verified + "," + verified_type + ",'" + remark + "'," + allow_all_comment + ",'" +
                avatar_large + "','" + verified_reason + "'," + follow_me + "," + online_status + "," + bi_followers_count + ",'" + lang + "')";
                log.Info(sql);
            }
            finally
            {
                conn.Close();
            }
            return flag;
        }

        public Boolean insertUserTagsParameter(string uid, string tagid, string name, int weight)
        {

            string sql = "insert into User_tags(uid,tagid,name,weight) values(@uid,@tagid,@name,@weight)";
            SqlConnection conn = SQLServerUnit.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@uid",uid),
                new SqlParameter("@tagid",tagid),
                new SqlParameter("@name",name),
                new SqlParameter("@weight",weight)
            };

            cmd.Parameters.AddRange(paras);
            Boolean flag = false;
            try
            {
                conn.Open();
                flag = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception e)
            {
                log.Info(e.Message.ToString());
                sql = "insert into User_tags(uid,tagid,name,weight) values('" + uid + "','" + tagid + "','" + name + "'," + weight + ")";
                log.Info(sql);
            }
            finally
            {
                conn.Close();
            }
            return flag;
        }

        public Boolean insertUserBlogIDSParameter(string uid, string blogID, string retweeted_status)
        {
            string sql = "insert into User_Blog_IDS(uid,blogID,retweeted_status) values(@uid,@blogID,@retweeted_status)";
            SqlConnection conn = SQLServerUnit.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@uid",uid),
                new SqlParameter("@blogID",blogID),
                new SqlParameter("@retweeted_status",retweeted_status)
            };

            cmd.Parameters.AddRange(paras);
            Boolean flag = false;
            try
            {
                conn.Open();
                flag = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception e)
            {
                log.Info(e.Message.ToString());
                sql = "insert into User_Blog_IDS(uid,blogID,retweeted_status) values('" + uid + "','" + blogID + "','" + retweeted_status + "')";
                log.Info(sql);
            }
            finally
            {
                conn.Close();
            }
            return flag;
        }
    }
}
