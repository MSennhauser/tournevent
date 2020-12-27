using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AnfrageController : Controller
    {
        private readonly Entities db = new Entities();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Anfrage
        public ActionResult Index()
        {
            List<VereinsDaten> lst = new List<VereinsDaten>();
            var benutzerIds = from o in db.Rollen
                                join r in db.BenutzerRollen on o.Id equals r.RollenId
                                where o.Rolle == "WartetAufBestaetigung"
                                select r.BenutzerId;

            Debug.WriteLine(benutzerIds);

            foreach(var item in benutzerIds)
            {
                Verein verein = (from v in db.Verein
                         join b in db.Benutzer on v.Index equals b.VereinId
                         where b.Id == item
                         select v).SingleOrDefault();
                Benutzer benutzer = (from b in db.Benutzer
                                     where b.Id == item
                                     select b).SingleOrDefault();
                if(verein != null && benutzer != null)
                {
                    VereinsDaten data = new VereinsDaten();
                    data.userId = benutzer.Id;
                    data.VereinsName = verein.Vereinsname;
                    data.Vorname = benutzer.Vorname;
                    data.Nachname = benutzer.Nachname;
                    data.Telefon = benutzer.Telefon;
                    lst.Add(data);
                }
            
            }
            return View(lst); 
        }
        public ActionResult Accept(int id)
        {
            Benutzer benutzer = (from b in db.Benutzer
                                 where b.Id == id
                                 select b).SingleOrDefault();

            roleProvider.ChangeUserRole(benutzer.Email, "Vereinsverantwortlicher");
            return RedirectToAction("Index");
        }
        public ActionResult Reject(int id)
        {
            Benutzer benutzer = (from b in db.Benutzer
                                 where b.Id == id
                                 select b).SingleOrDefault();
            BenutzerRollen benutzerRollen = (from b in db.BenutzerRollen
                                             where b.BenutzerId == benutzer.Id
                                             select b).SingleOrDefault();
            Verein verein = (from v in db.Verein
                             where v.Index == benutzer.VereinId
                             select v).SingleOrDefault();

            db.Verein.Remove(verein);
            db.BenutzerRollen.Remove(benutzerRollen);
            db.Benutzer.Remove(benutzer);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}