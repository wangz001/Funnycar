using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using TuoFeng.BLL;

namespace TuoFengWeb.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {

            return View();
        }

        public bool Login(string userName, string passWord)
        {
            if (string.IsNullOrEmpty(userName)||string.IsNullOrEmpty(passWord))
            {
                return false;
            }
            var userBll = new UserBll();

            var exist = userBll.Exists(userName);
            if (exist)
            {
                return true;
            }
            return false;
        }

}
}
