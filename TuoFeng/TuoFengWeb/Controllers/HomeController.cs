using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using TuoFeng.BLL;
using TuoFeng.Model;
using TuoFengWeb.Common;

namespace TuoFengWeb.Controllers
{
    public class HomeController : Controller
    {
        readonly UserBll _userBll = new UserBll();

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public string LoginIn(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return HttpRequestResult.StateError;
            }
            var exist = _userBll.Exists(userName);
            if (exist)
            {
                return HttpRequestResult.StateOk;
            }
            return HttpRequestResult.StateError;
        }

        public ActionResult Regist()
        {
            return View();
        }
        [HttpPost]
        public string RegistIn(FormCollection collection)
        {
            var userName = collection.Get("username");
            var password = collection.Get("password");
            var email = collection.Get("email");
            var phone = collection.Get("phone");
            if (string.IsNullOrEmpty(userName)||string.IsNullOrEmpty(password))
            {
                return HttpRequestResult.StateNotNull;
            }
            var user = new User
            {
                UserName = userName,
                PassWord = password,
                Email = email,
                PhoneNum = phone,
                IsEnable = true,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            if (_userBll.Exists(user.UserName))
            {
                return HttpRequestResult.StateExisted;
            }
            var flag = _userBll.Add(user);
            if (flag>0)
            {
                return HttpRequestResult.StateOk;
            }
            return HttpRequestResult.StateError;
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
