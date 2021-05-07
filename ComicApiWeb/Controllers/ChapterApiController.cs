using ComicApiWeb.Models;
using System;
using System.Collections;
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
    public class ChapterApiController : ApiController
    {

        // GET: api/ChapterApi/5
        public Chapter Get(int chapter_id)
        {
            Chapter chapter = new Chapter();
            try
            {
                //inc view
                string[] paras = new string[1] { "chapter_id" };
                object[] values = new object[1] { chapter_id };
                string query = "UPDATE Chapter SET _view = _view + 1 WHERE chapter_id = @chapter_id";
                Connection.Connection.RequestStatus(query, paras, values);
                //init

                //get info chapter
                DataSet chapterDataSet = null;
                paras = new string[1] { "chapter_id" };
                values = new object[1] { chapter_id };
                query = "SELECT name, _view, update_time, comic_id FROM Chapter WHERE chapter_id = @chapter_id";
                chapterDataSet = Connection.Connection.FillDataSet(query, paras, values);
                if (chapterDataSet.Tables[0].Rows.Count == 0)
                    return null;
                chapter.name = chapterDataSet.Tables[0].Rows[0]["name"].ToString();
                chapter.view = Convert.ToInt32(chapterDataSet.Tables[0].Rows[0]["_view"].ToString());
                chapter.update_time = Convert.ToDateTime(chapterDataSet.Tables[0].Rows[0]["update_time"].ToString());
                chapter.comic_id = Convert.ToInt32(chapterDataSet.Tables[0].Rows[0]["comic_id"].ToString());
                chapter.chapter_id = chapter_id;

                //get images of chapter
                paras = new string[1] { "chapter_id" };
                values = new object[1] { chapter_id };
                DataSet base64StringDataSet = null;
                query = "SELECT link FROM Image WHERE chapter_id = @chapter_id  ORDER BY image_id";
                base64StringDataSet = Connection.Connection.FillDataSet(query, paras, values);

                //add to list
                List<String> links = new List<string>();
                if (base64StringDataSet.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < base64StringDataSet.Tables[0].Rows.Count; i++)
                    {
                        links.Add(base64StringDataSet.Tables[0].Rows[i]["link"].ToString());
                    }
                    chapter.link = links.ToArray();
                }
            }
            catch { }
            return chapter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="KEY"></param>
        /// <returns>-1: VALIDATE_ERROR      -2: LOGIN_ERROR        -3: CREATE_FOLDER_ERROR     -4: UPLOAD_IMAGE_ERROR
        /// -5 -6: INSERT_CHAPTER_TO_DB_ERROR      -7: INSERT_IMAGE_TO_DB_ERROR</returns>
        // POST: api/ChapterApi
        public int Post([FromBody] Chapter chapter, string KEY)
        {
            if (Userr.checkLogin(KEY) == -1)
                return -2;

            //move file
            //AppDomain.CurrentDomain.BaseDirectory + folderPath

            /*
             * 
             * read image from Images folder
             * convert to base64
             * save to db
             * 
             */

            //validate
            if (string.IsNullOrEmpty(chapter.name.Replace(" ", "")) || chapter.link.Length == 0)
                return -1;

            try
            {
                string folderPath = @"Images\";
                var baseUrl = AppDomain.CurrentDomain.BaseDirectory + folderPath;

                //get Image Names from chapter.link
                List<String> imageName = new List<string>();
                foreach (string link in chapter.link)
                {
                    imageName.Add(baseUrl + link.Split('/')[link.Split('/').Length - 1]);
                }


                //  Create folder
                string parentFolderID = Chapter.getParentChapterFolderID(chapter.comic_id);
                if (string.IsNullOrEmpty(parentFolderID)) return -3;
                string folderID = Drive.createFolder(chapter.name, parentFolderID);

                //  Upload to Drive
                if (!Drive.uploadFile(folderID, imageName.ToArray()))
                {
                    Drive.deleteFolder(folderID);
                    return -4;
                }

                //create new chapter
                string[] paras = new string[3] { "name", "comic_id", "folderID" };
                object[] values = new object[3] { chapter.name, chapter.comic_id, folderID };
                string query = "INSERT INTO Chapter(name, comic_id, folderID) VALUES(@name, @comic_id, @folderID) SELECT MAX(chapter_id) AS chapter_id FROM Chapter";
                DataSet ds = Connection.Connection.FillDataSet(query, paras, values);
                int chapter_id = -1;
                try
                {
                    chapter_id = Convert.ToInt32(ds.Tables[0].Rows[0]["chapter_id"].ToString());
                }
                catch
                {
                    Drive.deleteFolder(folderID); 
                    return -5;
                }
                if (chapter_id == -1)
                {
                    Drive.deleteFolder(folderID);
                    return -6;
                }

                //create image
                int image_request;
                foreach (string fileID in Drive.fileIDs)
                {
                    paras = new string[2] { "link", "chapter_id" };
                    values = new object[2] { fileID, chapter_id };
                    query = "INSERT INTO Image(link, chapter_id) VALUES(@link, @chapter_id)";
                    image_request = Connection.Connection.ExcuteNonQuery(query, paras, values);
                    if (image_request <= 0)
                    {
                        //failure
                        //call delete  chapter
                        paras = new string[1] { "chapter_id" };
                        values = new object[1] { chapter_id };
                        query = "DELETE Chapter WHERE chapter_id = @chapter_id";
                        Connection.Connection.ExcuteNonQuery(query, paras, values);
                        Drive.deleteFolder(folderID);
                        return -7;
                    }
                }
            }
            catch { return -8; }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chapter_id"></param>
        /// <param name="KEY"></param>
        /// <returns>-1: VALIDATE_ERROR      -2: LOGIN_ERROR        -3: EXECUTE_ERROR</returns>
        // DELETE: api/ChapterApi/5
        public int Delete(int chapter_id, string KEY)
        {
            if (Userr.checkLogin(KEY) == -1)
                return -2;

            if (chapter_id < 1)
                return -1;

            string folderID = Chapter.getChapterFolderID(chapter_id);
            Drive.deleteFolder(folderID);

            string[] paras = new string[1] { "chapter_id" };
            object[] values = new object[1] { chapter_id };
            string query = "DELETE Chapter WHERE chapter_id = @chapter_id";
            int result = Connection.Connection.ExcuteNonQuery(query, paras, values);
            return result < 1 ? -3 : 0;
        }
    }
}
