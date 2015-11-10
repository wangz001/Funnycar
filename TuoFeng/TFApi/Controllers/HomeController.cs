using System;
using System.Web.Mvc;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace TFApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserBll _userBll = new UserBll();

        public ActionResult Index()
        {
            return View();
        }

        public string Login(string userName,string passWord)
        {
            var userInfo = new UserBll().GetModel(1);
            return userInfo.Id+userInfo.UserName;
        }

        public string Regist(string userName, string passWord)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                return "用户名或密码不能为空";
            }
            if (_userBll.Exists(userName))
            {
                return "用户名已被注册,请尝试别的名字";
            }
            var user = new User
            {
                UserName = userName,
                PassWord = passWord,
                IsEnable = true,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            var userid = _userBll.Add(user);
            return (userid > 0).ToString();
        }
    }
}
