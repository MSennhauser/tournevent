using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournavent.Models;

namespace Tournavent.Controllers
{
    public class PersonController : Controller
    {
        private IPersonModel _personModel;

        public PersonController(IPersonModel personModel)
        {
            _personModel = personModel;
        }
        // GET: Person
        public ViewResult Index()
        {
            var model = _personModel.GetPeople();
            return View(model);
            //return " count " + _personModel.GetPeople().Count().ToString();
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            Person person = _personModel.GetPerson(id);
            return View(person);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        [HttpPost]
        public RedirectToRouteResult Create(Person person)
        {
            // TODO: Add insert logic here
            Debug.WriteLine("Ausgabe auf der Konsole");
            if(_personModel.GetPeople().Count() > 0)
            {
                person.Id = _personModel.GetPeople().Max(p => p.Id) + 1;

            }
            else
            {
                person.Id = 0;
            }

            if (person != null)
            {
                Person newPerson = _personModel.AddPerson(person);
                return RedirectToAction("Details", new { id = newPerson.Id });
            }
            //return "id = " + person.Id + " name = " + person.Name + " alter = " + person.Alter.ToString() + " beruf = " + person.Beruf + " count " + _personModel.GetPeople().Count().ToString();
            return RedirectToAction("Index");

        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Person/Edit/5
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

        // GET: Person/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Person/Delete/5
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
