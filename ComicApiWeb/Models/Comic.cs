using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ComicApiWeb.Models
{
    public class Comic
    {
        public int comic_id { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public string summary { get; set; }
        public string author { get; set; }
        public string trans { get; set; }
        public int genre_id { get; set; }
        public Chapter[] chapters { get; set; }
        public string folderID { get; set; }

        public static string getComicFolderID(int comic_id)
        {
            string[] paras = new string[1] { "comic_id" };
            object[] values = new object[1] { comic_id };
            DataSet ds = null;
            string query = "SELECT folderID FROM Comic WHERE comic_id = @comic_id";
            ds = Connection.Connection.FillDataSet(query, paras, values);
            if (ds.Tables.Count <= 0 && ds.Tables[0].Rows.Count <= 0) return "";
            return ds.Tables[0].Rows[0]["folderID"].ToString();
        }
    }
}