﻿using System;
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
            string rolle = roleProvider.GetRolesForUser(User.Identity.Name).ElementAt(0);
            if (rolle == "WartetAufBestaetigung")
            {
                Benutzer benutzer = (from b in db.Benutzer where b.Email == User.Identity.Name select b).SingleOrDefault();
                if (benutzer.ID_Object == null)
                {
                    return RedirectToAction("VereinsDaten", "Login", new { userId = benutzer.ID_Benutzer });
                }
                else
                {
                    return RedirectToAction("WaitForConfirmation", "Login");
                }
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