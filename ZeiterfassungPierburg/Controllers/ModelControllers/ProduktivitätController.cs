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

            string message = " ";

        
            if(m.Day != 0)
            {
                message = message + " Tag:" + m.Day;
            ViewBag.Day = m.Day;
            }
            if(m.Month != 0)
            {
                message = message + " Monat:" + m.Month;

                ViewBag.Month = m.Month;
            }
            if(m.Year != 0)
            {
                message = message + " Jahr:" + m.Year;

                ViewBag.Year = m.Year;
            }
            
            if (m.ProduktionsanlageID != 0)
            {
                message = message + " ProduktionsanlageID:" + m.FertigungsteilID;

                ViewBag.Year = m.Year;
            }
            if (m.FertigungsteilID != 0)
            {
                message = message + " FertigungsteilID:" + m.ProduktionsanlageID;

                ViewBag.Year = m.Year;
            }
            if (m.MitarbeiterID != 0)
            {
                message = message + " MitarbeiterID:" + m.MitarbeiterID;

                ViewBag.Year = m.Year;
                ViewBag.MitarbeiterBeschreibung = "Produktivität des folgenden Mitarbeiters:";
            }
            if (m.Art != 0)
            {
                message = message + " SchichtID:" + m.Art;

                ViewBag.Year = m.Year;
            }

            ViewBag.Message = message;

            /*
            ViewBag.day = m.Day;
            ViewBag.day = m.Day;
            */

            var results = SQLServer.Instance.GetProduktivitätViewModel<ProduktivitätViewModel>(m.Day, m.Month, m.Year, m.ProduktionsanlageID, m.FertigungsteilID, m.MitarbeiterID, m.Art);
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
