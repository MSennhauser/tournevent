using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class WettkampfController : Controller
    {

        // GET: Wettkampf/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wettkampf/Create
        [HttpPost]
        public ActionResult Create(WettkampfDaten wettkampfDaten)
        {
            if (ModelState.IsValid)
            {
                wettkampfDaten.New();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: Wettkampf/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Wettkampf/Edit/5
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

        // GET: Wettkampf/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Wettkampf/Delete/5
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
