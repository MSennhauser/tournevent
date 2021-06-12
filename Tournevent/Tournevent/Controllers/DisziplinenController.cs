using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DisziplinenController : Controller
    {
        private readonly DataContext db = new DataContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Disziplinen
        public ActionResult Index()
        {
            int wettkampfID = GlobalData.currentWettkampf.ID_Wettkampf;
            List<Disziplin> disziplinen = (from d in db.Disziplin
                                           where d.ID_Wettkampf == wettkampfID
                                           select d).ToList();
            List<DisziplinenDaten> lst = new List<DisziplinenDaten>();
            foreach (var d in disziplinen)
            {
                lst.Add(new DisziplinenDaten(d));
            }
            return View(lst);
        }

        // GET: Disziplinen/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Disziplinen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Disziplinen/Create
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

        // GET: Disziplinen/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Disziplinen/Edit/5
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

        // GET: Disziplinen/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Disziplinen/Delete/5
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
