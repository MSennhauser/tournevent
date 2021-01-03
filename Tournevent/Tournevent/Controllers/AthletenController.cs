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
        // Gibt Alle Athleten im aktuellen Wettkampf zurück
        [Authorize(Roles = "Vereinsverantwortlicher")]
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
        [Authorize(Roles = "Administrator")]
        // Gibt Alle Athleten im aktuellen Wettkampf zurück
        public ActionResult Overview(int id)
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            List<Startnummern> startnummern = (from s in db.Startnummern
                                               join a in db.Athleten on s.AthletId equals a.Id
                                               where s.WettkampfId == wettkampfId && a.VereinsId == id
                                               select s).ToList();
            List<AthletDaten> lst = new List<AthletDaten>();
            foreach (var nr in startnummern)
            {
                Athleten athlet = (from a in db.Athleten where a.Id == nr.AthletId select a).Single();
                AthletDaten data = new AthletDaten(athlet, nr.Startnummer);
                lst.Add(data);
            }

            return View("Index", lst);
        }

        // Ein neuer Athlet kann hinzugefügt werden / Startnummer wird automatisch eingefügt
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

        // Der Athlet wird in die Datenbank geschrieben
        [HttpPost]
        public ActionResult Create(AthletDaten athletDaten)
        {
            if (ModelState.IsValid)
            {
                Athleten athlet = (from a in db.Athleten
                                   where a.Vorname == athletDaten.Vorname && a.Nachname == athletDaten.Nachname && a.Jahrgang == athletDaten.Jahrgang && a.VereinsId == GlobalVariables.VereinsId
                                   select a).SingleOrDefault();
                if (athlet == null)
                {
                    // TODO: Add insert logic here
                    int wettkampfId = GlobalVariables.WettkampfId;
                    var startnummer = (from s in db.Startnummern where s.Startnummer == athletDaten.Startnummer && s.WettkampfId == wettkampfId select s).SingleOrDefault();

                    if (startnummer == null && GlobalVariables.WettkampfId != 0)
                    {
                        athletDaten.New();
                        if (User.IsInRole("Administrator"))
                        {
                            return RedirectToAction("Overview", new { id = GlobalVariables.VereinsId });
                        }
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("Startnummer", "Diese Startnummer ist bereits vergeben.");
                }
                else
                {
                    ModelState.AddModelError("Vorname", "Dieser Athlet existiert bereits.");
                }
                
            }
            return View();

        }

        // Athlet kann Editiert werden
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
            if (ModelState.IsValid)
            {
                Athleten athlet = (from a in db.Athleten
                                   where a.Vorname == athletDaten.Vorname && a.Nachname == athletDaten.Nachname && a.Jahrgang == athletDaten.Jahrgang && a.VereinsId == GlobalVariables.VereinsId
                                   select a).SingleOrDefault();
                if (athlet == null)
                {
                    int wettkampfId = GlobalVariables.WettkampfId;
                    var startnummer = (from s in db.Startnummern
                                       where s.Startnummer == athletDaten.Startnummer && s.AthletId != athletDaten.Id && s.WettkampfId == wettkampfId
                                       select s).SingleOrDefault();

                    if (startnummer == null && GlobalVariables.WettkampfId != 0)
                    {
                        athletDaten.Update();
                        if (User.IsInRole("Administrator"))
                        {
                            return RedirectToAction("Overview", new { id = GlobalVariables.VereinsId });
                        }
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("Startnummer", "Diese Startnummer ist bereits vergeben.");
                }
                else
                {
                    ModelState.AddModelError("Vorname", "Dieser Athlet existiert bereits.");
                }
            }
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
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Overview", new { id = GlobalVariables.VereinsId });
            }
            return RedirectToAction("Index");
        }
    }
}
