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
                return RedirectToAction("Index", "Anfrage");
            }
            if (rolle == "Vereinsverantwortlicher")
            {
                return RedirectToAction("Index", "Athleten");
            }
            return View();
        }
    }
}