using db_SQLServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Client
{
    public class Topic
    {
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="status"></param>
        /// <param name="time"></param>
        /// <param name="praise"></param>
        /// <param name="forwarding"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public Boolean insert(string userid, string username, string status, string time, string praise, string forwarding, string comment)
        {
            string sql = "INSERT INTO topic(userid,username,status,[time],praise,forwarding,comment) VALUES(@userid,@username,@status,@time,@praise,@forwarding,@comment)";
            SqlConnection conn = SQLServerUnit.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@username",username),
                    new SqlParameter("@status",status),
                    new SqlParameter("@time",time),
                    new SqlParameter("@praise",praise),
                    new SqlParameter("@forwarding",forwarding),
                    new SqlParameter("@comment",comment)
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
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return flag;
        }
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="status"></param>
        /// <param name="time">格式： yyyy-MM-dd HH:mm</param>
        /// <param name="praise"></param>
        /// <param name="forwarding"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public Boolean update(string userid, string username, string status, string time, string praise, string forwarding, string comment)
        {
            string sql = "UPDATE topic SET praise=@praise,forwarding=@forwarding,comment=@comment WHERE userid=@userid AND status=@status AND SUBSTRING([time],1,11)=@time";
            SqlConnection conn = SQLServerUnit.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@username",username),
                    new SqlParameter("@status",status),
                    new SqlParameter("@time",time),
                    new SqlParameter("@praise",praise),
                    new SqlParameter("@forwarding",forwarding),
                    new SqlParameter("@comment",comment)
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
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return flag;
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="status"></param>
        /// <param name="time">格式： yyyy-MM-dd HH:mm</param>
        /// <returns></returns>
        public Boolean query(string userid, string status, string time)
        {
            string sql = "SELECT COUNT(*) FROM topic WHERE userid=@userid AND status=@status AND SUBSTRING([time],1,11)=@time";
            SqlConnection conn = SQLServerUnit.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@status",status),
                    new SqlParameter("@time",time)
                };
            cmd.Parameters.AddRange(paras);
            Boolean flag = false;
            try
            {
                conn.Open();
                flag = Convert.ToInt32(cmd.ExecuteScalar()) > 0 ? true : false;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return flag;
        }
    }
}
