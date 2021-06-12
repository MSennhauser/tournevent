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
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }
    }
}
