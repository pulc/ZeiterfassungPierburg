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
        public ActionResult Create(SchichtInfo m)
        {
            try
            {
                SQLServer.RunSqlCommand(new DataMapper<SchichtInfo>("SchichtInfo").GetInsertSqlString(m));

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }



        // POST: SchichtInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SchichtInfo m)
        {
            try
            {
                new DataMapper<SchichtInfo>("SchichtInfo").GetInsertSqlString(m);
                int ID = SQLServer.Instance.InsertItem<SchichtInfo>(m);

                Console.WriteLine("SchichtInfo mit ID " + ID + " wurde geändert.");
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return HttpNotFound("SchichtInfo konnte nicht bearbeitet werden.");
            }
        }

        // GET: SchichtInfo/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                SQLServer.RunSqlCommand(new DataMapper<SchichtInfo>("SchichtInfo").GetDeleteSqlString(id));
                return RedirectToAction("Index");
            }
            catch
            {
                return View("~/Views/Shared/Error.cshtml");

            }
        }




    }
}
