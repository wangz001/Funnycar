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

        public ActionResult Login()
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public bool LoginIn(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
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

        public ActionResult Regist()
        {
            return View();
        }
        
        /// <summary>
        /// 获取oss临时访问权限，用于app客户端上传图片到oss
        /// </summary>
        /// <returns></returns>
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
