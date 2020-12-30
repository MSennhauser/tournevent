using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    public class LayoutController : Controller
    {
        private readonly Entities db = new Entities();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Layout
        [ChildActionOnly]
        public ActionResult Wettkaempfe()
        {
            return PartialView("_SelectWettkampf", getWettkaempfe());
        }

        [ChildActionOnly]
        [HttpPost]
        public ActionResult Wettkaempfe(FormCollection collection)
        {
            string id = collection.Get("Wettkampf");
            if(id != "")
            {
                CurrentWettkampf.Id = Convert.ToInt32(id);
            }

            return PartialView("_SelectWettkampf", getWettkaempfe());
        }

        [ChildActionOnly]
        public ActionResult Navigation()
        {
            if (User.IsInRole("Administrator"))
            {
                return PartialView("_SideNavAdmin");
            }
            else
            {
                return PartialView("_SideNavVereins");
            }
        }

        private List<SelectListItem> getWettkaempfe()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<Wettkampf> wettkampf = new List<Wettkampf>();
            if (User.IsInRole("Administrator"))
            {
                wettkampf = (from w in db.Wettkampf select w).ToList();

            }
            else
            {
                var email = User.Identity.Name;
                Verein verein = (from b in db.Benutzer
                                 join v in db.Verein on b.VereinId equals v.Index
                                 where b.Email == email
                                 select v).Single();

                wettkampf = (from w in db.Wettkampf
                                             join vw in db.VereineWettkampf on w.Id equals vw.WettkampfId
                                             where vw.VereinId == verein.Index
                                             select w).ToList();
            }
            if(wettkampf.Count() == 1)
            {
                CurrentWettkampf.Id = wettkampf.ElementAt(0).Id;
            }
            foreach (var tmp in wettkampf)
            {
                if (tmp.Id == CurrentWettkampf.Id)
                {
                    lst.Add(new SelectListItem() { Text = tmp.WettkampfName, Value = tmp.Id.ToString(), Selected = true });
                }
                else
                {
                    lst.Add(new SelectListItem() { Text = tmp.WettkampfName, Value = tmp.Id.ToString(), Selected = false });
                }

            }
            return lst;
        }
    }
}