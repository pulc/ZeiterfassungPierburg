﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;


namespace ZeiterfassungPierburg.Controllers
{
    public class ProduktionsanlageController : Controller
    {
        // GET: Produktionsanlage
        public ActionResult Index()
        {
            var results = SQLServer.Instance.GetItems<Produktionsanlage>();
            return View(results);
        }

        // GET: Produktionsanlage/Edit/5
        public ActionResult Edit(int id)
        {
            var results = SQLServer.Instance.GetItems<Produktionsanlage>("id = " + id.ToString());
            if (results.Count() != 1)
            {
                // todo: implement proper error message to be displayed
                return HttpNotFound("Der Produktionsanlage wurde nicht gefunden.");
            }
            else
                return View(results.First());
        }

        // GET: Produktionsanlage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Produktionsanlage/Create
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

        // POST: Produktionsanlage/Edit/5
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

        // GET: Produktionsanlage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Produktionsanlage/Delete/5
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
