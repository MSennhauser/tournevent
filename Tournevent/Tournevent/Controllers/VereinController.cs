using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VereinController : Controller
    {
        private readonly DataContext db = new DataContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Verein
        public ActionResult Index()
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            List<VereinsDaten> lst = new List<VereinsDaten>();
            List<Verein> vereinList = new List<Verein>();
            vereinList = (from v in db.Verein
                          join a in db.Anmeldung on v.ID_Verein equals a.ID_Verein
                          where a.ID_Wettkampf == wettkampfId
                          select v).ToList();
            foreach(var verein in vereinList)
            {
                /*Benutzer benutzer = (from b in db.Benutzer 
                                     where b.ID_Object == verein.ID_Verein select b).Single();
                Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher
                                                                   join b in db.Benutzer on v.ID_Vereinsverantwortlicher equals b.ID_Object
                                                                   where b.ID_Benutzer == benutzer.ID_Benutzer
                                                                   select v).SingleOrDefault();*/
                lst.Add(new VereinsDaten(verein));
            }
            return View(lst);
        }

        // GET: Verein/Create
        public ActionResult Create()
        {
            if (ModelState.IsValid)
            {
                List<Verein> vereinList = (from v in db.Verein select v).ToList();
                List<VereinWettkampf> vwList = new List<VereinWettkampf>();
                foreach (var v in vereinList)
                {
                    Benutzer benutzer = (from b in db.Benutzer where b.Rolle != "WartetAufBestaetigung" select b).SingleOrDefault();
                    if (benutzer != null)
                    {
                        List<int> id = (from a in db.Anmeldung where a.ID_Verein == v.ID_Verein select a.ID_Wettkampf).ToList();
                        vwList.Add(new VereinWettkampf(v, id));
                    }
                }
                return View(vwList);
            }
            return View();
        }

        // POST: Verein/Create
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
        public ActionResult Edit(int id)
        {
            Benutzer benutzer = (from b in db.Benutzer where b.ID_Benutzer == id select b).Single();
            VereinKontoDaten vkDaten = new VereinKontoDaten(benutzer);
            return View(vkDaten);
        }
        [HttpPost]
        public ActionResult Edit(VereinKontoDaten vkDaten)
        {
            if (ModelState.IsValid)
            {
                vkDaten.Update();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher
                                                               where v.Mailadresse == User.Identity.Name
                                                               select v).SingleOrDefault();
            // Statement könnte nicht funktionieren, da Benutzer noch überarbeitet werden muss
            List<Athlet> lstAthleten = (from a in db.Athlet where a.ID_Verein == vereinsverantwortlicher.ID_Verein select a).ToList();
            foreach(var athlet in lstAthleten)
            {
                db.Athlet.Remove(athlet);
            }
            
            Anmeldung anmeldung = (from a in db.Anmeldung
                                   where a.ID_Verein == vereinsverantwortlicher.ID_Verein
                                   select a).SingleOrDefault();
            db.Anmeldung.Remove(anmeldung);
            db.Verein.Remove(vereinsverantwortlicher.Verein);
            db.Vereinsverantwortlicher.Remove(vereinsverantwortlicher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Athleten(int id)
        {
            Benutzer benutzer = (from b in db.Benutzer where b.ID_Benutzer == id select b).Single();
            Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher
                                                               where v.Mailadresse == benutzer.Email
                                                               select v).SingleOrDefault();
            GlobalVariables.VereinsId = vereinsverantwortlicher.ID_Verein;
            return RedirectToAction("Overview", "Athleten", new { id = vereinsverantwortlicher.ID_Verein });
        }

        // GET: Verein/Edit/5
        public ActionResult Add(int id)
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            Anmeldung anmeldung = new Anmeldung();
            anmeldung.ID_Verein = id;
            anmeldung.ID_Wettkampf = wettkampfId;
            db.Anmeldung.Add(anmeldung);
            db.SaveChanges();
            return RedirectToAction("Create");
        }

        // GET: Verein/Delete/5
        public ActionResult Remove(int id)
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            Anmeldung anmeldung = (from a in db.Anmeldung where a.ID_Verein == id && a.ID_Wettkampf == wettkampfId select a).Single();
            db.Anmeldung.Remove(anmeldung);
            db.SaveChanges();
            return RedirectToAction("Create");
        }
    }
}
