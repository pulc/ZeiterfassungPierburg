using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models.NeueZeiterfassung;

namespace ZeiterfassungPierburg.Controllers.ModelControllers
{
    public class NeueZeiterfassungController : Controller
    {
        // GET: NeueZeiterfassung
        public ActionResult Index()
        {
            return View();
        }

        // GET: NeueZeiterfassung/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NeueZeiterfassung/Create
        public ActionResult Create()
        {
            var produktionsanlagen = ControllerHelper.SelectColumn("Produktionsanlage", "Bezeichner", "Bezeichner");

            var model = new NeueZeiterfassung();

            // Create a list of SelectListItems so these can be rendered on the page
            model.ProduktionsanlageList = ControllerHelper.GetSelectListItems(produktionsanlagen);
            return View(model);
        }

        // POST: NeueZeiterfassung/Create
        [HttpPost]
        public ActionResult Create(NeueZeiterfassung model)
        {
            try
            {
                // Get all states again
                var produktionsanlagen = ControllerHelper.SelectColumn("Produktionsanlage", "Bezeichner", "Bezeichner");

                // Set these states on the model. We need to do this because
                // only the selected value from the DropDownList is posted back, not the whole
                // list of states.
                model.ProduktionsanlageList = ControllerHelper.GetSelectListItems(produktionsanlagen);

                // In case everything is fine - i.e. both "Name" and "State" are entered/selected,
                // redirect user to the "Done" page, and pass the user object along via Session
                
                if (ModelState.IsValid)
                {
                    Session["NeueZeiterfassung"] = model;
                    return RedirectToAction("Done");
                }
                
                
                if (ModelState.IsValid)
                {
                    
                    NeueZeiterfassungDBHandler sdb = new NeueZeiterfassungDBHandler();
                    if (sdb.AddZeiterfassung(model))
                    {
                        ModelState.Clear();
                    }

                }
                // Something is not right - so render the registration page again,
                // keeping the data user has entered by supplying the model.
                return View("Create", model);

            }
            catch
            {
                return Create();
            }
        }

        // GET: NeueZeiterfassung/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NeueZeiterfassung/Edit/5
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

        public ActionResult Done()
        {
            // Get information from the session
            var model = Session["NeueZeiterfassung"] as NeueZeiterfassung;

            // Display Done.html page that shows Name and selected state.
            return View(model);
        }

        // GET: NeueZeiterfassung/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NeueZeiterfassung/Delete/5
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
