using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DataContext db = new DataContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();

        public ActionResult Index()
        {
            string rolle = roleProvider.GetRolesForUser(User.Identity.Name).ElementAtOrDefault(0);
            Benutzer benutzer = (from b in db.Benutzer where b.Email == User.Identity.Name select b).SingleOrDefault();
            if(rolle == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (rolle == "WartetAufBestaetigung")
            {
                 return RedirectToAction("WaitForConfirmation", "Login");
            }
            if (rolle == "VereinsverantwortlicherDaten")
            {
                return RedirectToAction("VereinsverantwortlicherDaten", "Login", new { userId = benutzer.ID_Benutzer });
            }
            if (rolle == "VereinsDaten")
            {
                return RedirectToAction("VereinsDaten", "Login", new { userId = benutzer.ID_Benutzer });
            }
            if (rolle == "Administrator")
            {
                Administrator administrator = (from a in db.Administrator
                                               where a.Mailadresse == User.Identity.Name
                                               select a).SingleOrDefault();
                List<Wettkampf> wList = (from w in db.Wettkampf
                                         where w.ID_Administrator == administrator.ID_Administrator
                                         orderby w.Datum descending
                                         select w).ToList();
                if(wList.Count > 0)
                {
                    GlobalData.currentWettkampf = wList.First();
                    GlobalData.wettkampfList = wList;
                    TempData["CurrentWettkampf"] = wList.First();
                    TempData["WettkampfList"] = wList;
                }
                return RedirectToAction("Index", "Anfrage");
            }
            if (rolle == "Vereinsverantwortlicher")
            {
                Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher
                                                                   where v.Mailadresse == User.Identity.Name
                                                                   select v).SingleOrDefault();
                if (vereinsverantwortlicher != null)
                {
                    Verein verein = vereinsverantwortlicher.Verein;
                    TempData["Verein"] = verein;
                    List<Wettkampf> wList = (from w in db.Wettkampf
                                             join a in db.Anmeldung on w.ID_Wettkampf equals a.ID_Wettkampf
                                             where a.ID_Verein == verein.ID_Verein
                                             orderby w.Datum descending
                                             select w).ToList();
                    TempData["CurrentWettkampf"] = wList.First();
                    TempData["WettkampfList"] = wList;
                    GlobalData.verein = verein;
                    GlobalData.currentWettkampf = wList.First();
                    GlobalData.wettkampfList = wList;

                }
                return RedirectToAction("Index", "Athleten");
            }
            return View();
        }
    }
}