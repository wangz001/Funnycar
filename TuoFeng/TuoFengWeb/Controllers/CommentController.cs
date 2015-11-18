using System;
using System.Web.Mvc;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace TuoFengWeb.Controllers
{
    public class CommentController : Controller
    {

        private readonly CommentBll _commentBll=new CommentBll();

        public ActionResult Index()
        {
            return null;
        }
        //
        // GET: /Comment/Details/5

        public ActionResult Details(int id)
        {
            return null;
        }

        //
        // GET: /Comment/Create

        public string Create(int travelPartId,int userId,int ? toUserId,string content)
        {
            if (travelPartId<=0||userId<=0||string.IsNullOrEmpty(content))
            {
                return HttpRequestResult.StateNotNull;
            }
            var model = new Comment
            {
                TravelPartId = travelPartId,
                UserId = userId,
                Content = content,
                CreateTime = DateTime.Now
            };
            if (toUserId!=null&&toUserId>0)
            {
                model.ToUserId = toUserId;
            }
            var result = _commentBll.Add(model);
            if (result>0)
            {
                return HttpRequestResult.StateOk;
            }
            return HttpRequestResult.StateError;
        }

        //
        // POST: /Comment/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return null;
            }
        }

        //
        // GET: /Comment/Edit/5

        public ActionResult Edit(int id)
        {
            return null;
        }

        //
        // POST: /Comment/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return null;
            }
        }

        //
        // GET: /Comment/Delete/5

        public ActionResult Delete(int id)
        {
            return null;
        }

        //
        // POST: /Comment/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return null;
            }
        }
    }
}
