using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ComicApiWeb.Models
{
    public class Chapter
    {
        public int chapter_id { get; set; }
        public string name { get; set; }
        public DateTime update_time { get; set; }
        public int view { get; set; }
        public string[] link { get; set; }
        public int comic_id { get; set; }
        public static string getParentChapterFolderID(int comic_id)
        {
            string[] paras = new string[1] { "comic_id" };
            object[] values = new object[1] { comic_id };
            DataSet ds = null;
            string query = "SELECT folderID FROM Comic WHERE comic_id = @comic_id";
            ds = Connection.Connection.FillDataSet(query, paras, values);
            if (ds.Tables.Count < 1 && ds.Tables[0].Rows.Count < 1) return "";
            return ds.Tables[0].Rows[0]["folderID"].ToString();
        }
        public static string getChapterFolderID(int chapter_id)
        {
            string[] paras = new string[1] { "chapter_id" };
            object[] values = new object[1] { chapter_id };
            DataSet ds = null;
            string query = "SELECT folderID FROM Chapter WHERE chapter_id = @chapter_id";
            ds = Connection.Connection.FillDataSet(query, paras, values);
            if (ds.Tables.Count <= 0 && ds.Tables[0].Rows.Count <= 0) return "";
            return ds.Tables[0].Rows[0]["folderID"].ToString();
        }
    }
}