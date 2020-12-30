using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles = "Administrator,Vereinsverantwortlicher")]
    public class AthletenController : Controller
    {
        private readonly Entities db = new Entities();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Athleten
        public ActionResult Index()
        {
            int wettkampfId = CurrentWettkampf.Id;
            Startnummern startnummern = (from s in db.Startnummern where s.WettkampfId == wettkampfId select s).Single();

            Athleten athlet = (from a in db.Athleten where a.Id == startnummern.AthletId select a).Single();
            AthletDaten data = new AthletDaten(athlet, startnummern.Startnummer);
            return View(data);
        }

        // GET: Athleten/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Athleten/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Athleten/Create
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

        // GET: Athleten/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Athleten/Edit/5
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

        // GET: Athleten/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Athleten/Delete/5
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
