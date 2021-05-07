using ComicApiWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ComicApiWeb.Controllers
{
    public class ComicApiController : ApiController
    {
        //GET: api/ChapterApi/
        public int Get(string comic_idd)
        {
            int sum = 0;
            string[] paras = new string[1] { "comic_id" };
            object[] values = new object[1] { comic_idd };
            string query = "select sum(_view) from chapter where comic_id = @comic_id";
            sum = Int32.Parse(Connection.Connection.ExcuteScalar(query, paras, values));
            return sum;
        }

        // GET: api/ComicApi
        public IEnumerable<Comic> Get()
        {
            var comics = new List<Comic>();
            try
            {
                string query = "SELECT genre_id, name, summary, image, author, trans, comic_id FROM Comic";
                DataSet data = Connection.Connection.FillDataSet(query);
                if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                    {
                        Comic comic = new Comic();

                        comic.genre_id = Convert.ToInt32(data.Tables[0].Rows[i]["genre_id"].ToString());
                        comic.comic_id = Convert.ToInt32(data.Tables[0].Rows[i]["comic_id"].ToString());
                        comic.name = data.Tables[0].Rows[i]["name"].ToString();
                        comic.summary = data.Tables[0].Rows[i]["summary"].ToString();
                        comic.author = data.Tables[0].Rows[i]["author"].ToString();
                        comic.trans = data.Tables[0].Rows[i]["trans"].ToString();
                        comic.image = data.Tables[0].Rows[i]["image"].ToString();

                        string[] paras = new string[1] { "comic_id" };
                        object[] values = new object[1] { comic.comic_id };
                        query = "SELECT chapter_id, name, update_time, _view, comic_id FROM Chapter WHERE comic_id = @comic_id ORDER BY name ASC";
                        DataSet dsChapter = Connection.Connection.FillDataSet(query, paras, values);
                        List<Chapter> chapters = new List<Chapter>();
                        if (dsChapter.Tables.Count > 0 && dsChapter.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsChapter.Tables[0].Rows.Count; j++)
                            {
                                Chapter chapter = new Chapter();

                                chapter.chapter_id = Convert.ToInt32(dsChapter.Tables[0].Rows[j]["chapter_id"].ToString());
                                chapter.comic_id = comic.comic_id;
                                chapter.name = dsChapter.Tables[0].Rows[j]["name"].ToString();
                                chapter.update_time = Convert.ToDateTime(dsChapter.Tables[0].Rows[j]["update_time"].ToString());
                                chapter.view = Convert.ToInt32(dsChapter.Tables[0].Rows[j]["_view"].ToString());

                                chapters.Add(chapter);
                            }
                        }
                        comic.chapters = chapters.ToArray();

                        comics.Add(comic);
                    }

                }
            }
            catch { }
            return comics;
        }


        // GET: api/ComicApi/5
        public Comic Get(int comic_id)
        {
            Comic com = new Comic();
            try
            {
                string[] paras = new string[1] { "comic_id" };
                object[] values = new object[1] { comic_id };
                string query = "SELECT image, name, summary, trans, author, genre_id, comic_id FROM Comic WHERE comic_id = @comic_id";
                DataSet data = Connection.Connection.FillDataSet(query, paras, values);
                if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                {
                    com.comic_id = Convert.ToInt32(data.Tables[0].Rows[0]["comic_id"].ToString());
                    com.name = data.Tables[0].Rows[0]["name"].ToString();
                    com.summary = data.Tables[0].Rows[0]["summary"].ToString();
                    com.trans = data.Tables[0].Rows[0]["trans"].ToString();
                    com.author = data.Tables[0].Rows[0]["author"].ToString();
                    com.image = data.Tables[0].Rows[0]["image"].ToString();
                    com.genre_id = Convert.ToInt32(data.Tables[0].Rows[0]["genre_id"].ToString());
                }
                else { return com; }
                paras = new string[1] { "comic_id" };
                values = new object[1] { comic_id };
                query = "SELECT chapter_id, name, _view, update_time, comic_id FROM ( SELECT ROW_NUMBER() OVER (ORDER BY name DESC) AS rownumber, chapter_id, name, _view, update_time, comic_id FROM Chapter WHERE comic_id = " + comic_id + " ) AS foo WHERE rownumber <= 20 and rownumber >= 1";
                DataSet dataChapter = Connection.Connection.FillDataSet(query, paras, values);
                if (dataChapter.Tables.Count > 0 && dataChapter.Tables[0].Rows.Count > 0)
                {
                    List<Chapter> chapters = new List<Chapter>();
                    for (int i = 0; i < dataChapter.Tables[0].Rows.Count; i++)
                    {
                        Chapter chapter = new Chapter();

                        chapter.comic_id = comic_id;
                        chapter.chapter_id = Convert.ToInt32(dataChapter.Tables[0].Rows[i]["chapter_id"].ToString());
                        chapter.name = dataChapter.Tables[0].Rows[i]["name"].ToString();
                        chapter.update_time = Convert.ToDateTime(dataChapter.Tables[0].Rows[i]["update_time"].ToString());
                        chapter.view = Convert.ToInt32(dataChapter.Tables[0].Rows[i]["_view"].ToString());
                        chapters.Add(chapter);
                    }
                    com.chapters = chapters.ToArray();
                }
            }
            catch { }
            return com;
        }

        // GET: api/ComicApi/5
        public Comic Get(int comic_id, int begin, int end)
        {
            Comic com = new Comic();
            try
            {
                string[] paras;
                object[] values;
                string query;

                paras = new string[1] { "comic_id" };
                values = new object[1] { comic_id };
                query = "SELECT image, name, summary, trans, author, genre_id, comic_id FROM Comic WHERE comic_id = @comic_id";
                DataSet data = Connection.Connection.FillDataSet(query, paras, values);
                if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                {
                    com.comic_id = Convert.ToInt32(data.Tables[0].Rows[0]["comic_id"].ToString());
                    com.name = data.Tables[0].Rows[0]["name"].ToString();
                    com.summary = data.Tables[0].Rows[0]["summary"].ToString();
                    com.trans = data.Tables[0].Rows[0]["trans"].ToString();
                    com.author = data.Tables[0].Rows[0]["author"].ToString();
                    com.image = data.Tables[0].Rows[0]["image"].ToString();
                    com.genre_id = Convert.ToInt32(data.Tables[0].Rows[0]["genre_id"].ToString());
                }
                else { return com; }

                paras = new string[1] { "comic_id" };
                values = new object[1] { comic_id };
                query = "SELECT COUNT(chapter_id) FROM Chapter WHERE comic_id = @comic_id";
                int SumRecords = Int32.Parse(Connection.Connection.ExcuteScalar(query, paras, values));
                if (begin > end)
                    return com;
                if (begin > SumRecords)
                    return com;
                if (end > SumRecords)
                    end = SumRecords;

                paras = new string[1] { "comic_id" };
                values = new object[1] { comic_id };
                query = "SELECT chapter_id, name, _view, update_time, comic_id FROM ( SELECT ROW_NUMBER() OVER (ORDER BY name DESC) AS rownumber, chapter_id, name, _view, update_time, comic_id FROM Chapter WHERE comic_id = " + comic_id + " ) AS foo WHERE rownumber <= " + end + " and rownumber >= " + begin;
                DataSet dataChapter = Connection.Connection.FillDataSet(query, paras, values);
                if (dataChapter.Tables.Count > 0 && dataChapter.Tables[0].Rows.Count > 0)
                {
                    List<Chapter> chapters = new List<Chapter>();
                    for (int i = 0; i < dataChapter.Tables[0].Rows.Count; i++)
                    {
                        Chapter chapter = new Chapter();

                        chapter.comic_id = comic_id;
                        chapter.chapter_id = Convert.ToInt32(dataChapter.Tables[0].Rows[i]["chapter_id"].ToString());
                        chapter.name = dataChapter.Tables[0].Rows[i]["name"].ToString();
                        chapter.update_time = Convert.ToDateTime(dataChapter.Tables[0].Rows[i]["update_time"].ToString());
                        chapter.view = Convert.ToInt32(dataChapter.Tables[0].Rows[i]["_view"].ToString());

                        chapters.Add(chapter);
                    }
                    com.chapters = chapters.ToArray();
                }
            }
            catch { }
            return com;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comic"></param>
        /// <param name="KEY"></param>
        /// <returns>-1: VALIDATE_ERROR      -2: LOGIN_ERROR        -3: CREATE_FOLDER_ERROR
        /// -4: UPLOAD_IMAGE_ERROR      -5: INSERT_COMIC_TO_DB_ERROR</returns>
        // POST: api/ComicApi
        public int Post([FromBody] Comic comic, string KEY)
        {
            if (Userr.checkLogin(KEY) != 1)
                return -2;

            //validate
            if (string.IsNullOrEmpty(comic.name.Replace(" ", "")) || comic.genre_id == 0 || string.IsNullOrWhiteSpace(comic.image.Replace(" ", "")))
                return -1;

            //  Create folder
            string folderID = Drive.createFolder(comic.name, Drive.folderRootID);
            if (string.IsNullOrEmpty(folderID))
                return -3;

            //save image to server
            Image image = Base64String.Base64ToImage(comic.image);
            string folderPath = @"/Images/";
            var baseUrl = AppDomain.CurrentDomain.BaseDirectory + folderPath;
            string fileName = "jpg";
            string newFileName = Guid.NewGuid().ToString() + "." + fileName;
            string newPath = baseUrl + newFileName;

            var i2 = new Bitmap(image);
            i2.Save(newPath);

            //upload image to drive
            List<String> imageName = new List<string> { baseUrl + newFileName };
            if (!Drive.uploadFile(folderID, imageName.ToArray()))
            {
                Drive.deleteFolder(folderID);
                return -4;
            }
            string imageID = Drive.fileIDs.ToArray()[0];

            //save to db
            string[] paras = new string[7] { "name", "summary", "author", "trans", "genre_id", "image", "folderID" };
            object[] values = new object[7] { comic.name, comic.summary, comic.author, comic.trans, comic.genre_id, imageID, folderID };
            string query = "INSERT INTO Comic(name, summary, author, trans, genre_id, image, folderID) VALUES(@name, @summary, @author, @trans, @genre_id, @image, @folderID)";
            int result = Connection.Connection.ExcuteNonQuery(query, paras, values);
            if (result <= 0)
            {
                //failure
                Drive.deleteFolder(folderID);
                return -5;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comic"></param>
        /// <param name="KEY"></param>
        /// <returns>-1: VALIDATE_ERROR      -2: LOGIN_ERROR        -3: UPDATE_COMIC_NO_IMAGE_ERROR
        /// -4: UPDATE_COMIC_ERROR</returns>
        // PUT: api/ComicApi/5
        public int Put([FromBody] Comic comic, string KEY)
        {
            if (Userr.checkLogin(KEY) != 1)
                return -2;

            //validate
            if (string.IsNullOrEmpty(comic.name.Replace(" ", "")) || comic.genre_id == 0)
                return -1;

            string[] paras;
            object[] values;
            string query = "";
            int r = -1;
            //if (string.IsNullOrEmpty(comic.image.Replace(" ", "")))
            //{
            paras = new string[6] { "comic_id", "name", "summary", "author", "trans", "genre_id" };
            values = new object[6] { comic.comic_id, comic.name, comic.summary, comic.author, comic.trans, comic.genre_id };
            query = "UPDATE Comic SET name = @name, genre_id = @genre_id, summary = @summary, author = @author, trans = @trans WHERE Comic_id = @Comic_id";
            r = Connection.Connection.ExcuteNonQuery(query, paras, values);
            return r < 1 ? -3 : 0;
            //}
            //else
            //{
            //    //save new image
            //    string folderPath = @"/Images/";
            //    var baseUrl = AppDomain.CurrentDomain.BaseDirectory + folderPath;
            //    string fileName = "jpg";
            //    string newFileName = Guid.NewGuid().ToString() + "." + fileName;
            //    string newPath = baseUrl + newFileName;
            //    var list = comic.image.Split(',').ToList();
            //    File.WriteAllBytes(newPath, Convert.FromBase64String(list[list.Count - 1]));

            //    string folderID = Comic.getComicFolderID(comic.comic_id);
            //    if (string.IsNullOrEmpty(folderID))
            //        return -5;

            //    if (!Drive.uploadFile(folderID, new string[] { newFileName }))
            //        return -6;

            //    string imageID = Drive.fileIDs.ToArray()[0];

            //    //save comic
            //    paras = new string[7] { "comic_id", "name", "summary", "author", "trans", "genre_id", "image" };
            //    values = new object[7] { comic.comic_id, comic.name, comic.summary, comic.author, comic.trans, comic.genre_id, imageID };
            //    query = "UPDATE Comic SET name = @name, genre_id = @genre_id, summary = @summary, author = @author, trans = @trans, image = @image WHERE Comic_id = @Comic_id";
            //    r = Connection.Connection.ExcuteNonQuery(query, paras, values);
            //    return r < 1 ? -4 : 0;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comic_id"></param>
        /// <param name="KEY"></param>
        /// <returns>-1: VALIDATE_ERROR      -2: LOGIN_ERROR        -3: DELETE_ERROR</returns>
        // DELETE: api/ComicApi/5
        public int Delete(int comic_id, string KEY)
        {
            if (Userr.checkLogin(KEY) != 1)
                return -2;

            if (comic_id < 1)
                return -1;

            string folderID = Comic.getComicFolderID(comic_id);
            Drive.deleteFolder(folderID);

            string[] paras = new string[1] { "comic_id" };
            object[] values = new object[1] { comic_id };
            string query = "DELETE Comic WHERE comic_id = @comic_id";
            int r = -1;
            r = Connection.Connection.ExcuteNonQuery(query, paras, values);
            return r < 1 ? -3 : 0;
        }
    }
}
