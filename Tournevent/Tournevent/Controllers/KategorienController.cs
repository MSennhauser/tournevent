using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class KategorienController : Controller
    {
        private readonly DataContext db = new DataContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Kategorien
        public ActionResult Index()
        {
            int wettkampfID = GlobalData.currentWettkampf.ID_Wettkampf;
            List<Kategorie> kategorien = (from k in db.Kategorie
                                     where k.ID_Wettkampf == wettkampfID
                                     select k).ToList();
            List<KategorienDaten> lst = new List<KategorienDaten>();
            foreach(var k in kategorien)
            {
                lst.Add(new KategorienDaten(k));
            }
            return View(lst);
        }

        // GET: Kategorien/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Kategorien/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategorien/Create
        [HttpPost]
        public ActionResult Create(KategorienDaten data)
        {
            if (ModelState.IsValid)
            {
                data.New();
                return RedirectToAction("Index");
            }
            return View();
            
        }

        // GET: Kategorien/Edit/5
        public ActionResult Edit(int id)
        {
            Kategorie kategorie = new Kategorie();
            int wettkampfID = GlobalData.currentWettkampf.ID_Wettkampf;
            using (DataContext db = new DataContext())
            {
                kategorie = (from k in db.Kategorie where k.ID_Kategorie == id select k).FirstOrDefault();
            }
            return View(new KategorienDaten(kategorie));
        }

        // POST: Kategorien/Edit/5
        [HttpPost]
        public ActionResult Edit(KategorienDaten data)
        {
            if (ModelState.IsValid)
            {
                data.Update();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Kategorien/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Kategorien/Delete/5
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
