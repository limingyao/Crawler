using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace db_MySQL
{
    public class MySQLUnit
    {
        private static MySQLUnit instance;
        private static readonly object syncRoot = new object();
        private static readonly object syncExecute = new object();
        private static readonly object syncQuery = new object();

        private MySQLUnit()
        {
        }

        public static MySQLUnit getInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new MySQLUnit();
                    }
                }
            }
            return instance;
        }

        //建立DB连接
        private MySqlConnection getConnection()
        {
            string contString = System.Configuration.ConfigurationManager.AppSettings["mysqlUrl"];
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = contString;
            conn.Open();
            return conn;
        }
        //数据查询操作   
        public DataTable query(String sql)
        {
            lock (syncQuery)
            {
                MySqlConnection sqlConn = this.getConnection();
                MySqlCommand sqlCommand = sqlConn.CreateCommand();
                sqlCommand.CommandText = sql;
                MySqlDataAdapter adapter = new MySqlDataAdapter(sqlCommand);
                DataTable table = new DataTable();
                adapter.Fill(table);
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
                return table;
            }
        }
        //数据插入,删除,更新操作
        private Boolean executeUpdate(String sql)
        {
            lock (syncExecute)
            {
                MySqlConnection sqlConn = this.getConnection();
                MySqlCommand sqlCommand = sqlConn.CreateCommand();
                sqlCommand.CommandText = sql;
                int num = sqlCommand.ExecuteNonQuery();
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
                return num == 0 ? false : true;
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
    }
}
