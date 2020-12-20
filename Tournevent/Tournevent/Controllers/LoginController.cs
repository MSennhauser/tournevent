using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    public class LoginController : Controller
    {
        private readonly Entities _dbContext = new Entities();
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
                bool IsValidUser = _dbContext.Benutzer
               .Any(u => 
               u.Email.ToLower() == user.Email.ToLower() && 
               u.Passwort == user.Passwort);

                if (IsValidUser)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("Index", "Home");
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
        public ActionResult Register(Benutzer registerUser)
        {
            if (ModelState.IsValid)
            {
                Benutzer email = _dbContext.Benutzer.FirstOrDefault(u => u.Email.ToLower() == registerUser.Email.ToLower());
                if(email == null)
                {
                    _dbContext.Benutzer.Add(registerUser);
                    _dbContext.SaveChanges();

                    UserRoleProvider roleProvider = new UserRoleProvider();
                    roleProvider.AddUserToRole(registerUser.Email, "Vereinsverantwortlicher");

                    return RedirectToAction("WaitForConfirmation");
                }
                else
                {
                    ModelState.AddModelError("Email", "Diese Email Adresse existiert bereits.");
                }
            }
            return View();
        }

        public ActionResult WaitForConfirmation()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}