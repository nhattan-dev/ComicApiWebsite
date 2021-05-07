using ComicApiWeb.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ComicApiWeb.Controllers
{
    public class ImageUploadController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="KEY"></param>
        /// <returns></returns>
        [HttpPost]
        public string ImageUpload(Base64String base64String, string KEY)
        {
            if (Userr.checkLogin(KEY) == -1)
                return "";

            List<string> base64Strings = base64String.base64String.Split('.').ToList();
            List<string> urls = new List<string>();
            foreach (string base64 in base64Strings)
            {
                string imageUrl = "";
                try
                {
                    string folderPath = @"/Images/";
                    var baseUrl = AppDomain.CurrentDomain.BaseDirectory + folderPath;
                    string fileName = "jpg";
                    string newFileName = Guid.NewGuid().ToString() + "." + fileName;
                    string newPath = baseUrl + newFileName;
                    var list = base64.Split(',').ToList();
                    File.WriteAllBytes(newPath, Convert.FromBase64String(list[list.Count-1]));
                    //SaveJpeg(newPath, image, 50);
                    imageUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + folderPath + newFileName;
                    urls.Add(imageUrl);
                }
                catch (Exception e) { return e.Message; }
            }
            return urls.Aggregate((a, b) => a + " " + b);
        }
    }
}
