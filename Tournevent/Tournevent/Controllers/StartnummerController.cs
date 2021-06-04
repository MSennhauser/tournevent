using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StartnummerController : Controller
    {
        private readonly DataContext db = new DataContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Startnummer
        public ActionResult Index()
        {
            int wettkampfId = GlobalVariables.WettkampfId;
            List<Startnummer> allNummbers = (from s in db.Startnummer where s.ID_Wettkampf == wettkampfId select s).ToList();
            List<List<int>> nums = new List<List<int>>();
            List<int> tmpLst = new List<int>();
            for (var i = 0; i < allNummbers.Count(); i++)
            {
                tmpLst.Add(allNummbers.ElementAt(i).Startnr);
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
            int athletenId = (from s in db.Startnummer where s.Startnr == id && s.ID_Wettkampf == GlobalVariables.WettkampfId select s.ID_Athlet).Single();
            return RedirectToAction("Edit", new RouteValueDictionary(
                    new { controller = "Athleten", action = "Edit", Id = athletenId }));
        }
    }
}
