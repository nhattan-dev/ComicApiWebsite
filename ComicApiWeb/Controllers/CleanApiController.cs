using ComicApiWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ComicApiWeb.Controllers
{
    public class CleanApiController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KEY"></param>
        /// <returns>-2: LOGIN_ERROR        -3: EXECUTE_ERROR</returns>
        // POST: api/CleanApi
        public int Post(string KEY)
        {
            if (Userr.checkLogin(KEY) == -1)
                return -2;

            //delete all items in Images folder
            string folderPath = @"/Images/";
            var baseUrl = AppDomain.CurrentDomain.BaseDirectory + folderPath;
            DirectoryInfo dir = new DirectoryInfo(baseUrl.Substring(0, baseUrl.Length - 1));
            foreach (FileInfo file in dir.GetFiles())
            {
                try { file.Delete(); } catch { return -3; };
            }
            return 0;
        }
    }
}
