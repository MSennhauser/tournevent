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
            int wettkampfId = GlobalData.currentWettkampf.ID_Wettkampf;
            List<VereinKontoDaten> lst = new List<VereinKontoDaten>();
            List<Verein> vereinList = new List<Verein>();
            vereinList = (from v in db.Verein
                          join a in db.Anmeldung on v.ID_Verein equals a.ID_Verein
                          where a.ID_Wettkampf == wettkampfId
                          select v).ToList();
            
            foreach(Verein verein in vereinList)
            {
                Vereinsverantwortlicher vereinsverantwortlicher = (from ve in db.Vereinsverantwortlicher where ve.ID_Verein == verein.ID_Verein select ve).SingleOrDefault();
                Benutzer benutzer = (from b in db.Benutzer where b.Rolle != "WartetAufBestaetigung" && b.Email == vereinsverantwortlicher.Mailadresse select b).SingleOrDefault();
                VereinKontoDaten vereinKontoDaten = new VereinKontoDaten();
                vereinKontoDaten.KontoDaten = new KontoDaten(benutzer);
                vereinKontoDaten.VereinsverantwortlicherDaten = new VereinsverantwortlicherDaten(benutzer);
                vereinKontoDaten.VereinsDaten = new VereinsDaten(verein);
                lst.Add(vereinKontoDaten);
            }
            return View(lst);
        }

        // GET: Verein/Create
        public ActionResult Create()
        {
            if (ModelState.IsValid)
            {
                List<Verein> vereinList = (from v in db.Verein select v).ToList();
                /*
                List<VereinKontoDaten> vwList = new List<VereinKontoDaten>();
                foreach (Verein v in vereinList)
                {
                    Vereinsverantwortlicher vereinsverantwortlicher = (from ve in db.Vereinsverantwortlicher where ve.ID_Verein == ve.ID_Verein select ve).SingleOrDefault();
                    Benutzer benutzer = (from b in db.Benutzer where b.Rolle != "WartetAufBestaetigung" && b.Email == vereinsverantwortlicher.Mailadresse select b).SingleOrDefault();
                    VereinKontoDaten vereinKontoDaten = new VereinKontoDaten();
                    vereinKontoDaten.KontoDaten = new KontoDaten(benutzer);
                    vereinKontoDaten.VereinsverantwortlicherDaten = new VereinsverantwortlicherDaten(benutzer);
                    vereinKontoDaten.VereinsDaten = new VereinsDaten(v);
                    vereinKontoDaten.userId = benutzer.ID_Benutzer;
                    vwList.Add(vereinKontoDaten);
                }*/
                return View(vereinList);
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
            Benutzer benutzer = (from b in db.Benutzer where b.ID_Benutzer == id select b).SingleOrDefault();
            Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher where v.Mailadresse == benutzer.Email select v).SingleOrDefault();
            VereinKontoDaten vkDaten = new VereinKontoDaten();
            vkDaten.KontoDaten = new KontoDaten(benutzer);
            vkDaten.VereinsverantwortlicherDaten = new VereinsverantwortlicherDaten(vereinsverantwortlicher);
            vkDaten.VereinsDaten = new VereinsDaten(vereinsverantwortlicher.Verein);
            return View(vkDaten);
        }
        [HttpPost]
        public ActionResult Edit(VereinKontoDaten vkDaten)
        {
            if (ModelState.IsValid)
            {

                Benutzer email = (from b in db.Benutzer
                                    where b.ID_Benutzer != vkDaten.KontoDaten.userId && b.Email == vkDaten.KontoDaten.Email
                                    select b).SingleOrDefault();
                if (email == null)
                {
                    vkDaten.VereinsverantwortlicherDaten.Email = vkDaten.KontoDaten.Email;
                    vkDaten.VereinsverantwortlicherDaten.Update();
                    vkDaten.KontoDaten.Update();
                    vkDaten.VereinsDaten.Update();
                }
                else
                {
                    ModelState.AddModelError("KontoDaten.Email", "Diese Email Adresse existiert bereits.");
                }
                
            }
            return View(vkDaten);
        }

        public ActionResult Delete(int id)
        {
            Verein verein = (from v in db.Verein
                             where v.ID_Verein == id
                             select v).SingleOrDefault();
            List<Athlet> lstAthleten = (from a in db.Athlet where a.ID_Verein == verein.ID_Verein select a).ToList();
            foreach(var athlet in lstAthleten)
            {
                foreach(Startnummer nr in athlet.Startnummer)
                {
                    db.Startnummer.Remove(nr);
                }
                db.SaveChanges();
                db.Athlet.Remove(athlet);
            }
            
            Anmeldung anmeldung = (from a in db.Anmeldung
                                   where a.ID_Verein == verein.ID_Verein
                                   select a).SingleOrDefault();
            db.Anmeldung.Remove(anmeldung);
            List<Vereinsverantwortlicher> lstVereinsverantwortlicher = verein.Vereinsverantwortlicher.ToList();
            foreach(Vereinsverantwortlicher vereinsverantwortlicher in lstVereinsverantwortlicher)
            {
                db.Vereinsverantwortlicher.Remove(vereinsverantwortlicher);
            }
            db.SaveChanges();
            db.Verein.Remove(verein);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Athleten(int id)
        {
            Benutzer benutzer = (from b in db.Benutzer where b.ID_Benutzer == id select b).Single();
            Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher
                                                               where v.Mailadresse == benutzer.Email
                                                               select v).SingleOrDefault();
            GlobalData.verein = vereinsverantwortlicher.Verein;
            return RedirectToAction("Overview", "Athleten", new { id = vereinsverantwortlicher.ID_Verein });
        }
        
        //Wettkampf hinzufügen
        // GET: Verein/Edit/5
        public ActionResult Add(int id)
        {
            int wettkampfId = GlobalData.currentWettkampf.ID_Wettkampf;
            Anmeldung anmeldung = new Anmeldung();
            anmeldung.ID_Verein = id;
            anmeldung.ID_Wettkampf = wettkampfId;
            db.Anmeldung.Add(anmeldung);
            db.SaveChanges();
            return RedirectToAction("Create");
        }
        // Wettkampf entfernen
        // GET: Verein/Delete/5
        public ActionResult Remove(int id)
        {
            int wettkampfId = GlobalData.currentWettkampf.ID_Wettkampf;
            Anmeldung anmeldung = (from a in db.Anmeldung where a.ID_Verein == id && a.ID_Wettkampf == wettkampfId select a).Single();
            db.Anmeldung.Remove(anmeldung);
            db.SaveChanges();
            return RedirectToAction("Create");
        }
    }
}
