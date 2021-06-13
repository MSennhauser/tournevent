﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DisziplinenController : Controller
    {
        private readonly DataContext db = new DataContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        private readonly List<Disziplin> disziplinList = new List<Disziplin>();
        // GET: Disziplinen
        public ActionResult Index()
        {
            int wettkampfID = GlobalData.currentWettkampf.ID_Wettkampf;
            List<Disziplin> disziplinen = (from d in db.Disziplin
                                           where d.ID_Wettkampf == wettkampfID
                                           select d).ToList();
            List<DisziplinenDaten> lst = new List<DisziplinenDaten>();
            foreach (var d in disziplinen)
            {
                lst.Add(new DisziplinenDaten(d));
            }
            return View(lst);
        }

        // GET: Disziplinen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Disziplinen/Create
        [HttpPost]
        public ActionResult Create(DisziplinenDaten data)
        {
            if (ModelState.IsValid)
            {
                data.New();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Disziplinen/Edit/5
        public ActionResult Edit(int id)
        {
            Disziplin disziplin = new Disziplin();
            int wettkampfID = GlobalData.currentWettkampf.ID_Wettkampf;
            using (DataContext db = new DataContext())
            {
                disziplin = (from d in db.Disziplin where d.ID_Disziplin == id select d).FirstOrDefault();
            }
            return View(new DisziplinenDaten(disziplin));
        }

        // POST: Disziplinen/Edit/5
        [HttpPost]
        public ActionResult Edit(DisziplinenDaten data)
        {
            if (ModelState.IsValid)
            {
                data.Update();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Disziplinen/Delete/5
        public ActionResult Delete(int id)
        {
            Disziplin disziplin = (from d in db.Disziplin where d.ID_Disziplin == id select d).FirstOrDefault();
            List<Wahldisziplin> wahldisziplinList = disziplin.Wahldisziplin.ToList();
            List<Kategorie_Disziplin> kategorieDisziplinList = (from d in db.Kategorie_Disziplin where d.ID_Disziplin == id select d).ToList();
            foreach(Wahldisziplin wahldisziplin in wahldisziplinList)
            {
                db.Wahldisziplin.Remove(wahldisziplin);
            }
            db.SaveChanges();
            foreach(Kategorie_Disziplin kategorieDisziplin in kategorieDisziplinList)
            {
                db.Kategorie_Disziplin.Remove(kategorieDisziplin);
                
            }
            db.SaveChanges();
            db.Disziplin.Remove(disziplin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Disziplin hinzufügen
        // GET: Verein/Edit/5
        public ActionResult Add(int disziplinId, int wahldisziplinId)
        {
            Wahldisziplin wahldisziplin = new Wahldisziplin();
            wahldisziplin.ID_Wahldisziplin = wahldisziplinId;
            wahldisziplin.ID_Disziplin = disziplinId;
            db.Wahldisziplin.Add(wahldisziplin);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = wahldisziplinId });
        }
        // Disziplin entfernen
        // GET: Verein/Delete/5
        public ActionResult Remove(int disziplinId, int wahldisziplinId)
        {
            Wahldisziplin wahldisziplin = (from w in db.Wahldisziplin where w.ID_Disziplin == disziplinId && w.ID_Wahldisziplin == wahldisziplinId select w).Single();
            db.Wahldisziplin.Remove(wahldisziplin);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = wahldisziplinId });
        }
    }
}
