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
        private readonly DBContext db = new DBContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Verein
        public ActionResult Index()
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            List<VereinsDaten> lst = new List<VereinsDaten>();
            List<Verein> vereinList = new List<Verein>();
            vereinList = (from v in db.Verein
                          join vw in db.VereineWettkampf on v.Index equals vw.VereinId
                          where vw.WettkampfId == wettkampfId
                          select v).ToList();
            foreach(var verein in vereinList)
            {
                Benutzer benutzer = (from b in db.Benutzer 
                                     where b.VereinId == verein.Index select b).Single();
                lst.Add(new VereinsDaten(benutzer, verein));
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
                    BenutzerRollen rollen = (from b in db.Benutzer join br in db.BenutzerRollen on b.Id equals br.BenutzerId where b.VereinId == v.Index && br.RollenId != 3 select br).SingleOrDefault();
                    if (rollen != null)
                    {
                        List<int> id = (from vw in db.VereineWettkampf where vw.VereinId == v.Index select vw.WettkampfId).ToList();
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
            Benutzer benutzer = (from b in db.Benutzer where b.Id == id select b).Single();
            VereinKontoDaten vkDaten = new VereinKontoDaten(benutzer, benutzer.Verein);
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
            Benutzer benutzer = (from b in db.Benutzer where b.Id == id select b).Single();
            List<Athleten> lstAthleten = (from a in db.Athleten where a.VereinsId == benutzer.VereinId select a).ToList();
            foreach(var athlet in lstAthleten)
            {
                db.Athleten.Remove(athlet);
            }
            BenutzerRollen benutzerRollen = (from b in db.BenutzerRollen
                                             where b.BenutzerId == benutzer.Id
                                             select b).SingleOrDefault();
            VereineWettkampf vereineWettkampf = (from vw in db.VereineWettkampf
                                             where vw.VereinId == benutzer.VereinId
                                             select vw).SingleOrDefault();
            db.VereineWettkampf.Remove(vereineWettkampf);
            db.Verein.Remove(benutzer.Verein);
            db.BenutzerRollen.Remove(benutzerRollen);
            db.Benutzer.Remove(benutzer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Athleten(int id)
        {
            Benutzer benutzer = (from b in db.Benutzer where b.Id == id select b).Single();
            GlobalVariables.VereinsId = (int)benutzer.VereinId;
            return RedirectToAction("Overview", "Athleten", new { id = benutzer.VereinId});
        }

        // GET: Verein/Edit/5
        public ActionResult Add(int id)
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            VereineWettkampf vw = new VereineWettkampf();
            vw.VereinId = id;
            vw.WettkampfId = wettkampfId;
            db.VereineWettkampf.Add(vw);
            db.SaveChanges();
            return RedirectToAction("Create");
        }

        // GET: Verein/Delete/5
        public ActionResult Remove(int id)
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            VereineWettkampf vw = (from v in db.VereineWettkampf where v.VereinId == id && v.WettkampfId == wettkampfId select v).Single();
            db.VereineWettkampf.Remove(vw);
            db.SaveChanges();
            return RedirectToAction("Create");
        }
    }
}
