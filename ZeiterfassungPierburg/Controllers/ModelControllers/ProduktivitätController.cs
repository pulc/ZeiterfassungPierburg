using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;
using ZeiterfassungPierburg.Models.ViewModels;


namespace ZeiterfassungPierburg.Controllers.ModelControllers
{
    public class ProduktivitätController : Controller
    {
        
        // GET: Produktivität
        public ActionResult Index(ProduktivitätViewModel m)
        {
            var results = SQLServer.Instance.GetProduktivitätViewModel<ProduktivitätViewModel>(m.Day, m.Month, m.Year, m.ProduktionsanlageID, m.FertigungsteilID);
            return View(results);
        }
        
        // GET: Produktivität/Edit/5
        public ActionResult Edit(int id)
        {
            var results = SQLServer.Instance.GetItems<ProduktivitätViewModel>("id = " + id.ToString());
            if (results.Count() != 1)
            {
                TempData["Message"] = "Der Eintrag wurde nicht gefunden";
                return RedirectToAction("Index");
            }
            else
            return View(results.First());
        }

        // GET: Produktivität/Create
        public ActionResult Create()
        {
            ProduktivitätViewModel p = new ProduktivitätViewModel();
            return View(p);
        }

        // POST: Produktivität/Create
        [HttpPost]
        public ActionResult Create(ProduktivitätViewModel m)
        {
            try
            {
                return RedirectToAction("Index", m);
            }
            catch
            {
                return View();
            }
        }
    }
}
