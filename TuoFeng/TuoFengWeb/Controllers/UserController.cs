using System;
using System.Web.Mvc;
using TuoFeng.BLL;
using TuoFeng.Model;
using TuoFengWeb.UserAuthorize;

namespace TuoFengWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly UserBll _userBll = new UserBll();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Setting(int userid=0)
        {
            var user = _userBll.GetModelByCache(userid);
            ViewBag.User = user;
            return View();
        }
        /// <summary>
        /// 用户详细信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDetail(int userid)
        {
            var user = _userBll.GetModelByCache(userid);
            ViewBag.User = user;
            return View();
        }

        public string Regist(string userName, string passWord)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                return "用户名不能为空";
            }
            var exist = _userBll.Exists(userName);
            if (exist)
            {
                return "用户名已存在";
            }
            var model = new User
            {
                UserName = userName,
                PassWord = passWord,
                IsEnable = true,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            var result = _userBll.Add(model);
            if (result>0)
            {
                Session.Add("user",model);
                return HttpRequestResult.StateOk;
            }
            return HttpRequestResult.StateError;
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            return null;
        }

        public string SetUserDetail(FormCollection collection)
        {
            var userId = collection.Get("userid");
            var showName = collection.Get("showname");
            var email = collection.Get("email");
            var phone = collection.Get("phone");
            var sex = collection.Get("sex");
            var user = new User()
            {
                Id = Int32.Parse(userId),
                ShowName = showName,
                Email = email,
                PhoneNum = phone,
                Sex = Int32.Parse(sex) > 0,
                UpdateTime = DateTime.Now
            };
            var flag = _userBll.SetUserDetail(user);
            if (flag)
            {
                return HttpRequestResult.StateOk;
            }
            else
            {
                return HttpRequestResult.StateError;
            }
        }


        public ActionResult SetImage(int userid)
        {
            var user = _userBll.GetModelByCache(userid);
            ViewBag.User = user;
            return View();
        }
        public string SetHeadImage(FormCollection collection)
        {
            var userId = collection.Get("userid");
            var imgUrl = collection.Get("imgurl");
            if (string.IsNullOrEmpty(userId)||string.IsNullOrEmpty(imgUrl))
            {
                return HttpRequestResult.StateNotNull;
            }
            var flag = _userBll.SetHeadImage(Int32.Parse(userId), imgUrl);
            if (flag)
            {
                return HttpRequestResult.StateOk;
            }
            return HttpRequestResult.StateError;
        }
        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                return null;
            }
            catch
            {
                return null;
            }
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            return null;
        }

        //
        // POST: /User/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
