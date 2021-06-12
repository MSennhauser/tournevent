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
            Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher where v.Mailadresse == User.Identity.Name select v).SingleOrDefault();
            if (vereinsverantwortlicher != null && User.IsInRole("Vereinsverantwortlicher"))
            {
                ViewBag.Verein = vereinsverantwortlicher.Verein;
                /*GlobalVariables.VereinsId = (int)vId;*/
            }
            return PartialView("_SelectWettkampf", getWettkaempfe());
        }

        [ChildActionOnly]
        [HttpPost]
        public ActionResult Wettkaempfe(FormCollection collection)
        {
            string id = collection.Get("Wettkampf");
            if(id != "" && id != null)
            {
                int ID_Wettkampf = Convert.ToInt32(id);
                GlobalData.currentWettkampf = (from w in db.Wettkampf where w.ID_Wettkampf == ID_Wettkampf select w).SingleOrDefault();
                // Reload page after value changed.
                String previousUrl = Request.UrlReferrer.AbsolutePath;
                List<String> paths = previousUrl.Split('/').ToList();
                String controller = paths.ElementAt(1);
                String method = paths.ElementAt(2);
                String value = paths.ElementAt(3);
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
            foreach (Wettkampf tmp in GlobalData.wettkampfList)
            {
                if (tmp.ID_Wettkampf == GlobalData.currentWettkampf.ID_Wettkampf)
                {
                    lst.Add(new SelectListItem() { Text = tmp.Name, Value = tmp.ID_Wettkampf.ToString(), Selected = true });
                }
                else
                {
                    lst.Add(new SelectListItem() { Text = tmp.Name, Value = tmp.ID_Wettkampf.ToString(), Selected = false });
                }

            }
            return lst;
        }
    }
}