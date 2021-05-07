using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ComicApiWeb.Models
{
    public class Genre
    {
        public int genre_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}