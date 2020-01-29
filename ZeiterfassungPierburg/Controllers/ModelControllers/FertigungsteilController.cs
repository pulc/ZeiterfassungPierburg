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
    public class FertigungsteilController : Controller
    {
        // GET: Fertigungsteil
        public ActionResult Index()
        {
            ViewBag.FertigungsteilMessage = TempData["Message"];

            var results = SQLServer.Instance.GetItems<Fertigungsteil>();

            return View(results);
        }

        [Authorize(Users = Startup.Administrators)]
        public ActionResult Edit(int id)
        {
            var results = SQLServer.Instance.GetItems<Fertigungsteil>("id = " + id.ToString());
            if (results.Count() != 1)
            {
                // todo: implement proper error message to be displayed
                return HttpNotFound("Der Fertigungsteil wurde nicht gefunden.");
            }
            else
                return View(results.First());
        }


        // GET: Produktionsanlage/Create
        public ActionResult Create()
        {
            Fertigungsteil m = new Fertigungsteil();
            return View(m);
        }

        [HttpPost]
        public ActionResult Create(Fertigungsteil m)
        {
            try
            {
                SQLServer.Instance.InsertItem<Fertigungsteil>(m);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                ViewBag.FertigungsteilMessage = "Es ist ein Fehler aufgetreten. Kein Fertigungsteil wurde hinzugefügt. Grund: " + e;
                return View(m);
            }
        }
        [Authorize(Users = Startup.Administrators)]
        [HttpPost]
        public ActionResult Edit(Fertigungsteil m)
        {
            try
            {
                SQLServer.Instance.EditItem<Fertigungsteil>(m);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(m);
            }
        }
        [Authorize(Users = Startup.Administrators)]
        public ActionResult Delete(int id)
        {
            try
            {
                SQLServer.Instance.RemoveItem<Fertigungsteil>(id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Der Fertigungsteil konnte nicht gelöscht werden, weil es bereits für Einträge benutzt wurde";
                //return Index();
                return RedirectToAction("Index");
            }
        }
    }
}
