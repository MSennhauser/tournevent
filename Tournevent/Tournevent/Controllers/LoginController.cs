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
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    
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
                Benutzer email = db.Benutzer.FirstOrDefault(u => u.Email.ToLower() == daten.Email.ToLower());
                
                if(email == null)
                {
                    Benutzer benutzer = new Benutzer();
                    benutzer.Email = daten.Email;
                    benutzer.Passwort = daten.Passwort;
                    benutzer.Telefon = daten.Telefon;
                    db.Benutzer.Add(benutzer);
                    db.SaveChanges();

                    FormsAuthentication.SetAuthCookie(benutzer.Email, false);
                    roleProvider.ChangeUserRole(benutzer.Email, "VereinsDaten");
                    return RedirectToAction("VereinsDaten");
                }
                else
                {
                    ModelState.AddModelError("Email", "Diese Email Adresse existiert bereits.");
                }
            }
            return View();
        }
        [Authorize(Roles = "WaitForConfirmation")]
        public ActionResult WaitForConfirmation()
        {
            return View();
        }

        [Authorize(Roles = "VereinsverantwortlicherDaten")]
        public ActionResult VereinsverantwortlicherDaten(int vereinsId)
        {
            VereinsverantwortlicherDaten vereinsverantwortlicherDaten = new VereinsverantwortlicherDaten();
            vereinsverantwortlicherDaten.vereinsId = vereinsId;
            return View(vereinsverantwortlicherDaten);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VereinsverantwortlicherDaten(VereinsverantwortlicherDaten data)
        {
            if (ModelState.IsValid)
            {
                Vereinsverantwortlicher vereinsverantwortlicher = new Vereinsverantwortlicher();
                Adresse adresse = new Adresse();

                vereinsverantwortlicher.Nachname = data.Nachname;
                vereinsverantwortlicher.Mailadresse = User.Identity.Name;
                vereinsverantwortlicher.Vorname = data.Vorname;
                adresse.Strasse = data.Strasse;
                adresse.PLZ = data.PLZ;
                adresse.Ort = data.Ort;
                adresse.Hausnummer = data.Hausnummer;
                vereinsverantwortlicher.Adresse = adresse;
                vereinsverantwortlicher.ID_Verein = data.vereinsId;

                db.Vereinsverantwortlicher.Add(vereinsverantwortlicher);
                db.SaveChanges();

                FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
                roleProvider.ChangeUserRole(User.Identity.Name, "WaitForConfirmation");

                return RedirectToAction("WaitForConfirmation", "Login");
                
            }
            return View(data);
        }

        [Authorize(Roles = "VereinsDaten")]
        public ActionResult VereinsDaten()
        {
            return View();
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
                    verein.Ort = vereinsDaten.Ort;
                    verein.PLZ = vereinsDaten.PLZ;
                    db.Verein.Add(verein);
                    db.SaveChanges();
                    
                    roleProvider.ChangeUserRole(User.Identity.Name, "VereinsverantwortlicherDaten");
                    return RedirectToAction("VereinsverantwortlicherDaten", new { vereinsId = verein.ID_Verein });
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