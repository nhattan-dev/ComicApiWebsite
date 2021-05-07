using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ComicApiWeb.Models
{
    public class Userr
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KEY"></param>
        /// <returns>-1:failure 0:TRANS 1:ADMIN</returns>
        public static int checkLogin(string KEY)
        {
            string[] paras = new string[1] { "key" };
            object[] values = new object[1] { KEY };
            string query = "SELECT role FROM Userr WHERE username + password = @KEY";
            DataSet ds = Connection.Connection.FillDataSet(query, paras, values);
            if (ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) return -1;
            try {
                if (ds.Tables[0].Rows[0]["role"].ToString().Equals("ADMIN")) return 1;
                if (ds.Tables[0].Rows[0]["role"].ToString().Equals("TRANS")) return 0;
            }
            catch { return -1; }
            return -1;
        }
    }
}