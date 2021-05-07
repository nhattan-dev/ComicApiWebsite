using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ComicApiWeb.Connection
{
    public class Connection
    {
        public static string sqlcon;
        public static SqlConnection Getconnection()
        {
            //sqlcon = "server =" + "LAPTOP" + "; uid= " + "sa" + "; pwd= " + "sa" + "; database = " + "ComicApp";
            sqlcon = "workstation id=comicapp.mssql.somee.com;packet size=4096;user id=ltwnhattan_SQLLogin_1;pwd=667c4n9drv;data source=comicapp.mssql.somee.com;persist security info=False;initial catalog=comicapp";
            SqlConnection con = new SqlConnection(sqlcon);
            return con;
        }
        public static int RequestStatus(string strQuery, CommandType cmdtype, string[] para, object[] values)
        {

            SqlConnection conn = null;
            int RequestStatus = -1;
            try
            {
                conn = Getconnection();
                conn.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.CommandText = strQuery;
                sqlcmd.Connection = conn;
                sqlcmd.CommandType = cmdtype;

                SqlParameter sqlpara;
                for (int i = 0; i < para.Length; i++)
                {
                    sqlpara = new SqlParameter();
                    sqlpara.ParameterName = para[i];
                    sqlpara.SqlValue = values[i];
                    sqlcmd.Parameters.Add(sqlpara);
                }
                sqlpara = new SqlParameter("@RequestStatus", SqlDbType.Int);
                sqlpara.Direction = ParameterDirection.ReturnValue;
                sqlcmd.Parameters.Add(sqlpara);
                sqlcmd.ExecuteNonQuery();
                RequestStatus = (int)sqlcmd.Parameters["@RequestStatus"].Value;
            }
            catch 
            {
            }
            finally
            {
                conn.Close();
            }
            return RequestStatus;

        }
        public static int RequestStatus(string strQuery,string[] para, object[] values)
        {

            SqlConnection conn = null;
            int RequestStatus = -1;
            try
            {
                conn = Getconnection();
                conn.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.CommandText = strQuery;
                sqlcmd.Connection = conn;

                SqlParameter sqlpara;
                for (int i = 0; i < para.Length; i++)
                {
                    sqlpara = new SqlParameter();
                    sqlpara.ParameterName = para[i];
                    sqlpara.SqlValue = values[i];
                    sqlcmd.Parameters.Add(sqlpara);
                }
                sqlpara = new SqlParameter("@RequestStatus", SqlDbType.Int);
                sqlpara.Direction = ParameterDirection.ReturnValue;
                sqlcmd.Parameters.Add(sqlpara);
                sqlcmd.ExecuteNonQuery();
                RequestStatus = (int)sqlcmd.Parameters["@RequestStatus"].Value;
            }
            catch 
            {
            }
            finally
            {
                conn.Close();
            }
            return RequestStatus;

        }

        public static DataSet FillDataSet(string strQuery, CommandType cmdtype)
        {
            SqlConnection con = null;
            DataSet ds = new DataSet();
            try
            {
                con = Getconnection();
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strQuery;
                cmd.CommandType = cmdtype;
                cmd.Connection = con;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                da.Dispose();
            }
            catch
            {
            }
            finally
            {
                con.Close();
            }
            return ds;
        }
        public static DataSet FillDataSet(string strQuery)
        {
            SqlConnection con = null;
            DataSet ds = new DataSet();
            try
            {
                con = Getconnection();
                con.Open();
                SqlCommand cmd = new SqlCommand(strQuery,con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                da.Dispose();
            }
            catch  
            {
            }
            finally
            {
                con.Close();
            }
            return ds;
        }

        public static DataSet FillDataSet(string strQuery, CommandType cmdtype, string[] para, object[] values)
        {
            DataSet ds = new DataSet();
            SqlConnection con = null;
            try
            {
                con = Getconnection();
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strQuery;
                cmd.CommandType = cmdtype;
                cmd.Connection = con;

                SqlParameter sqlpara;
                for (int i = 0; i < para.Length; i++)
                {
                    sqlpara = new SqlParameter();
                    sqlpara.ParameterName = para[i];
                    sqlpara.SqlValue = values[i];
                    cmd.Parameters.Add(sqlpara);
                }

                SqlDataAdapter sqlda = new SqlDataAdapter(cmd);
                sqlda.Fill(ds);
                sqlda.Dispose();
            }
            catch
            {
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return ds;
        }
        public static DataSet FillDataSet(string strQuery, string[] para, object[] values)
        {
            DataSet ds = new DataSet();
            SqlConnection con = null;
            try
            {
                con = Getconnection();
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strQuery;
                cmd.Connection = con;

                SqlParameter sqlpara;
                for (int i = 0; i < para.Length; i++)
                {
                    sqlpara = new SqlParameter();
                    sqlpara.ParameterName = para[i];
                    sqlpara.SqlValue = values[i];
                    cmd.Parameters.Add(sqlpara);
                }

                SqlDataAdapter sqlda = new SqlDataAdapter(cmd);
                sqlda.Fill(ds);
                sqlda.Dispose();
            }
            catch
            {
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return ds;
        }
        public static string ExcuteScalar(string stringSQL)
        {
            string giaTri = "";
            SqlConnection sqlconn = null;
            try
            {
                sqlconn = Getconnection();
                sqlconn.Open();
                SqlCommand cmd = new SqlCommand(stringSQL, sqlconn);
                giaTri = cmd.ExecuteScalar().ToString();
            }
            catch { }
            finally
            {
                if (sqlconn != null)
                    sqlconn.Close();
            }
            return giaTri;
        }
        public static string ExcuteScalar(string strQuery, string[] para, object[] values)
        {
            SqlConnection conn = new SqlConnection();
            conn = Getconnection();
            conn.Open();
            string value = "";
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandText = strQuery;
            sqlcmd.Connection = conn;

            SqlParameter sqlpara;
            for (int i = 0; i < para.Length; i++)
            {
                sqlpara = new SqlParameter();
                sqlpara.ParameterName = para[i];
                sqlpara.SqlValue = values[i];
                sqlcmd.Parameters.Add(sqlpara);
            }
            try
            {
                value = sqlcmd.ExecuteScalar().ToString();
            }
            catch
            {
            }
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strQuery"></param>
        /// <param name="para"></param>
        /// <param name="values"></param>
        /// <returns> <= 0 failure</returns>
        public static int ExcuteNonQuery(string strQuery, string[] para, object[] values)
        {
            SqlConnection conn = new SqlConnection();
            conn = Getconnection();
            conn.Open();
            int value = -1;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandText = strQuery;
            sqlcmd.Connection = conn;

            SqlParameter sqlpara;
            for (int i = 0; i < para.Length; i++)
            {
                sqlpara = new SqlParameter();
                sqlpara.ParameterName = para[i];
                sqlpara.SqlValue = values[i];
                sqlcmd.Parameters.Add(sqlpara);
            }
            try
            {
                value = sqlcmd.ExecuteNonQuery();
            }
            catch
            {
            }
            return value;
        }
    }
}