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
        private readonly DBContext db = new DBContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Athleten
        public ActionResult Index()
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            Benutzer benutzer = (from b in db.Benutzer where b.Email == User.Identity.Name select b).Single();
            List<Startnummern> startnummern = (from s in db.Startnummern
                                               join a in db.Athleten on s.AthletId equals a.Id
                                               where s.WettkampfId == wettkampfId && a.VereinsId == benutzer.VereinId select s).ToList();
            List<AthletDaten> lst = new List<AthletDaten>();
            foreach (var nr in startnummern)
            {
                Athleten athlet = (from a in db.Athleten where a.Id == nr.AthletId select a).Single();
                AthletDaten data = new AthletDaten(athlet, nr.Startnummer);
                lst.Add(data);
            }
            
            return View(lst);
        }

        // GET: Athleten/Create
        public ActionResult Create()
        {
            AthletDaten data = new AthletDaten();
            int wettkampfId = GlobalVariables.WettkampfId;
            if (wettkampfId != 0)
            {
                var nr = (from s in db.Startnummern where s.WettkampfId == wettkampfId select s).FirstOrDefault();
                if (nr != null)
                {
                    data.Startnummer = (from s in db.Startnummern where s.WettkampfId == wettkampfId select s.Startnummer).Max() + 1;
                }
                else
                {
                    data.Startnummer = 1;
                }
               
            }
            else
            {
                data.Startnummer = (from s in db.Startnummern select s.Startnummer).Max() + 1;
            }
            return View(data);
        }

        // POST: Athleten/Create
        [HttpPost]
        public ActionResult Create(AthletDaten athletDaten)
        {
            // TODO: Add insert logic here
            int wettkampfId = GlobalVariables.WettkampfId;
            var startnummer = (from s in db.Startnummern where s.Startnummer == athletDaten.Startnummer && s.WettkampfId == wettkampfId select s).SingleOrDefault();

            if(startnummer == null && GlobalVariables.WettkampfId != 0)
            {
                athletDaten.New();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Startnummer", "Diese Startnummer ist bereits vergeben.");
            return View();

        }

        // GET: Athleten/Edit/5
        public ActionResult Edit(int id)
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            Athleten athlet = (from a in db.Athleten where a.Id == id select a).Single();
            Startnummern nr = (from s in db.Startnummern where s.AthletId == id && s.WettkampfId == wettkampfId select s).Single();
            return View(new AthletDaten(athlet, nr.Startnummer));
        }

        // POST: Athleten/Edit/5
        [HttpPost]
        public ActionResult Edit(AthletDaten athletDaten)
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            var startnummer = (from s in db.Startnummern where s.Startnummer == athletDaten.Startnummer && s.AthletId != athletDaten.Id && s.WettkampfId == wettkampfId
                               select s).SingleOrDefault();

            if (startnummer == null && GlobalVariables.WettkampfId != 0)
            {
                // TODO: Add update logic here
                athletDaten.Update();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Startnummer", "Diese Startnummer ist bereits vergeben.");
            return View();
        }

        // GET: Athleten/Delete/5
        public ActionResult Delete(int id)
        {
            Athleten athlet = (from a in db.Athleten where a.Id == id select a).Single();
            Startnummern nr = (from s in db.Startnummern
                               where s.AthletId == id
                               select s).Single();
            db.Startnummern.Remove(nr);
            db.Athleten.Remove(athlet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
