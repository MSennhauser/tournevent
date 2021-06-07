using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournevent.Models;

namespace Tournevent.Controllers
{
    [Authorize(Roles = "Administrator,Vereinsverantwortlicher")]
    public class LayoutController : Controller
    {
        private readonly DataContext db = new DataContext();
        private readonly UserRoleProvider roleProvider = new UserRoleProvider();
        // GET: Layout
        [ChildActionOnly]
        public ActionResult Wettkaempfe()
        {
            var vId = (from v in db.Vereinsverantwortlicher where v.Mailadresse == User.Identity.Name select v.ID_Verein).Single();
            if (vId != null && User.IsInRole("Vereinsverantwortlicher"))
            {
                GlobalVariables.VereinsId = (int)vId;
            }
            return PartialView("_SelectWettkampf", getWettkaempfe());
        }

        [ChildActionOnly]
        [HttpPost]
        public ActionResult Wettkaempfe(FormCollection collection)
        {
            string id = collection.Get("Wettkampf");
            if(id != "")
            {
                GlobalVariables.WettkampfId = Convert.ToInt32(id);
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
            else if (User.IsInRole("Vereinsverantwortlicher"))
            {
                var email = User.Identity.Name;
                Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher
                                                                   where v.Mailadresse == email
                                                                   select v).SingleOrDefault();
                Verein verein = vereinsverantwortlicher.Verein;

                wettkampf = (from w in db.Wettkampf
                                join a in db.Anmeldung on w.ID_Wettkampf equals a.ID_Wettkampf
                                where a.ID_Verein == verein.ID_Verein
                                select w).ToList();
            }
            if(wettkampf.Count() == 1)
            {
                GlobalVariables.WettkampfId = wettkampf.ElementAt(0).ID_Wettkampf;
            }
            foreach (Wettkampf tmp in wettkampf)
            {
                if (tmp.ID_Wettkampf == GlobalVariables.WettkampfId)
                {
                    lst.Add(new SelectListItem() { Text = tmp.Name, Value = tmp.ID_Wettkampf.ToString(), Selected = true });
                }
                else
                {
                    lst.Add(new SelectListItem() { Text = tmp.Name, Value = tmp.ID_Wettkampf.ToString(), Selected = false });
                }

                if(GlobalVariables.WettkampfId == 0)
                {
                    GlobalVariables.WettkampfId = Convert.ToInt32(lst.ElementAt(0).Value);
                }

            }
            return lst;
        }
    }
}