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
            var results = SQLServer.Instance.GetItems<Fertigungsteil>();
            return View(results);
        }

        // GET: Fertigungsteil/Edit/5
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

        // GET: Fertigungsteil/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Fertigungsteil m)
        {
            try
            {
                SQLServer.Instance.InsertItem<Fertigungsteil>(m);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

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
                return View();
            }
        }
        // GET: Fertigungsteil/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                SQLServer.Instance.RemoveItem<Fertigungsteil>(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
