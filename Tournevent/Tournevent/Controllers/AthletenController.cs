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
        private readonly DataContext db = new DataContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // Gibt Alle Athleten im aktuellen Wettkampf zurück
        [Authorize(Roles = "Vereinsverantwortlicher")]
        public ActionResult Index()
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher where v.Mailadresse == User.Identity.Name select v).Single();
            List<Startnummer> startnummerList = (from s in db.Startnummer
                                               join a in db.Athlet on s.ID_Athlet equals a.ID_Athlet
                                               where s.ID_Wettkampf == wettkampfId && a.ID_Verein == vereinsverantwortlicher.ID_Verein select s).ToList();
            List<AthletDaten> lst = new List<AthletDaten>();
            foreach (Startnummer nr in startnummerList)
            {
                Athlet athlet = (from a in db.Athlet where a.ID_Athlet == nr.ID_Athlet select a).Single();
                AthletDaten data = new AthletDaten(athlet, nr.Startnr);
                lst.Add(data);
            }
            
            return View(lst);
        }
        [Authorize(Roles = "Administrator")]
        // Gibt Alle Athleten im aktuellen Wettkampf zurück
        public ActionResult Overview(int id)
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            List<Startnummer> startnummern = (from s in db.Startnummer
                                               join a in db.Athlet on s.ID_Athlet equals a.ID_Athlet
                                               where s.ID_Wettkampf == wettkampfId && a.ID_Verein == id
                                               select s).ToList();
            List<AthletDaten> lst = new List<AthletDaten>();
            foreach (var nr in startnummern)
            {
                Athlet athlet = (from a in db.Athlet where a.ID_Athlet == nr.ID_Athlet select a).Single();
                AthletDaten data = new AthletDaten(athlet, nr.Startnr);
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
                var nr = (from s in db.Startnummer where s.ID_Wettkampf == wettkampfId select s).FirstOrDefault();
                if (nr != null)
                {
                    data.Startnummer = (from s in db.Startnummer where s.ID_Wettkampf == wettkampfId select s.Startnr).Max() + 1;
                }
                else
                {
                    data.Startnummer = 1;
                }

            }
            else
            {
                data.Startnummer = (from s in db.Startnummer select s.Startnr).Max() + 1;
            }
            return View(data);

        }

        // Der Athlet wird in die Datenbank geschrieben
        [HttpPost]
        public ActionResult Create(AthletDaten athletDaten)
        {
            if (ModelState.IsValid)
            {
                Athlet athlet = (from a in db.Athlet
                                   where a.Vorname == athletDaten.Vorname && a.Nachname == athletDaten.Nachname && a.Geburtsdatum == athletDaten.Geburtsdatum && a.ID_Verein == GlobalVariables.VereinsId
                                   select a).SingleOrDefault();
                if (athlet == null)
                {
                    // TODO: Add insert logic here
                    int wettkampfId = GlobalVariables.WettkampfId;
                    var startnummer = (from s in db.Startnummer where s.Startnr == athletDaten.Startnummer && s.ID_Wettkampf == wettkampfId select s).SingleOrDefault();

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
            Athlet athlet = (from a in db.Athlet where a.ID_Athlet == id select a).Single();
            Startnummer nr = (from s in db.Startnummer where s.ID_Athlet == id && s.ID_Wettkampf == wettkampfId select s).Single();
            return View(new AthletDaten(athlet, nr.Startnr));
        }

        // POST: Athleten/Edit/5
        [HttpPost]
        public ActionResult Edit(AthletDaten athletDaten)
        {
            if (ModelState.IsValid)
            {
                Athlet athlet = (from a in db.Athlet
                                   where a.Vorname == athletDaten.Vorname && a.Nachname == athletDaten.Nachname && a.Geburtsdatum == athletDaten.Geburtsdatum && a.ID_Verein == GlobalVariables.VereinsId
                                   select a).SingleOrDefault();
                if (athlet == null)
                {
                    int wettkampfId = GlobalVariables.WettkampfId;
                    var startnummer = (from s in db.Startnummer
                                       where s.Startnr == athletDaten.Startnummer && s.ID_Athlet != athletDaten.Id && s.ID_Wettkampf == wettkampfId
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
            Athlet athlet = (from a in db.Athlet where a.ID_Athlet == id select a).Single();
            Startnummer nr = (from s in db.Startnummer
                               where s.ID_Athlet == id
                               select s).Single();
            db.Startnummer.Remove(nr);
            db.Athlet.Remove(athlet);
            db.SaveChanges();
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Overview", new { id = GlobalVariables.VereinsId });
            }
            return RedirectToAction("Index");
        }
    }
}
