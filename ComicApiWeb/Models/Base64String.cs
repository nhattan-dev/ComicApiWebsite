using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ComicApiWeb.Models
{
    public class Base64String
    {
        public string base64String { get; set; }

        public static Image Base64ToImage(string base64String)
        {
            Image image = null;
            try {
                var str = base64String.Split('/').ToList();
                str.RemoveAt(0);
                str.RemoveAt(0);
                var joinedNames = "/" + str.Aggregate((a, b) => a + "/" + b);
                byte[] bytes = Convert.FromBase64String(joinedNames);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }
            }
            catch { }
            return image;
        }
        public static string ImageToBase64(Image image, ImageFormat format)
        {
            string base64String;
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                ms.Position = 0;
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                base64String = Convert.ToBase64String(imageBytes);
            }
            return base64String;
        }
    }
}