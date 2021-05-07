using ComicApiWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web.Http;

namespace ComicApiWeb.Controllers
{
    public class CommentApiController : ApiController
    {
        //GET: api/ChapterApi/
        public int Get(string comic_idd)
        {
            int sum = 0;
            string[] paras = new string[1] { "comic_id" };
            object[] values = new object[1] { comic_idd };
            string query = "select sum(sl) from ( select (select count(cmt_id) from Comment where comment.chapter_id = chapter.chapter_id) as sl from Chapter where comic_id = @comic_id) as c";
            sum = Int32.Parse(Connection.Connection.ExcuteScalar(query, paras, values));
            return sum;
        }

        // GET: api/ChapterApi/5
        public IEnumerable<Comment> Get(int comic_id)
        {
            var coms = new List<Comment>();
            string[] paras = new string[1] { "comic_id" };
            object[] values = new object[1] { comic_id };
            string query = "SELECT cmt_id, commentator, cmt_content, cmt_time, chapter_id, comic_id FROM ( SELECT ROW_NUMBER() OVER (ORDER BY cmt_time desc) AS rownumber, cmt_id, commentator, cmt_content, cmt_time, C.chapter_id, comic_id FROM Chapter AS C INNER JOIN Comment ON C.chapter_id = Comment.chapter_id WHERE C.comic_id = @comic_id ) AS foo WHERE rownumber <= 20 and rownumber >= 1";
            DataSet data = Connection.Connection.FillDataSet(query, paras, values);
            try
            {
                if (data.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                    {
                        Comment com = new Comment();

                        com.cmt_id = Convert.ToInt32(data.Tables[0].Rows[i]["cmt_id"].ToString());
                        com.commentator = data.Tables[0].Rows[i]["commentator"].ToString();
                        com.cmt_content = data.Tables[0].Rows[i]["cmt_content"].ToString();
                        com.cmt_time = Convert.ToDateTime(data.Tables[0].Rows[i]["cmt_time"].ToString());
                        com.chapter_id = Convert.ToInt32(data.Tables[0].Rows[i]["chapter_id"].ToString());

                        coms.Add(com);
                    }
                }
            }
            catch { }
            return coms;
        }

        // GET: api/CommentApi/5
        public IEnumerable<Comment> Get(int comic_id, int begin, int end)
        {
            var coms = new List<Comment>();
            string[] paras = new string[1] { "comic_id" };
            object[] values = new object[1] { comic_id };
            string query = "SELECT cmt_id, commentator, cmt_content, cmt_time, chapter_id, comic_id FROM ( SELECT ROW_NUMBER() OVER (ORDER BY cmt_time desc) AS rownumber, cmt_id, commentator, cmt_content, cmt_time, C.chapter_id, comic_id FROM Chapter AS C INNER JOIN Comment ON C.chapter_id = Comment.chapter_id WHERE C.comic_id = @comic_id ) AS foo WHERE rownumber <= "+end+" and rownumber >= " + begin;
            DataSet data = Connection.Connection.FillDataSet(query, paras, values);
            try
            {
                if (data.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                    {
                        Comment com = new Comment();

                        com.cmt_id = Convert.ToInt32(data.Tables[0].Rows[i]["cmt_id"].ToString());
                        com.commentator = data.Tables[0].Rows[i]["commentator"].ToString();
                        com.cmt_content = data.Tables[0].Rows[i]["cmt_content"].ToString();
                        com.cmt_time = Convert.ToDateTime(data.Tables[0].Rows[i]["cmt_time"].ToString());
                        com.chapter_id = Convert.ToInt32(data.Tables[0].Rows[i]["chapter_id"].ToString());

                        coms.Add(com);
                    }
                }
            }
            catch { }
            return coms;
        }

        // POST: api/Comment
        public int Post([FromBody] Comment cmt)
        {
            if (string.IsNullOrWhiteSpace(cmt.commentator.Replace(" ", "")))
                return -1;
            if (string.IsNullOrWhiteSpace(cmt.cmt_content.Replace(" ", "")))
                return -2;
            //create new comment
            string[] paras = new string[3] { "cmt_content", "commentator", "chapter_id" };
            object[] values = new object[3] { cmt.cmt_content, cmt.commentator, cmt.chapter_id };
            string query = "INSERT INTO Comment(cmt_content, commentator,chapter_id) VALUES(@cmt_content, @commentator,@chapter_id)";
            int result = Connection.Connection.ExcuteNonQuery(query, paras, values);
            return result < 1 ? -3 : 0;
        }
    }
}
