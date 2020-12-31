using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VereinController : Controller
    {
        private readonly Entities db = new Entities();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Verein
        public ActionResult Index()
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            List<VereinsDaten> lst = new List<VereinsDaten>();
            List<Verein> vereinList = new List<Verein>();
            vereinList = (from v in db.Verein
                          join vw in db.VereineWettkampf on v.Index equals vw.VereinId
                          where vw.WettkampfId == wettkampfId
                          select v).ToList();
            foreach(var verein in vereinList)
            {
                Benutzer benutzer = (from b in db.Benutzer 
                                     where b.VereinId == verein.Index select b).Single();
                lst.Add(new VereinsDaten(benutzer, verein));
            }
            return View(lst);
        }

        // GET: Verein/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Verein/Create
        public ActionResult Create()
        {
            List<Verein> vereinList = (from v in db.Verein select v).ToList();
            List<VereinWettkampf> vwList = new List<VereinWettkampf>();
            foreach(var v in vereinList)
            {
                List<int> id = (from vw in db.VereineWettkampf where vw.VereinId == v.Index select vw.WettkampfId).ToList();
                vwList.Add(new VereinWettkampf(v, id));
            }
            return View(vwList);
        }

        // POST: Verein/Create
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

        // GET: Verein/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Verein/Edit/5
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

        // GET: Verein/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Verein/Delete/5
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
