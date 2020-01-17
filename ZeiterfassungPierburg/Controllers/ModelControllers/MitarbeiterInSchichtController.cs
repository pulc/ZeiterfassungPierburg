using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;
using ZeiterfassungPierburg.Models.ViewModel.MitarbeiterInschichtViewModel;

namespace ZeiterfassungPierburg.Controllers
{
    public class MitarbeiterInSchichtController : Controller
    {
        // GET: MitarbeiterInSchicht
        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];

            var results = SQLServer.Instance.GetMitarbeiterInSchichtModel<MitarbeiterInschichtViewModel>();
            return View(results);
        }
        public ActionResult IndexMEBA()
        {
            var results = SQLServer.Instance.GetMitarbeiterInSchichtModelMEBA<MitarbeiterInschichtViewModel>();
            return View(results);
        }

        // GET: MitarbeiterInSchicht/Edit/5
        public ActionResult Edit(int id)
        {
            var results = SQLServer.Instance.GetItems<MitarbeiterInSchicht>("id = " + id.ToString());
            if (results.Count() != 1)
            {
                // todo: implement proper error message to be displayed
                return HttpNotFound("Der MitarbeiterInSchicht wurde nicht gefunden.");
            }
            else
                return View(results.First());
        }

        // GET: MitarbeiterInSchicht/Create
        public ActionResult Create()
        {
            return RedirectToAction("Index", "NeuezeiterfassungController");
        }

        // POST: MitarbeiterInSchicht/Create
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

        // POST: MitarbeiterInSchicht/Edit/5
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

        public ActionResult Delete(int id)
        {
            try
            {
                SQLServer.Instance.RemoveItem<MitarbeiterInSchicht>(id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Der Eintrag konnte nicht gelöscht werden.";
                //return Index();
                return RedirectToAction("Index");
            }
        }
       
    }
}
