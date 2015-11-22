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


        // 取消赞
        // GET: /Thumb/Delete/5

        public string Delete(int travelPartId, int userId)
        {
            if (travelPartId > 0 && userId > 0)
            {
                var falg = _thumbBll.DeleteThumb(travelPartId, userId);
                if (falg)
                {
                    return HttpRequestResult.StateOk;
                }
            }
            return HttpRequestResult.StateError;
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
