using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Models;

namespace ZeiterfassungPierburg.Controllers
{
    public class MitarbeiterController : Controller
    {
        /*
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
        */
        // 1. *************RETRIEVE ALL MITARBEITER DETAILS ******************
        // GET: Mitarbeiter
        public ActionResult Index()
        {
            MitarbeiterDBHandle dbhandle = new MitarbeiterDBHandle();
            ModelState.Clear();
            return View(dbhandle.GetMitarbeiter());
        }

        // 2. *************ADD NEW MITARBEITER ******************
        // GET: Mitarbeiter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mitarbeiter/Create
        [HttpPost]
        public ActionResult Create(Mitarbeiter mitarbeitermodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MitarbeiterDBHandle mitarbdb = new MitarbeiterDBHandle();
                    if (mitarbdb.AddMitarbeiter(mitarbeitermodel))
                    {
                        ViewBag.Message = "Mitarbeiter erfolgreich hinzugefügt.";
                        ModelState.Clear();
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // 3. ************* EDIT MITARBEITER DETAILS ******************
        // GET: Mitarbeiter/Edit/5
        public ActionResult Edit(int id)
        {
            MitarbeiterDBHandle mitarbdb = new MitarbeiterDBHandle();
            return View(mitarbdb.GetMitarbeiter().Find(mitarbeitermodel => mitarbeitermodel.ID == id));
        }

        // POST: Mitarbeiter/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Mitarbeiter mitarbeitermodel)
        {
            try
            {
                MitarbeiterDBHandle mitarbdb = new MitarbeiterDBHandle();
                mitarbdb.UpdateDetails(mitarbeitermodel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // 4. ************* DELETE MITARBEITER DETAILS ******************
        // GET: Mitarbeiter/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                MitarbeiterDBHandle mitarbdb = new MitarbeiterDBHandle();
                if (mitarbdb.DeleteMitarbeiter(id))
                {
                    ViewBag.AlertMsg = "Mitarbeiter gelöscht.";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

