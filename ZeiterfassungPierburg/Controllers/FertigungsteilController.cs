using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Models;
using ZeiterfassungPierburg.Models.Produktion;

namespace ZeiterfassungPierburg.Controllers
{
    public class FertigungsteilController : Controller
    {
        // 1. *************RETRIEVE ALL STUDENT DETAILS ******************
        // GET: Fertigungsteil
        public ActionResult Index()
        {
            FertigungsteilDBHandle dbhandle = new FertigungsteilDBHandle();
            ModelState.Clear();
            return View(dbhandle.GetFertigungsteil());
        }

        // 2. *************ADD NEW STUDENT ******************
        // GET: Fertigungsteil/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(Fertigungsteil smodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FertigungsteilDBHandle sdb = new FertigungsteilDBHandle();
                    if (sdb.AddFertigungsteil(smodel))
                    {
                        ViewBag.Message = "Fertigungsteil Details Added Successfully";
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

        // 3. ************* EDIT STUDENT DETAILS ******************
        // GET: Fertigungsteil/Edit/5
        public ActionResult Edit(int id)
        {
            FertigungsteilDBHandle sdb = new FertigungsteilDBHandle();
            return View(sdb.GetFertigungsteil().Find(smodel => smodel.ID == id));
        }

        // POST: Fertigungsteil/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Fertigungsteil smodel)
        {
            try
            {
                FertigungsteilDBHandle sdb = new FertigungsteilDBHandle();
                sdb.UpdateDetails(smodel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // 4. ************* DELETE STUDENT DETAILS ******************
        // GET: Fertigungsteil/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                FertigungsteilDBHandle sdb = new FertigungsteilDBHandle();
                if (sdb.DeleteFertigungsteil(id))
                {
                    ViewBag.AlertMsg = "Fertigungsteil Deleted Successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        /*
        // GET: Fertigungsteil
        public ActionResult Index()
        {
            return View();
        }

        // GET: Fertigungsteil/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Fertigungsteil/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fertigungsteil/Create
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

        // GET: Fertigungsteil/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Fertigungsteil/Edit/5
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

        // GET: Fertigungsteil/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Fertigungsteil/Delete/5
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

    }
}
