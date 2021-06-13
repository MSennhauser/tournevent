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
            KategorienDaten kategorienDaten = new KategorienDaten();
            return View(kategorienDaten);
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
            Kategorie kategorie = (from k in db.Kategorie where k.ID_Kategorie == id select k).FirstOrDefault();
            List<Kategorie_Disziplin> kategorieDisziplinList = (from d in db.Kategorie_Disziplin where d.ID_Kategorie == id select d).ToList();
            foreach (Kategorie_Disziplin kategorieDisziplin in kategorieDisziplinList)
            {
                db.Kategorie_Disziplin.Remove(kategorieDisziplin);

            }
            db.SaveChanges();
            db.Kategorie.Remove(kategorie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Disziplin hinzufügen
        // GET: Verein/Edit/5
        public ActionResult Add(int disziplinId, int kategorieId)
        {
            Kategorie_Disziplin kategorieDisziplin = new Kategorie_Disziplin();
            kategorieDisziplin.ID_Kategorie = kategorieId;
            kategorieDisziplin.ID_Disziplin = disziplinId;
            db.Kategorie_Disziplin.Add(kategorieDisziplin);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = kategorieId });
        }
        // Disziplin entfernen
        // GET: Verein/Delete/5
        public ActionResult Remove(int disziplinId, int kategorieId)
        {
            Kategorie_Disziplin kategorieDisziplin = (from k in db.Kategorie_Disziplin where k.ID_Disziplin == disziplinId && k.ID_Kategorie == kategorieId select k).Single();
            db.Kategorie_Disziplin.Remove(kategorieDisziplin);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = kategorieId });
        }
    }
}
