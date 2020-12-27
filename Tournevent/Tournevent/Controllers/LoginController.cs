﻿using System;
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
        private readonly Entities db = new Entities();
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
                    UserRoleProvider roleProvider = new UserRoleProvider();
                    string rolle = roleProvider.GetRolesForUser(user.Email).ElementAt(0);
                    if(rolle == "WartetAufBestaetigung")
                    {
                        return RedirectToAction("WaitForConfirmation", "Login");
                    }
                    if (rolle == "Administrator")
                    {
                        return RedirectToAction("Index", "Anfrage");
                    }
                    if (rolle == "Vereinsverantwortlicher")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "invalid Email or Password");


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
                else if (roleProvider.IsUserInRole(benutzer.Email, "WartetAufBestaetigung"))
                {
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
        public ActionResult VereinsDaten(VereinsDaten vereinsDaten)
        {
            if (ModelState.IsValid)
            {
                Benutzer benutzer = new Benutzer();
                benutzer.Id = vereinsDaten.userId;
                benutzer.Telefon = vereinsDaten.Telefon;
                benutzer.Nachname = vereinsDaten.Nachname;
                benutzer.Vorname = vereinsDaten.Vorname;

                db.Benutzer.Attach(benutzer);
                ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(benutzer, System.Data.Entity.EntityState.Modified);

                db.SaveChanges();
                Verein verein = new Verein();
                verein.Vereinsname = vereinsDaten.VereinsName;
                db.Verein.Add(verein);
                db.SaveChanges();

                return RedirectToAction("WaitForConfirmation");
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