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
    public class GenreApiController : ApiController
    {
        // GET: api/GenreApi
        public IEnumerable<Genre> Get()
        {
            var gens = new List<Genre>();
            try
            {
                DataSet data = Connection.Connection.FillDataSet("SELECT genre_id, name, description FROM Genre");
                if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                        {
                            Genre gen = new Genre();

                            gen.genre_id = Convert.ToInt32(data.Tables[0].Rows[i]["genre_id"].ToString());
                            gen.name = data.Tables[0].Rows[i]["name"].ToString();
                            gen.description = data.Tables[0].Rows[i]["description"].ToString();

                            gens.Add(gen);
                        }
                    }
                    catch { return gens; }

                }
            }
            catch { }
            return gens;
        }

        // GET: api/GenreApi/5
        public Genre Get(int genre_id)
        {
            var genre = new Genre();
            string[] paras = new string[1] { "genre_id" };
            object[] values = new object[1] { genre_id };
            string query = "SELECT genre_id, name, description FROM Genre WHERE genre_id = @genre_id";
            DataSet data = Connection.Connection.FillDataSet(query, paras, values);

            try
            {
                if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                {
                    genre.genre_id = Convert.ToInt32(data.Tables[0].Rows[0]["genre_id"].ToString());
                    genre.name = data.Tables[0].Rows[0]["name"].ToString();
                    genre.description = data.Tables[0].Rows[0]["description"].ToString();
                }
            }
            catch { return genre; }

            return genre;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="KEY"></param>
        /// <returns>-1: VALIDATE_ERROR      -2: LOGIN_ERROR        -3: INSERT_TO_DB_ERROR</returns>
        // POST: api/GenreApi
        public int Post([FromBody] Genre genre, string KEY)
        {
            if (Userr.checkLogin(KEY) != 1)
                return -2;

            if (string.IsNullOrWhiteSpace(genre.name.Replace(" ","")))
                return -1;

            //create new genre
            string[] paras = new string[2] { "name", "description" };
            object[] values = new object[2] { genre.name, genre.description };
            string query = "INSERT INTO Genre(name, description) VALUES(@name, @description)";
            int result = Connection.Connection.ExcuteNonQuery(query, paras, values);
            return result < 1 ? -3 : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="KEY"></param>
        /// <returns>-1: VALIDATE_ERROR      -2: LOGIN_ERROR        -3: UPDATE_ERROR</returns>
        // PUT: api/GenreApi/5
        public int Put([FromBody] Genre genre, string KEY)
        {
            if (Userr.checkLogin(KEY) != 1)
                return -2;

            if (genre.genre_id < 1 || string.IsNullOrEmpty(genre.name.Replace(" ", "")))
                return -1; 

            string[] paras = new string[3] { "genre_id", "name", "description" };
            object[] values = new object[3] { genre.genre_id, genre.name, genre.description };
            string query = "UPDATE Genre SET name = @name, description = @description WHERE genre_id = @genre_id";
            int result = Connection.Connection.ExcuteNonQuery(query, paras, values);
            return result < 1 ? -3 : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genre_id"></param>
        /// <param name="KEY"></param>
        /// <returns>-1: VALIDATE_ERROR      -2: LOGIN_ERROR        -3: DELETE_ERROR</returns>
        // DELETE: api/GenreApi/5
        public int Delete(int genre_id, string KEY)
        {
            if (Userr.checkLogin(KEY) != 1)
                return -2;

            if (genre_id < 1) return -1;

            string[] paras = new string[1] { "genre_id" };
            object[] values = new object[1] { genre_id };
            string query = "DELETE Genre WHERE genre_id = @genre_id";
            int result = Connection.Connection.ExcuteNonQuery(query, paras, values);
            return result < 1 ? -3 : 0;
        }
    }
}
