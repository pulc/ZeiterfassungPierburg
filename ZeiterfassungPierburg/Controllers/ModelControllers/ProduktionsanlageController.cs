using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            ViewBag.ProduktionsanlageMessage = TempData["Message"];

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
                return HttpNotFound("Die Produktionsanlage wurde nicht gefunden.");
            }
            else
                return View(results.First());
        }


        // GET: Produktionsanlage/Create
        public ActionResult Create()
        {
            Produktionsanlage m = new Produktionsanlage();
            return View(m);
        }

        [HttpPost]
        public ActionResult Create(Produktionsanlage m)
        {
            try
            {
                SQLServer.Instance.InsertItem<Produktionsanlage>(m);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                ViewBag.ProduktionsanlageMessage = "Es ist ein Fehler aufgetreten. Kein Produktionsanlage wurde hinzugefügt. Grund: " + e;
                return View(m);
            }
        }

        [HttpPost]
        public ActionResult Edit(Produktionsanlage m)
        {
            try
            {
                SQLServer.Instance.EditItem<Produktionsanlage>(m);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(m);
            }
        }
        // GET: Produktionsanlage/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                SQLServer.Instance.RemoveItem<Produktionsanlage>(id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Die Produktionsanlage konnte nicht gelöscht werden.";
                //return Index();
                return RedirectToAction("Index");
            }
        }
    }
}
