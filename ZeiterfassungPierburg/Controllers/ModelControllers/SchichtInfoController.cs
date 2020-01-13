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
    public class SchichtInfoController : Controller
    {
        // GET: SchichtInfo
        public ActionResult Index(String message)
        {
            ViewBag.SchichtInfoMessage = TempData["Message"];
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
        [HttpPost]
        public ActionResult Create(SchichtInfo m)
        {
            try
            {
                SQLServer.Instance.InsertItem<SchichtInfo>(m);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /*
        // POST: SchichtInfo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Dictionary<string, string> form = collection.AllKeys.ToDictionary(k => k, v => collection[v]);

                SchichtInfo m = new SchichtInfo();

                foreach (KeyValuePair<string, string> entry in form)
                {
                    m.SetValue(entry.Value, entry.Key);
                }
                SQLServer.RunSqlCommand(new DataMapper<SchichtInfo>("SchichtInfo").GetInsertSqlString(m));

                return RedirectToAction("Index");
            }
            catch (Exception t)
            {
                return View();
            }
        }
        */

        /*
    // POST: SchichtInfo/Edit/5


    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
        try
        {
            Dictionary<string, string> form = collection.AllKeys.ToDictionary(k => k, v => collection[v]);

            SchichtInfo m = new SchichtInfo();

            foreach (KeyValuePair<string, string> entry in form)
            {
                m.SetValue(entry.Value, entry.Key);
            }
            SQLServer.RunSqlCommand(new DataMapper<SchichtInfo>("SchichtInfo").GetUpdateSqlString(m, id));

            return RedirectToAction("Index");
        }
        catch (Exception t)
        {
            return View();
        }
    }
    */

        // POST: SchichtInfo/Edit/5


        [HttpPost]
        public ActionResult Edit(SchichtInfo m)
        {
            try
            {
                SQLServer.Instance.EditItem<SchichtInfo>(m);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }
        // GET: SchichtInfo/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                SQLServer.Instance.RemoveItem<SchichtInfo>(id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Der SchichtInfo konnte nicht gelöscht werden.";
                //return Index();
                return RedirectToAction("Index");
            }

        }
    }
}
