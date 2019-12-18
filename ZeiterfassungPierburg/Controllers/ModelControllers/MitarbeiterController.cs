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
            var results = SQLServer.Instance.GetItems<Mitarbeiter>();
            return View(results);
        }

        // GET: Mitarbeiter/Edit/5
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

        // GET: Mitarbeiter/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Mitarbeiter m)
        {
            try
            { 
                SQLServer.RunSqlCommand(new DataMapper<Mitarbeiter>("Mitarbeiter").GetInsertSqlString(m));

                return RedirectToAction("Index");
            }
            catch 
            {
                return View();
            }
        }

        /*
        // POST: Mitarbeiter/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Dictionary<string, string> form = collection.AllKeys.ToDictionary(k => k, v => collection[v]);

                Mitarbeiter m = new Mitarbeiter();

                foreach (KeyValuePair<string, string> entry in form)
                {
                    m.SetValue(entry.Value, entry.Key);
                }
                SQLServer.RunSqlCommand(new DataMapper<Mitarbeiter>("Mitarbeiter").GetInsertSqlString(m));

                return RedirectToAction("Index");
            }
            catch (Exception t)
            {
                return View();
            }
        }
        */

        /*
    // POST: Mitarbeiter/Edit/5


    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
        try
        {
            Dictionary<string, string> form = collection.AllKeys.ToDictionary(k => k, v => collection[v]);

            Mitarbeiter m = new Mitarbeiter();

            foreach (KeyValuePair<string, string> entry in form)
            {
                m.SetValue(entry.Value, entry.Key);
            }
            SQLServer.RunSqlCommand(new DataMapper<Mitarbeiter>("Mitarbeiter").GetUpdateSqlString(m, id));

            return RedirectToAction("Index");
        }
        catch (Exception t)
        {
            return View();
        }
    }
    */

        // POST: Mitarbeiter/Edit/5


        [HttpPost]
        public ActionResult Edit(int id, Mitarbeiter m)
        {
            try
            {
                SQLServer.RunSqlCommand(new DataMapper<Mitarbeiter>("Mitarbeiter").GetUpdateSqlString(m, id));

                return RedirectToAction("Index");
            }
            catch (Exception t)
            {
                return View();
            }
        }
        // GET: Mitarbeiter/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                SQLServer.RunSqlCommand(new DataMapper<Mitarbeiter>("Mitarbeiter").GetDeleteSqlString(id));
                return RedirectToAction("Index");
            }
            catch
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
