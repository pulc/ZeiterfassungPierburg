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
    public class MitarbeiterController : Controller
    {
        // GET: Mitarbeiter
        public ActionResult Index()
        {
            ViewBag.MitarbeiterMessage = TempData["Message"];

            var results = SQLServer.Instance.GetItems<Mitarbeiter>();

            return View(results);
        }

        // GET: Mitarbeiter/Edit/5
        [Authorize(Users = Startup.Administrators)]
        public ActionResult Edit(int id)
        {
            var results = SQLServer.Instance.GetItems<Mitarbeiter>("id = " + id.ToString());
            if (results.Count() != 1)
            {
                // todo: implement proper error message to be displayed
                return HttpNotFound("Der Mitarbeiter wurde nicht gefunden.");
            }
            else
                return View(results.First());
        }


        // GET: Produktionsanlage/Create
        public ActionResult Create()
        {
            Mitarbeiter m = new Mitarbeiter();
            return View(m);
        }

        [HttpPost]
        public ActionResult Create(Mitarbeiter m)
        {
            try
            {
                SQLServer.Instance.InsertItem<Mitarbeiter>(m);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                ViewBag.MitarbeiterMessage = "Es ist ein Fehler aufgetreten. Kein Mitarbeiter wurde hinzugefügt. Grund: " + e;
                return View(m);
            }
        }
        [Authorize(Users = Startup.Administrators)]
        [HttpPost]
        public ActionResult Edit(Mitarbeiter m)
        {
            try
            {
                SQLServer.Instance.EditItem<Mitarbeiter>(m);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(m);
            }
        }
        // GET: Mitarbeiter/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {

                    SQLServer.Instance.RemoveItem<Mitarbeiter>(id);
                    TempData["Message"] = "Mitarbeiter gelöscht.";
                    return RedirectToAction("Index");
                
            }
            catch 
            {
                TempData["Message"] = "Der Mitarbeiter konnte nicht gelöscht werden, weil er/sie bereits Einträge hat";
                return RedirectToAction("Index");
            }
        }
    }
}
