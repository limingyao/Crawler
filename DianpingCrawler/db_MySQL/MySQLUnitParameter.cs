using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace db_MySQL
{
    public class MySQLUnitParameter
    {
        private static MySQLUnitParameter instance;
        private static readonly object syncRoot = new object();
        private static readonly object syncInsertOrUpdataList = new object();
        private static readonly object syncInsertOrUpdataDetail = new object();
        private static readonly object syncInsertOrUpdataComment = new object();
        private static readonly object syncInsertOrUpdataUser = new object();

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

        public Boolean insertOrUpdataShopList(string shopid, string name, string nickname, string url, string batchflag,string flag)
        {
            lock (syncInsertOrUpdataList)
            {
                DataRowCollection rows = MySQLUnit.getInstance().query("SELECT * FROM shoplist WHERE shopid='"+shopid+"'").Rows; 
                int count = rows.Count;
                MySqlConnection sqlConn = this.getConnection();
                MySqlCommand sqlCommand = new MySqlCommand("set names utf8", sqlConn);
                sqlCommand.ExecuteNonQuery();
                string sql = "";
                if(count>0)
                {
                    flag = rows[0]["flag"].ToString();
                    batchflag = rows[0]["batchflag"].ToString();
                    sql = "UPDATE shoplist SET `name`=?name,nickname=?nickname,url=?url,batchflag=?batchflag,flag=?flag WHERE shopid=?shopid";
                }
                else
                {
                    sql = "INSERT INTO shoplist(shopid,name,nickname,url,batchflag,flag) VALUES(?shopid,?name,?nickname,?url,?batchflag,?flag)";
                }                
                sqlCommand.CommandText = sql;
                MySqlParameter[] para ={
                new MySqlParameter("?shopid",MySqlDbType.Int32),
                new MySqlParameter("?name",MySqlDbType.VarChar,255),
                new MySqlParameter("?nickname",MySqlDbType.VarChar,255),
                new MySqlParameter("?url",MySqlDbType.VarChar,255),
                new MySqlParameter("?batchflag",MySqlDbType.Int32),
                new MySqlParameter("?flag",MySqlDbType.Int32)};
                para[0].Value = shopid;
                para[1].Value = name;
                para[2].Value = nickname;
                para[3].Value = url;
                para[4].Value = batchflag;
                para[5].Value = flag;
                sqlCommand.Parameters.AddRange(para);
                int num = sqlCommand.ExecuteNonQuery();
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
                return num > 0 ? true : false;
            }
        }        
        public Boolean insertOrUpdataShopDetial(string shopid, string isv, string shopscore, string percapita, string taste, string ambience, string service, string address, string tel, string special, string feature, string tag, string introduction, string category, string favourable)
        {
            lock (syncInsertOrUpdataDetail)
            {
                int count = MySQLUnit.getInstance().query("SELECT * FROM shopdetial WHERE shopid='" + shopid + "'").Rows.Count;
                MySqlConnection sqlConn = this.getConnection();
                MySqlCommand sqlCommand = new MySqlCommand("set names utf8", sqlConn);
                sqlCommand.ExecuteNonQuery();
                string sql = "";
                if (count > 0)
                {
                    sql = "UPDATE shopdetial SET isv=?isv,shopscore=?shopscore,percapita=?percapita,taste=?taste,ambience=?ambience,service=?service,address=?address,tel=?tel,special=?special,feature=?feature,tag=?tag,introduction=?introduction,category=?category,favourable=?favourable WHERE shopid=?shopid";
                }
                else
                {
                    sql = "INSERT INTO shopdetial(shopid,isv,shopscore,percapita,taste,ambience,service,address,tel,special,feature,tag,introduction,category,favourable) VALUES(?shopid,?isv,?shopscore,?percapita,?taste,?ambience,?service,?address,?tel,?special,?feature,?tag,?introduction,?category,?favourable)";
                }
                sqlCommand.CommandText = sql;
                MySqlParameter[] para ={
                new MySqlParameter("?shopid",MySqlDbType.Int32),
                new MySqlParameter("?isv",MySqlDbType.Int32),
                new MySqlParameter("?shopscore",MySqlDbType.VarChar,255),
                new MySqlParameter("?percapita",MySqlDbType.VarChar,255),
                new MySqlParameter("?taste",MySqlDbType.VarChar,255),
                new MySqlParameter("?ambience",MySqlDbType.VarChar,255),
                new MySqlParameter("?service",MySqlDbType.VarChar,255),
                new MySqlParameter("?address",MySqlDbType.VarChar,255),
                new MySqlParameter("?tel",MySqlDbType.VarChar,255),
                new MySqlParameter("?special",MySqlDbType.VarChar,255),
                new MySqlParameter("?feature",MySqlDbType.VarChar,255),
                new MySqlParameter("?tag",MySqlDbType.VarChar,255),
                new MySqlParameter("?introduction",MySqlDbType.VarChar,255),
                new MySqlParameter("?category",MySqlDbType.VarChar,255),
                new MySqlParameter("?favourable",MySqlDbType.VarChar,255)};
                para[0].Value = shopid;
                para[1].Value = isv;
                para[2].Value = shopscore;
                para[3].Value = percapita;
                para[4].Value = taste;
                para[5].Value = ambience;
                para[6].Value = service;
                para[7].Value = address;
                para[8].Value = tel;
                para[9].Value = special;
                para[10].Value = feature;
                para[11].Value = tag;
                para[12].Value = introduction;
                para[13].Value = category;
                para[14].Value = favourable;
                sqlCommand.Parameters.AddRange(para);
                int num = sqlCommand.ExecuteNonQuery();
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
                return num > 0 ? true : false;
            }
        }
        public Boolean insertOrUpdataShopComment(string commentid, string shopid, string userid, string userscore, string percapita, string taste, string ambience, string service, string commenttype, string comment, string date)
        {
            lock (syncInsertOrUpdataComment)
            {
                int count = MySQLUnit.getInstance().query("SELECT * FROM shopcomment WHERE commentid='" + commentid + "'").Rows.Count;
                MySqlConnection sqlConn = this.getConnection();
                MySqlCommand sqlCommand = new MySqlCommand("set names utf8", sqlConn);
                sqlCommand.ExecuteNonQuery();
                string sql = "";
                if (count > 0)
                {
                    sql = "UPDATE shopcomment SET shopid=?shopid,userid=?userid,userscore=?userscore,percapita=?percapita,taste=?taste,ambience=?ambience,service=?service,commenttype=?commenttype,comment=?comment,date=?date WHERE commentid=?commentid";
                }
                else
                {
                    sql = "INSERT INTO shopcomment(commentid,shopid,userid,userscore,percapita,taste,ambience,service,commenttype,comment,date) VALUES(?commentid,?shopid,?userid,?userscore,?percapita,?taste,?ambience,?service,?commenttype,?comment,?date)";
                }
                sqlCommand.CommandText = sql;
                MySqlParameter[] para ={
                new MySqlParameter("?commentid",MySqlDbType.Int32),
                new MySqlParameter("?shopid",MySqlDbType.Int32),
                new MySqlParameter("?userid",MySqlDbType.Int32),
                new MySqlParameter("?userscore",MySqlDbType.VarChar,255),
                new MySqlParameter("?percapita",MySqlDbType.VarChar,255),
                new MySqlParameter("?taste",MySqlDbType.VarChar,255),
                new MySqlParameter("?ambience",MySqlDbType.VarChar,255),
                new MySqlParameter("?service",MySqlDbType.VarChar,255),
                new MySqlParameter("?commenttype",MySqlDbType.VarChar,255),
                new MySqlParameter("?comment",MySqlDbType.Text),
                new MySqlParameter("?date",MySqlDbType.VarChar,255)};
                para[0].Value = commentid;
                para[1].Value = shopid;
                para[2].Value = userid;
                para[3].Value = userscore;
                para[4].Value = percapita;
                para[5].Value = taste;
                para[6].Value = ambience;
                para[7].Value = service;
                para[8].Value = commenttype;
                para[9].Value = comment;
                para[10].Value = date;
                sqlCommand.Parameters.AddRange(para);
                int num = sqlCommand.ExecuteNonQuery();
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
                return num > 0 ? true : false;
            }
        }
        public Boolean insertOrUpdataUser(string userid, string username, string userrank)
        {
            lock (syncInsertOrUpdataUser)
            {
                int count = MySQLUnit.getInstance().query("SELECT * FROM user WHERE userid='" + userid + "'").Rows.Count;
                MySqlConnection sqlConn = this.getConnection();
                MySqlCommand sqlCommand = new MySqlCommand("set names utf8", sqlConn);
                sqlCommand.ExecuteNonQuery();
                string sql = "";
                if (count > 0)
                {
                    sql = "UPDATE user SET username=?username,userrank=?userrank WHERE userid=?userid";
                }
                else
                {
                    sql = "INSERT INTO user(userid,username,userrank) VALUES(?userid,?username,?userrank)";
                }
                sqlCommand.CommandText = sql;
                MySqlParameter[] para ={
                new MySqlParameter("?userid", MySqlDbType.Int32),
                new MySqlParameter("?username", MySqlDbType.VarChar,255),
                new MySqlParameter("?userrank", MySqlDbType.VarChar,255)};
                para[0].Value = userid;
                para[1].Value = username;
                para[2].Value = userrank;
                sqlCommand.Parameters.AddRange(para);
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
