using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace db_SQLServer
{
    public class SQLServerUnit
    {
        //创建日志记录组件实例
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static SQLServerUnit instance;
        private static readonly object syncRoot = new object();
        private static readonly object syncExecute = new object();
        private static readonly object syncQuery = new object();
        private static readonly object syncInsertLog = new object();

        private SQLServerUnit()
        {
        }

        public static SQLServerUnit getInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new SQLServerUnit();
                    }
                }
            }
            return instance;
        }

        //建立DB连接
        public SqlConnection getConnection()
        {
            string strConnInfo = System.Configuration.ConfigurationManager.AppSettings["sqlServerlUrl"];
            return new SqlConnection(strConnInfo);
        }

        public DataTable query(String sql)
        {
            lock (syncQuery)
            {
                SqlConnection sqlConn = getConnection();
                if (sqlConn.State != ConnectionState.Open)
                {
                    sqlConn.Open();
                }
                DataTable dt = new DataTable();
                SqlCommand sqlcmd = new SqlCommand(sql, sqlConn);
                try
                {
                    SqlDataReader dr = sqlcmd.ExecuteReader();
                    dt.Load(dr);
                }
                catch (SqlException ae)
                {
                    log.Info(ae.Message.ToString());
                    log.Info(sql);
                }
                finally
                {
                    sqlcmd.Dispose();
                    sqlConn.Close();
                }
                return dt;
            }
        }

        private Boolean executeUpdate(String sql)
        {
            lock (syncExecute)
            {
                int num = -1;
                SqlConnection sqlConn = getConnection();
                if (sqlConn.State != ConnectionState.Open)
                {
                    sqlConn.Open();
                }
                SqlCommand sqlcmd = new SqlCommand(sql, sqlConn);
                try
                {
                    num = sqlcmd.ExecuteNonQuery();
                }
                catch (SqlException ae)
                {
                    log.Info(ae.Message.ToString());
                    log.Info(sql);
                }
                finally
                {
                    sqlcmd.Dispose();
                    sqlConn.Close();
                }
                return num > 0 ? true : false;
            }
        }
        public Boolean insert(string sql)
        {
            return executeUpdate(sql);
        }
        public Boolean update(string sql)
        {
            return executeUpdate(sql);
        }
        public Boolean delete(string sql)
        {
            return executeUpdate(sql);
        }

        public Boolean isExist(string sql)
        {
            DataTable table = query(sql);
            return table.Rows.Count > 0 ? true : false;
        }

        public Boolean insertLog(string threadid, string uid, string error, string errormessage, string time)
        {
            lock (syncInsertLog)
            {

                string sql = "INSERT INTO log(threadid, uid,error,errormessage, time) VALUES ( @threadid,@uid,@error,@errormessage,@time)";
                SqlConnection conn = SQLServerUnit.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlParameter[] paras = new SqlParameter[] { 
                    new SqlParameter("@threadid",threadid),
                    new SqlParameter("@uid",uid),
                    new SqlParameter("@error",error),
                    new SqlParameter("@errormessage",errormessage),
                    new SqlParameter("@time",time)
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
                    log.Info(sql);
                    sql = "insert into log(threadid, uid,error,errormessage, time) values('" + threadid + "','" + uid + "','" + error + "','" + errormessage + "','" + time + "')";
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
}
