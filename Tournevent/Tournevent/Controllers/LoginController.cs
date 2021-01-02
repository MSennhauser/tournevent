using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    public class LoginController : Controller
    {
        private readonly DBContext db = new DBContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Benutzer user)
        {
            if (ModelState.IsValid)
            {
                bool IsValidUser = db.Benutzer
               .Any(u => 
               u.Email.ToLower() == user.Email.ToLower() && 
               u.Passwort == user.Passwort);

                if (IsValidUser)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email oder Passwort ist falsch.");
                }
            }

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KontoDaten daten)
        {
            if (ModelState.IsValid)
            {
                Benutzer benutzer = new Benutzer();
                benutzer.Email = daten.Email;
                benutzer.Passwort = daten.Passwort;
                Benutzer email = db.Benutzer.FirstOrDefault(u => u.Email.ToLower() == daten.Email.ToLower());
                
                if(email == null)
                {

                    db.Benutzer.Add(benutzer);
                    db.SaveChanges();

                    roleProvider.AddUserToRole(benutzer.Email, "WartetAufBestaetigung");

                    var userId = (from b in db.Benutzer
                                  where b.Email == benutzer.Email
                                  select b.Id).SingleOrDefault();
                    return RedirectToAction("VereinsDaten", new { userId = userId });
                }
                else
                {
                    ModelState.AddModelError("Email", "Diese Email Adresse existiert bereits.");
                }
            }
            return View();
        }
        [Authorize(Roles = "WartetAufBestaetigung")]
        public ActionResult WaitForConfirmation()
        {
            return View();
        }

        public ActionResult VereinsDaten(int userId)
        {
            VereinsDaten vereinsDaten = new VereinsDaten();
            vereinsDaten.userId = userId;
            return View(vereinsDaten);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VereinsDaten(VereinsDaten vereinsDaten)
        {
            if (ModelState.IsValid)
            {
                Verein vereinsName = db.Verein.FirstOrDefault(u => u.Vereinsname.ToLower() == vereinsDaten.VereinsName.ToLower());

                if (vereinsName == null)
                {
                    Verein verein = new Verein();
                    verein.Vereinsname = vereinsDaten.VereinsName;
                    db.Verein.Add(verein);
                    db.SaveChanges();
                    var vereinsId = (from v in db.Verein
                                     where v.Vereinsname == vereinsDaten.VereinsName
                                     select v.Index).Single();
                    Benutzer benutzer = (from b in db.Benutzer
                                         where b.Id == vereinsDaten.userId
                                         select b).Single();
                    benutzer.Telefon = vereinsDaten.Telefon;
                    benutzer.Nachname = vereinsDaten.Nachname;
                    benutzer.Vorname = vereinsDaten.Vorname;
                    benutzer.VereinId = vereinsId;

                    db.Benutzer.Attach(benutzer);
                    ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(benutzer, System.Data.Entity.EntityState.Modified);

                    db.SaveChanges();

                    FormsAuthentication.SetAuthCookie(benutzer.Email, true);
                    return RedirectToAction("WaitForConfirmation", "Login");
                }
                else
                {
                    ModelState.AddModelError("VereinsName", "Dieser Verein existiert bereits.");
                }

            }
            return View(vereinsDaten);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}