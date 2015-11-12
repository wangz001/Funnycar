using System.Web.Mvc;

namespace TuoFengWeb.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return null;
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

                return RedirectToAction("Index");
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

                return RedirectToAction("Index");
            }
            catch
            {
                return null;
            }
        }
    }
}
