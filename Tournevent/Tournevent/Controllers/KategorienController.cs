﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    public class KategorienController : Controller
    {
        private readonly DBContext db = new DBContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Kategorien
        public ActionResult Index()
        {
            return View();
        }

        // GET: Kategorien/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Kategorien/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategorien/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Kategorien/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Kategorien/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Kategorien/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Kategorien/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
