using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tournevent.Controllers
{
    public class VereinController : Controller
    {
        // GET: Verein
        public ActionResult Index()
        {
            return View();
        }

        // GET: Verein/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Verein/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Verein/Create
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
                return View();
            }
        }

        // GET: Verein/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Verein/Edit/5
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
                return View();
            }
        }

        // GET: Verein/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Verein/Delete/5
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
                return View();
            }
        }
    }
}
