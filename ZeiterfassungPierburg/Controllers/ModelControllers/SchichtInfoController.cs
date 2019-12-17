using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;

namespace ZeiterfassungPierburg.Controllers
{
    public class SchichtInfoController : Controller
    {
        // GET: SchichtInfo
        public ActionResult Index()
        {
            var results = SQLServer.Instance.GetItems<SchichtInfo>();
            return View(results);
        }

        // GET: SchichtInfo/Edit/5
        public ActionResult Edit(int id)
        {
            var results = SQLServer.Instance.GetItems<SchichtInfo>("id = " + id.ToString());
            if (results.Count() != 1)
            {
                // todo: implement proper error message to be displayed
                return HttpNotFound("Der SchichtInfo wurde nicht gefunden.");
            }
            else
                return View(results.First());
        }

        // GET: SchichtInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SchichtInfo/Create
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

        // POST: SchichtInfo/Edit/5
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

        // GET: SchichtInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SchichtInfo/Delete/5
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
