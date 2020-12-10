using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tournevent.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AnfrageController : Controller
    {
        // GET: Anfrage
        public ActionResult Index()
        {

            return View();
        }

        
    }
}