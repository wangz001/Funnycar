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
                return "ok-->" + result;
            }
            return "error";
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            return null;
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return null;
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return null;
            }
            catch
            {
                return null;
            }
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            return null;
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
