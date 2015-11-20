using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace db_MySQL
{
    public class MySQLUnitParameter
    {
        private static MySQLUnitParameter instance;
        private static readonly object syncRoot = new object();
        private static readonly object syncInsertLog = new object();

        private MySQLUnitParameter()
        {
        }

        public static MySQLUnitParameter getInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new MySQLUnitParameter();
                    }
                }
            }
            return instance;
        }

        //建立DB连接
        private MySqlConnection getConnection()
        {
            string contString = System.Configuration.ConfigurationManager.AppSettings["mysqlUrl"];
            MySqlConnection conn = new MySqlConnection(contString);
            conn.Open();
            return conn;
        }

        public Boolean insert(List<Object> list)
        {
            MySqlConnection sqlConn = this.getConnection();
            MySqlCommand sqlCommand = new MySqlCommand("set names utf8", sqlConn);
            sqlCommand.ExecuteNonQuery();
            string sql = "insert into test(name) values (?name)";
            sqlCommand.CommandText = sql;
            MySqlParameter[] para ={
				new MySqlParameter ("?name",MySqlDbType .VarChar,20),                                        
            };
            ////
            para[0].Value = "777";
            for (int i = 0; i < para.Length; i++)
            {
                sqlCommand.Parameters.Add(para[i]);
            }
            ////
            int num = sqlCommand.ExecuteNonQuery();
            if (sqlConn != null)
            {
                sqlConn.Close();
            }
            return num > 0 ? true : false;
        }

        public Boolean insertLog(string threadid, string uid, string error, string errormessage, string time)
        {
            lock (syncInsertLog)
            {
                MySqlConnection sqlConn = this.getConnection();
                MySqlCommand sqlCommand = new MySqlCommand("set names utf8", sqlConn);
                sqlCommand.ExecuteNonQuery();
                string sql = "INSERT INTO log(threadid, uid,error,errormessage, time) VALUES ( ?threadid,?uid,?error,?errormessage,?time)";
                sqlCommand.CommandText = sql;
                MySqlParameter[] para ={
				new MySqlParameter("?threadid",MySqlDbType.Int32),  
                new MySqlParameter("?uid",MySqlDbType.VarChar,20),
                new MySqlParameter("?error",MySqlDbType.LongText),
                new MySqlParameter("?errormessage",MySqlDbType.LongText),
                new MySqlParameter("?time",MySqlDbType.VarChar,30)};
                para[0].Value = threadid;
                para[1].Value = uid;
                para[2].Value = error;
                para[3].Value = errormessage;
                para[4].Value = time;
                for (int i = 0; i < para.Length; i++)
                {
                    sqlCommand.Parameters.Add(para[i]);
                }
                int num = sqlCommand.ExecuteNonQuery();
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
                return num > 0 ? true : false;
            }
        }
    }
}
