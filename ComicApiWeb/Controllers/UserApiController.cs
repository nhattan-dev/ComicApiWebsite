using ComicApiWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ComicApiWeb.Controllers
{
    public class UserApiController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>-1: LOGIN_ERROR        0: IS_TRANS     1: IS_ADMIN</returns>
        // GET: api/UserApi/5
        public int Get(string username, string password)
        {
            return Userr.checkLogin(username+password);
        }
    }
}
