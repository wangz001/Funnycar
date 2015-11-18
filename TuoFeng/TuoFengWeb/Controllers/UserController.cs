using System;
using System.Web.Mvc;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace TuoFengWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly UserBll _userBll = new UserBll();

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


        public string SetHeadImage(FormCollection collection)
        {
            var userId = collection.Get("userId");
            var imgUrl = collection.Get("imgUrl");
            if (string.IsNullOrEmpty(userId)||string.IsNullOrEmpty(imgUrl))
            {
                return "图片地址不能为空";
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
