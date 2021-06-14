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
        public ActionResult Index()
        {
            List<WettkampfDaten> wettkampfDatenList = new List<WettkampfDaten>();
            using (DataContext db = new DataContext())
            {
                List<Wettkampf> wettkampfList = (from w in db.Wettkampf select w).ToList();
                
                foreach(Wettkampf wettkampf in wettkampfList)
                {
                    wettkampfDatenList.Add(new WettkampfDaten(wettkampf));
                }
            }
            return View(wettkampfDatenList);
        }

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
                wettkampfDaten.New(User.Identity.Name);
                if (GlobalData.currentWettkampf != null)
                {
                    return RedirectToAction("Index");
                } else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        // GET: Wettkampf/Edit/5
        public ActionResult Edit(int id)
        {
            Wettkampf wettkampf = new Wettkampf();
            using (DataContext db = new DataContext())
            {
                wettkampf = (from w in db.Wettkampf where w.ID_Wettkampf == id select w).FirstOrDefault();
            }
            return View(new WettkampfDaten(wettkampf));
        }

        // POST: Wettkampf/Edit/5
        [HttpPost]
        public ActionResult Edit(WettkampfDaten wettkampfDaten)
        {
            if (ModelState.IsValid)
            {
                wettkampfDaten.Update();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Wettkampf/Delete/5
        public ActionResult Delete(int id)
        {
            // Add Delete when Kategorien und disziplinen bestehen
            using (DataContext db = new DataContext())
            { 
                Wettkampf wettkampf = (from w in db.Wettkampf where w.ID_Wettkampf == id select w).Single();
                List<Kategorie> kategorieList = wettkampf.Kategorie.ToList();
                List<Disziplin> disziplinList = wettkampf.Disziplin.ToList();
                List<Startnummer> startnummerList = wettkampf.Startnummer.ToList();
                List<Anmeldung> anmeldungList = wettkampf.Anmeldung.ToList();

                foreach(Kategorie kategorie in kategorieList)
                {
                    List<Kategorie_Disziplin> kategorieDisziplinList = kategorie.Kategorie_Disziplin.ToList();
                    foreach (Kategorie_Disziplin kategorieDisziplin in kategorieDisziplinList)
                    {
                        db.Kategorie_Disziplin.Remove(kategorieDisziplin);

                    }
                    db.SaveChanges();
                    db.Kategorie.Remove(kategorie);
                }
                db.SaveChanges();
                foreach (Disziplin disziplin in disziplinList)
                {
                    List<Wahldisziplin> wahldisziplinList = disziplin.Wahldisziplin.ToList();
                    foreach (Wahldisziplin wahldisziplin in wahldisziplinList)
                    {
                        db.Wahldisziplin.Remove(wahldisziplin);
                    }
                    db.SaveChanges();
                    db.Disziplin.Remove(disziplin);
                }
                db.SaveChanges();
                foreach (Startnummer startnummer in startnummerList)
                {
                    db.Startnummer.Remove(startnummer);
                }
                db.SaveChanges();
                foreach (Anmeldung anmeldung in anmeldungList)
                {
                    db.Anmeldung.Remove(anmeldung);
                }
                db.SaveChanges();
                db.Wettkampf.Remove(wettkampf);
                db.SaveChanges();

                Administrator administrator = (from a in db.Administrator
                                               where a.Mailadresse == User.Identity.Name
                                               select a).SingleOrDefault();
                List<Wettkampf> wList = (from w in db.Wettkampf
                                         where w.ID_Administrator == administrator.ID_Administrator
                                         orderby w.Datum descending
                                         select w).ToList();
                if (wList.Count > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    GlobalData.currentWettkampf = null;
                    return RedirectToAction("Create", "Wettkampf");
                }
            }
        }
    }
}
