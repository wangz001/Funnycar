using System;
using System.Web.Mvc;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace TuoFengWeb.Controllers
{
    public class TravelsController : Controller
    {
        //
        // GET: /Travels/
        private readonly TravelsBll _travelsBll=new TravelsBll();

        public ActionResult Index()
        {
            return null;
        }

        public string Add(string userId,string travelName)
        {
            if (string.IsNullOrEmpty(userId)||string.IsNullOrEmpty(travelName))
            {
                return HttpRequestResult.StateNotNull;
            }
            int useridInt;
            if (Int32.TryParse(userId,out useridInt)&&useridInt>0)
            {
                var model = new Travels
                {
                    TravelName = travelName,
                    UserId = useridInt,
                    IsDelete = false,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                var flag = _travelsBll.Add(model);
                return flag > 0 ? HttpRequestResult.StateOk : HttpRequestResult.StateError;
            }
            return HttpRequestResult.StateError;
        }

        public string SetCoverImage(int travelId,string imgUrl)
        {

            return null;
        }

        //
        // GET: /Travels/Details/5

        public ActionResult Details(int id)
        {
            return null;
        }

        //
        // GET: /Travels/Create

        public ActionResult Create()
        {
            return null;
        }

        //
        // POST: /Travels/Create

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
        // GET: /Travels/Edit/5

        public ActionResult Edit(int id)
        {
            return null;
        }

        //
        // POST: /Travels/Edit/5

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
        // GET: /Travels/Delete/5

        public ActionResult Delete(int id)
        {
            return null;
        }

        //
        // POST: /Travels/Delete/5

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
