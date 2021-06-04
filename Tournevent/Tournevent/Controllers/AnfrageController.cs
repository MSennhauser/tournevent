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
        private readonly DataContext db = new DataContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // Gibt alle Anfragen zurück
        public ActionResult Index()
        {
            List<VereinsDaten> lst = new List<VereinsDaten>();
            List<Benutzer> benutzerWartetList = (from b in db.Benutzer
                                where b.Rolle == "WartetAufBestaetigung"
                                select b).ToList();

            foreach(Benutzer benutzer in benutzerWartetList)
            {
                Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher
                         join b in db.Benutzer on v.ID_Vereinsverantwortlicher equals b.ID_Object
                         where b.ID_Benutzer == benutzer.ID_Benutzer
                         select v).SingleOrDefault();
                Verein verein = vereinsverantwortlicher.Verein;
                if (verein != null && benutzer != null)
                {
                    VereinsDaten data = new VereinsDaten();
                    data.userId = benutzer.ID_Benutzer;
                    data.VereinsName = verein.Name;
                    data.Vorname = vereinsverantwortlicher.Vorname;
                    data.Nachname = vereinsverantwortlicher.Nachname;
                    data.Telefon = benutzer.Telefon;
                    lst.Add(data);
                }
            
            }
            return View(lst); 
        }
        // Anfrage wird akzeptiert
        public ActionResult Accept(int id)
        {
            Benutzer benutzer = (from b in db.Benutzer
                                 where b.ID_Benutzer == id
                                 select b).SingleOrDefault();

            roleProvider.ChangeUserRole(benutzer.Email, "Vereinsverantwortlicher");
            return RedirectToAction("Index");
        }
        // Anfrage wird abgelehnt
        public ActionResult Reject(int id)
        {
            Benutzer benutzer = (from b in db.Benutzer
                                 where b.ID_Benutzer == id
                                 select b).SingleOrDefault();

            Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher
                                                               join b in db.Benutzer on v.ID_Vereinsverantwortlicher equals b.ID_Object
                                                               where b.ID_Benutzer == benutzer.ID_Benutzer
                                                               select v).SingleOrDefault();
            Verein verein = vereinsverantwortlicher.Verein;

            db.Verein.Remove(verein);
            db.Benutzer.Remove(benutzer);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}