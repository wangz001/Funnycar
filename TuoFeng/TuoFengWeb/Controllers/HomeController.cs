using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using TuoFeng.BLL;
using TuoFengWeb.Common;

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

        public string GetOssSecurityToken()
        {
            var credentials = OssAccessUtil.GetSecurityToken();
            if (credentials!=null)
            {
                var resultStr = new StringBuilder();
                resultStr.AppendFormat("{{\"AccessKeyId\": \"{0}\",",credentials.AccessKeyId);
                resultStr.AppendFormat("\"AccessKeySecret\": \"{0}\",",credentials.AccessKeySecret);
                resultStr.AppendFormat("\"Expiration\": \"{0}\",",credentials.Expiration);
                resultStr.AppendFormat("\"SecurityToken\": \"{0}\"}}",credentials.SecurityToken);
                return resultStr.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

}
}
