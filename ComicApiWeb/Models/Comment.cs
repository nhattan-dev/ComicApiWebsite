using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicApiWeb.Models
{
    public class Comment
    {
        public int cmt_id { get; set; }
        public string commentator { get; set; }
        public string cmt_content { get; set; }
        public DateTime cmt_time { get; set; }
        public int chapter_id { get; set; }
    }
}