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
        private readonly DataContext db = new DataContext();
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

                    int userId = (from b in db.Benutzer
                                  where b.Email == benutzer.Email
                                  select b.ID_Benutzer).SingleOrDefault();
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
                Verein vereinsName = db.Verein.FirstOrDefault(u => u.Name.ToLower() == vereinsDaten.VereinsName.ToLower());

                if (vereinsName == null)
                {
                    Verein verein = new Verein();
                    verein.Name = vereinsDaten.VereinsName;
                    db.Verein.Add(verein);
                    db.SaveChanges();
                    int vereinsId = (from v in db.Verein
                                     where v.Name == vereinsDaten.VereinsName
                                     select v.ID_Verein).Single();
                    Benutzer benutzer = (from b in db.Benutzer
                                         where b.ID_Benutzer == vereinsDaten.userId
                                         select b).Single();
                    Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher
                                                                       join b in db.Benutzer on v.ID_Vereinsverantwortlicher equals b.ID_Object
                                                                       where b.ID_Benutzer == benutzer.ID_Benutzer
                                                                       select v).SingleOrDefault();
                    benutzer.Telefon = vereinsDaten.Telefon;
                    vereinsverantwortlicher.Nachname = vereinsDaten.Nachname;
                    vereinsverantwortlicher.Vorname = vereinsDaten.Vorname;
                    vereinsverantwortlicher.ID_Verein = vereinsId;

                    db.Benutzer.Attach(benutzer);
                    ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(benutzer, System.Data.Entity.EntityState.Modified);
                    db.Vereinsverantwortlicher.Attach(vereinsverantwortlicher);
                    ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(vereinsverantwortlicher, System.Data.Entity.EntityState.Modified);

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