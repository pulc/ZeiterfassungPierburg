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
        [HttpPost]
        public ActionResult Create(Produktionsanlage m)
        {
            try
            {
                SQLServer.RunSqlCommand(new DataMapper<Produktionsanlage>("Produktionsanlage").GetInsertSqlString(m));

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }
        /*
        // POST: Produktionsanlage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Dictionary<string, string> form = collection.AllKeys.ToDictionary(k => k, v => collection[v]);
                form.Remove("__RequestVerificationToken");
                //bool:_ true,false
                Produktionsanlage m = new Produktionsanlage();

                foreach (KeyValuePair<string, string> entry in form)
                {
                    m.SetValue(entry.Value, entry.Key);
                }
                SQLServer.RunSqlCommand(new DataMapper<Produktionsanlage>("Produktionsanlage").GetInsertSqlString(m));

                return RedirectToAction("Index");
            }
            catch (Exception t)
            {
                return View();
            }
        }
        */
        // POST: Produktionsanlage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Dictionary<string, string> form = collection.AllKeys.ToDictionary(k => k, v => collection[v]);

                Produktionsanlage m = new Produktionsanlage();

                foreach (KeyValuePair<string, string> entry in form)
                {
                    m.SetValue(entry.Value, entry.Key);
                }
                SQLServer.RunSqlCommand(new DataMapper<Produktionsanlage>("Produktionsanlage").GetUpdateSqlString(m));

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        // GET: Produktionsanlage/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                SQLServer.RunSqlCommand(new DataMapper<Produktionsanlage>("Produktionsanlage").GetDeleteSqlString(id));
                return RedirectToAction("Index");
            }
            catch
            {

                return View("~/Views/Shared/Error.cshtml");

            }
        }
    }
}
