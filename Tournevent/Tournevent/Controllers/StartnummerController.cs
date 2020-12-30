using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StartnummerController : Controller
    {
        private readonly Entities db = new Entities();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Startnummer
        public ActionResult Index()
        {
            int wettkampfId = CurrentWettkampf.Id;
            List<Startnummern> allNummbers = (from s in db.Startnummern where s.WettkampfId == wettkampfId select s).ToList();
            List<List<int>> nums = new List<List<int>>();
            List<int> tmpLst = new List<int>();
            for (var i = 0; i < allNummbers.Count(); i++)
            {
                tmpLst.Add(allNummbers.ElementAt(i).Startnummer);
                if((i+ 1) % 10 == 0 || (i+1) == allNummbers.Count())
                {
                    nums.Add(tmpLst);
                    tmpLst = new List<int>();
                }
            }
            Debug.WriteLine(nums);
            return View(nums);
        }

        // GET: Startnummer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Startnummer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Startnummer/Create
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

        // GET: Startnummer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Startnummer/Edit/5
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

        // GET: Startnummer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Startnummer/Delete/5
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
