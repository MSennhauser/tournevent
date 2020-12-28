using System;
using System.Collections.Generic;
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
            if (User.IsInRole("Administrator"))
            {
                List<SelectListItem> lst = new List<SelectListItem>();
                List<Wettkampf> wettkampf = (from w in db.Wettkampf select w).ToList();
                foreach (var tmp in wettkampf)
                {
                    lst.Add(new SelectListItem() { Text = tmp.WettkampfName, Value = tmp.Id.ToString() });
                }
                return PartialView("_SelectWettkampf", lst);
            }
            else
            {
                var email = User.Identity.Name;
                List<SelectListItem> lst = new List<SelectListItem>();
                Verein verein = (from b in db.Benutzer
                                 join v in db.Verein on b.VereinId equals v.Index
                                 where b.Email == email
                                 select v).Single();

                List<Wettkampf> wettkampf = (from w in db.Wettkampf
                                             join vw in db.VereineWettkampf on w.Id equals vw.WettkampfId
                                             where vw.VereinId == verein.Index select w).ToList();
                foreach (var tmp in wettkampf)
                {
                    lst.Add(new SelectListItem() { Text = tmp.WettkampfName, Value = tmp.Id.ToString() });
                }
                return PartialView("_SelectWettkampf", lst);
            }
        }
    }
}