using ComicApiWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ComicApiWeb.Controllers
{
    public class HomeApiController : ApiController
    {
        // GET: api/HomeApi?key=5
        public IEnumerable<Comic> Get(int key)
        {
            List<Comic> comics = new List<Comic>();
            try
            {
                DataSet comicDataSet = null;
                string query;
                if (key == 0)
                {
                    query = "SELECT comic_id, name, Image, author, trans, genre_id, summary FROM( SELECT comic_id, name, Image, author, trans, genre_id, summary, ROW_NUMBER() OVER (ORDER BY( SELECT TOP 1 update_time FROM Chapter WHERE Chapter.comic_id = Comic.comic_id ORDER BY update_time DESC) DESC) AS rownumber FROM Comic) AS foo WHERE rownumber <= 20 AND rownumber >= 1";
                    comicDataSet = Connection.Connection.FillDataSet(query);
                }
                else if (key == 1)
                {
                    query = "SELECT comic_id, name, Image, author, trans, genre_id, summary FROM ( SELECT comic_id, name, Image, author, trans, genre_id, summary, ROW_NUMBER() OVER (ORDER BY ( SELECT SUM(_view) FROM Chapter WHERE Chapter.comic_id = Comic.comic_id AND update_time BETWEEN GETDATE()-7 AND GETDATE()) desc) AS rownumber FROM Comic) AS foo WHERE rownumber <= 20 and rownumber >= 1";
                    comicDataSet = Connection.Connection.FillDataSet(query);
                }
                else if (key == 2)
                {
                    query = "SELECT TOP 6 comic_id, name, Image, author, trans, genre_id, summary FROM Comic  ORDER BY (SELECT SUM(_view) FROM Chapter WHERE Chapter.comic_id = Comic.comic_id AND update_time BETWEEN GETDATE()-1 AND GETDATE()) DESC";
                    comicDataSet = Connection.Connection.FillDataSet(query);
                }
                if (comicDataSet.Tables.Count <= 0)
                    return comics;
                for (int i = 0; i < comicDataSet.Tables[0].Rows.Count; i++)
                {
                    Comic comic = new Comic();
                    comic.name = comicDataSet.Tables[0].Rows[i]["name"].ToString();
                    comic.comic_id = Convert.ToInt32(comicDataSet.Tables[0].Rows[i]["comic_id"].ToString());
                    comic.genre_id = Convert.ToInt32(comicDataSet.Tables[0].Rows[i]["genre_id"].ToString());
                    comic.image = comicDataSet.Tables[0].Rows[i]["image"].ToString();
                    comic.author = comicDataSet.Tables[0].Rows[i]["author"].ToString();
                    comic.trans = comicDataSet.Tables[0].Rows[i]["trans"].ToString();
                    comic.summary = comicDataSet.Tables[0].Rows[i]["summary"].ToString();

                    List<Chapter> chapters = new List<Chapter>();
                    DataSet chapterDataSet = null;
                    string[] paras = new string[1] { "comic_id" };
                    object[] values = new object[1] { comic.comic_id };
                    query = "SELECT TOP 2 chapter_id, name, _view, update_time FROM Chapter WHERE comic_id = @comic_id";
                    chapterDataSet = Connection.Connection.FillDataSet(query, paras, values);
                    for (int j = 0; j < chapterDataSet.Tables[0].Rows.Count; j++)
                    {
                        Chapter chapter = new Chapter();

                        chapter.name = chapterDataSet.Tables[0].Rows[j]["name"].ToString();
                        chapter.chapter_id = Convert.ToInt32(chapterDataSet.Tables[0].Rows[j]["chapter_id"].ToString());
                        chapter.comic_id = comic.comic_id;
                        chapter.view = Convert.ToInt32(chapterDataSet.Tables[0].Rows[j]["_view"].ToString());
                        chapter.update_time = DateTime.Parse(chapterDataSet.Tables[0].Rows[j]["update_time"].ToString());

                        chapters.Add(chapter);
                    }
                    comic.chapters = chapters.ToArray();
                    comics.Add(comic);
                }
            }
            catch { }
            return comics;
        }

        // GET: api/HomeApi?key=5&begin=1&end=10
        public IEnumerable<Comic> Get(int key, int begin, int end)
        {
            List<Comic> comics = new List<Comic>();
            try
            {
                DataSet comicDataSet = null;
                string query;
                query = "SELECT COUNT(comic_id) FROM Comic";
                int SumRecords = Int32.Parse(Connection.Connection.ExcuteScalar(query));
                if (begin > end)
                    return comics;
                if (begin > SumRecords)
                    return comics;
                if (end > SumRecords)
                    end = SumRecords;
                if (key == 0)
                {
                    query = "SELECT comic_id, name, Image, author, trans, genre_id, summary FROM( SELECT comic_id, name, Image, author, trans, genre_id, summary, ROW_NUMBER() OVER (ORDER BY( SELECT TOP 1 update_time FROM Chapter WHERE Chapter.comic_id = Comic.comic_id ORDER BY update_time DESC) DESC) AS rownumber FROM Comic) AS foo WHERE rownumber <= " + end +" AND rownumber >= " + begin;
                    comicDataSet = Connection.Connection.FillDataSet(query);
                }
                else if (key == 1)
                {
                    query = "SELECT comic_id, name, Image, author, trans, genre_id, summary FROM ( SELECT comic_id, name, Image, author, trans, genre_id, summary, ROW_NUMBER() OVER (ORDER BY ( SELECT SUM(_view) FROM Chapter WHERE Chapter.comic_id = Comic.comic_id AND update_time BETWEEN GETDATE()-7 AND GETDATE()) desc) AS rownumber FROM Comic) AS foo WHERE rownumber <= " + end + " and rownumber >= " + begin;
                    comicDataSet = Connection.Connection.FillDataSet(query);
                }
                else if (key == 2)
                {
                    query = "WITH ABC(comic_id, name, sumview, Image, author, trans, genre_id, summary) AS( SELECT TOP " + end + " comic_id, name, sumview = (SELECT SUM(_view) FROM Chapter WHERE Chapter.comic_id = Comic.comic_id AND update_time BETWEEN GETDATE()-1 AND GETDATE()), Image, author, trans, genre_id, summary FROM Comic ORDER BY( SELECT SUM(_view) FROM Chapter WHERE Chapter.comic_id = Comic.comic_id AND update_time BETWEEN GETDATE()-1 AND GETDATE()) DESC) SELECT comic_id, name, Image, author, trans, genre_id, summary FROM ( SELECT TOP " + (end - begin + 1) + " comic_id, name, sumview, Image, author, trans, genre_id, summary FROM ABC ORDER BY sumview ASC) AS c ORDER BY sumview DESC";
                    comicDataSet = Connection.Connection.FillDataSet(query);
                }
                if (comicDataSet.Tables.Count <= 0)
                    return comics;
                for (int i = 0; i < comicDataSet.Tables[0].Rows.Count; i++)
                {
                    Comic comic = new Comic();
                    comic.name = comicDataSet.Tables[0].Rows[i]["name"].ToString();
                    comic.comic_id = Convert.ToInt32(comicDataSet.Tables[0].Rows[i]["comic_id"].ToString());
                    comic.genre_id = Convert.ToInt32(comicDataSet.Tables[0].Rows[i]["genre_id"].ToString());
                    comic.image = comicDataSet.Tables[0].Rows[i]["image"].ToString();
                    comic.author = comicDataSet.Tables[0].Rows[i]["author"].ToString();
                    comic.trans = comicDataSet.Tables[0].Rows[i]["trans"].ToString();
                    comic.summary = comicDataSet.Tables[0].Rows[i]["summary"].ToString();

                    List<Chapter> chapters = new List<Chapter>();
                    DataSet chapterDataSet = null;
                    string[] paras = new string[1] { "comic_id" };
                    object[] values = new object[1] { comic.comic_id };
                    query = "SELECT TOP 2 chapter_id, name, _view, update_time FROM Chapter WHERE comic_id = @comic_id ORDER BY name DESC";
                    chapterDataSet = Connection.Connection.FillDataSet(query, paras, values);
                    for (int j = 0; j < chapterDataSet.Tables[0].Rows.Count; j++)
                    {
                        Chapter chapter = new Chapter();
                        chapter.name = chapterDataSet.Tables[0].Rows[j]["name"].ToString();
                        chapter.chapter_id = Convert.ToInt32(chapterDataSet.Tables[0].Rows[j]["chapter_id"].ToString());
                        chapter.comic_id = comic.comic_id;
                        chapter.view = Convert.ToInt32(chapterDataSet.Tables[0].Rows[j]["_view"].ToString());
                        chapter.update_time = DateTime.Parse(chapterDataSet.Tables[0].Rows[j]["update_time"].ToString());
                        chapters.Add(chapter);
                    }
                    comic.chapters = chapters.ToArray();
                    comics.Add(comic);
                }
            }
            catch { }
            return comics;
        }
    }
}
