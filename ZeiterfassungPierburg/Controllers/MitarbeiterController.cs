using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZeiterfassungPierburg.Controllers
{
    public class MitarbeiterController : Controller
    {
        // GET: Mitarbeiter
        public ActionResult Index()
        {
            return View();
        }

        // GET: Mitarbeiter/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mitarbeiter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mitarbeiter/Create
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

        // GET: Mitarbeiter/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mitarbeiter/Edit/5
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

        // GET: Mitarbeiter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mitarbeiter/Delete/5
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
