using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace TuoFengWeb.Controllers
{
    public class ThumbController : Controller
    {
        private readonly ThumbBll _thumbBll=new ThumbBll();
        //
        // GET: /Thumb/

        public ActionResult Index()
        {
            return null;
        }

        //
        // GET: /Thumb/Details/5

        public ActionResult Details(int id)
        {
            return null;
        }

        //
        // GET: /Thumb/Create

        public string Create(int travelPartId,int userId)
        {
            if (travelPartId>0&&userId>0)
            {
                var model = new Thumb
                {
                    TravelPartId = travelPartId,
                    UserId = userId,
                    IsDelete = false,
                    CreateTime = DateTime.Now
                };
                var flag = _thumbBll.Add(model);
                if (flag>0)
                {
                    return HttpRequestResult.StateOk;
                }
            }
            return HttpRequestResult.StateError;
        }

        //
        // POST: /Thumb/Create

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
        // GET: /Thumb/Edit/5

        public ActionResult Edit(int id)
        {
            return null;
        }

        //
        // POST: /Thumb/Edit/5

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
        // GET: /Thumb/Delete/5

        public ActionResult Delete(int id)
        {
            return null;
        }

        //
        // POST: /Thumb/Delete/5

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
