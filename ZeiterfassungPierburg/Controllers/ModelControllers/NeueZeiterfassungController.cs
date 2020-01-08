using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;
using ZeiterfassungPierburg.Models.NeueZeiterfassung;

namespace ZeiterfassungPierburg.Controllers.ModelControllers
{
    public class NeueZeiterfassungController : Controller
    {
        static NeueZeiterfassung temp = new NeueZeiterfassung();
        int timesClicked = 0;

        // GET: NeueZeiterfassung
        public ActionResult Index()
        {
            return View();
        }

        // GET: NeueZeiterfassung/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NeueZeiterfassung/Create
        public ActionResult Create()
        {
            var model = new NeueZeiterfassung();
            model.Datum =  DateTime.Today;

         var date = DateTime.Now;
            if(date.Hour >= 22 || date.Hour <= 6)
            {
                model.Schicht = 3;
            }
            else if (date.Hour >= 14 && date.Hour <= 22)
            {
                model.Schicht = 2;
            }
            else if (date.Hour >= 6 && date.Hour <= 14)
            {
                model.Schicht = 1;
            }

            return View(model);
        }


       // POST: NeueZeiterfassung/Create
       public  ActionResult AddMitarbeiterHTML()
        {
            //ViewBag.NewMitarbeiter = PartialView("~/Views/NeueZeiterfassung/CreatePartial.cshtml", new NeueZeiterfassung());
            NeueZeiterfassung n = new NeueZeiterfassung();



            ViewBag.NewMitarbeiter = n.FertigungteilList;

            //return PartialView("~/Views/NeueZeiterfassung/CreatePartial.cshtml", new NeueZeiterfassung());
            return View("Create", temp);
        }

        [HttpPost]
        public ActionResult Create(string submit, NeueZeiterfassung model, FormCollection col)
        {
            
            switch (submit)
            {
                case "addMitarbeiter":
                    //temp = model;
                    return AddMitarbeiterHTML();

                case "Abschicken":
                

                    SchichtInfo s = new SchichtInfo()
                    {
                        Datum = model.Datum,
                        Art = model.Schicht
                    };
                    int id = SQLServer.Instance.InsertItem<SchichtInfo>(s);
                    /*
                    4   Durchlaufe die Formulardaten, bis der letzte Mitarbeiter gefunden wird
                    5   Erstelle pro Mitarbeiter in MitarbeiterInSchicht Modell
                    6   Setze alle zu einer Liste zusammen
                    7   Füge durch eine For-Schleife jedes einzelne MitarbeiterInSchichtModell
                         in die DB hinzu
                    */
                    try
                    {
                        /*
                        // Get all states again
                        var produktionsanlagen = ControllerHelper.SelectColumn("Produktionsanlage", "Bezeichner", "Bezeichner");
                        var mitarbeiter = ControllerHelper.SelectColumn("Mitarbeiter", ("Nachname" + "+' '+" + "Vorname"), "Nachname,Vorname");
                        var fertigungsteile = ControllerHelper.SelectColumn("Fertigungsteil", "ZeichenNr", "ZeichenNr");

                        // Set these states on the model. We need to do this because
                        // only the selected value from the DropDownList is posted back, not the whole
                        // list of states.
                        model.ProduktionsanlageList = ControllerHelper.GetSelectListItems(produktionsanlagen);
                        model.NameList = ControllerHelper.GetSelectListItems(mitarbeiter);
                        model.FertigungteilList = ControllerHelper.GetSelectListItems(fertigungsteile);


                        // In case everything is fine - i.e. both "Name" and "State" are entered/selected,
                        // redirect user to the "Done" page, and pass the user object along via Session
                        /*
                        if (ModelState.IsValid)
                        {
                            Session["NeueZeiterfassung"] = model;
                            return RedirectToAction("Done");
                        }
                        */
                        if (ModelState.IsValid)
                        {

                            NeueZeiterfassungDBHandler sdb = new NeueZeiterfassungDBHandler();
                            if (sdb.AddZeiterfassung(model))
                            {
                                ModelState.Clear();
                                ViewBag.Message = "Schicht erfasst";

                                return Create();
                            }

                        }
                        ViewBag.Message = "Fehler beim Hinzufügen in die Datenbank";

                        // Something is not right - so render the registration page again,
                        // keeping the data user has entered by supplying the model.
                        return View("Create", model);

                    }
                    catch
                    {
                        ViewBag.Message = "Die Eingabe ist falsch";
                        return Create();
                    }
             default:
                 throw new Exception();

         }
         
        }

        // GET: NeueZeiterfassung/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NeueZeiterfassung/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Done()
        {
            // Get information from the session
            var model = Session["NeueZeiterfassung"] as NeueZeiterfassung;

            // Display Done.html page that shows Name and selected state.
            return View(model);
        }

        // GET: NeueZeiterfassung/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NeueZeiterfassung/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


    }
}
