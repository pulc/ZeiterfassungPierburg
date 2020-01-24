﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;

namespace ZeiterfassungPierburg.Controllers
{
    public class ZugriffsrechteController : Controller
    {
        // GET: Zugriffsrechte
        public ActionResult Index()
        {
            ViewBag.ZugriffsrechteMessage = TempData["Message"];

            var results = SQLServer.Instance.GetItems<Zugriffsrechte>();

            return View(results);
        }

        // GET: Zugriffsrechte/Edit/5
        public ActionResult Edit(int id)
        {
            var results = SQLServer.Instance.GetItems<Zugriffsrechte>("id = " + id.ToString());
            if (results.Count() != 1)
            {
                // todo: implement proper error message to be displayed
                return HttpNotFound("Der Zugriffsrecht wurde nicht gefunden.");
            }
            else
                return View(results.First());
        }


        // GET: Produktionsanlage/Create
        public ActionResult Create()
        {
            Zugriffsrechte m = new Zugriffsrechte();
            return View(m);
        }

        [HttpPost]
        public ActionResult Create(Zugriffsrechte m)
        {
            try
            {
                SQLServer.Instance.InsertItem<Zugriffsrechte>(m);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                ViewBag.ZugriffsrechteMessage = "Es ist ein Fehler aufgetreten. Kein Zugriffsrechte wurde hinzugefügt. Grund: " + e;
                return View(m);
            }
        }

        [HttpPost]
        public ActionResult Edit(Zugriffsrechte m)
        {
            try
            {
                SQLServer.Instance.EditItem<Zugriffsrechte>(m);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(m);
            }
        }
        // GET: Zugriffsrechte/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                SQLServer.Instance.RemoveItem<Zugriffsrechte>(id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Der Zugriffsrechte konnte nicht gelöscht werden.";
                //return Index();
                return RedirectToAction("Index");
            }
        }
    }
}