using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;
using ZeiterfassungPierburg.Models.NeueZeiterfassung;
using ZeiterfassungPierburg.Models.NeuezeiterfassungMEBA;

namespace ZeiterfassungPierburg.Controllers.ModelControllers
{
    public class NeueZeiterfassungController : Controller
    {
        static NeueZeiterfassung temp = new NeueZeiterfassung();

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

            ModelState.Clear();
            var model = new NeueZeiterfassung();
            model.Datum = DateTime.Today;

            var date = DateTime.Now;
            if (date.Hour >= 22 || date.Hour <= 6)
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
        public ActionResult CreateMEBA()
        {
            ModelState.Clear();
            var model = new NeuezeiterfassungMEBA();
            model.Datum = DateTime.Today;

            var date = DateTime.Now;
            if (date.Hour >= 22 || date.Hour <= 6)
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

        /* get Fertigungsteile depending on the ID of the Produktionsanlage
         * it gets called each time the Produktionsanlage from the dropdown list is changed from the client side 
         * the ID gets passed
         */
        public JsonResult FetchFertigungsteile(int produktionsanlageID)
        {
            Dictionary<int, string> fteileDictionary = SQLServer.Instance.GetFertigungsteilDictionary(produktionsanlageID);
            string json = JsonConvert.SerializeObject(fteileDictionary, Formatting.Indented);

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Create(NeueZeiterfassung model, FormCollection col)
        {
            ViewBag.Message = "";
            List<int> InsertedID = new List<int>();
            SchichtInfo s = new SchichtInfo();

            try
            {
                // retrieve Datum and Schicht directly from the model and create a new SchichInfo model class
                s.Datum = model.Datum;
                s.Art = model.Schicht;

                int SchichtInfoID = SQLServer.Instance.InsertItem<SchichtInfo>(s);
                int ProduktionsanlageID = model.Produktionsanlage;
                List<MitarbeiterInSchicht> MitarbeiterInSchichtList = new List<MitarbeiterInSchicht>();

                var date = DateTime.Now;
                // truncate seconds and miliseconds
                date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, 0, date.Kind);
                //string ErstelltVon = System.Web.HttpContext.Current.User.Identity.Name;
                string ErstelltVon = "unbekannt";
                if(Session["UserName"] != null) 
                {
                    ErstelltVon = Session["UserName"].ToString();
                }

                Dictionary<string, string> form = col.AllKeys.ToDictionary(k => k, v => col[v]); // get the whole collection
                List<string> keyList = new List<string>(form.Keys);

                int TeileCount = 0; //how many Teile should get added
                int MitarbeiterCount = 0; //how many Mitarbeiter should get added
                foreach (string name in keyList)
                {
                    if (name.Contains("fteil"))
                        TeileCount++;
                    if (name.Contains("name"))
                        MitarbeiterCount++;
                }
                if (MitarbeiterCount != 0)
                {
                    for (int i = 0; i < MitarbeiterCount; i++)
                    {
                        // check if the there is an entry where dirzeit + indirzeit = 0
                        // TODO: do this action in Javascript in the View
                        if ((float.Parse(Request.Form["dirzeit" + i]) + float.Parse(Request.Form["indirzeit" + i])) == 0)
                        {
                            foreach (var id in InsertedID)
                            {
                                SQLServer.Instance.RemoveItem<MitarbeiterInSchicht>(id);
                            }
                            ViewBag.Message = "Die Summe von direkten und indirekten Stunden darf bei keinem Mitarbeiter 0 sein. Keine Mitarbeiter wurden erfasst.";
                            return View(model);
                        }
                        // auswertung = ((Int32.Parse(Request.Form["st" + i]) / (float.Parse(Request.Form["dirzeit" + i]) + float.Parse(Request.Form["indirzeit" + i]))) * 100) / teZeit;

                        // TODO: berechnen Fertigungsteil ID 
                        for (int j = 0; j < TeileCount; j++)
                        {
                            //string cmd = $"select id from Fertigungsteil where Bezeichnung ='{Request.Form["fteil" + j]}'"; 
                            //int FertigungsTeilID = SQLServer.Instance.GetInt(cmd); //getFertigungsteilID

                            MitarbeiterInSchicht n = new MitarbeiterInSchicht()
                            {
                                SchichtInfoID = SchichtInfoID,
                                FertigungsteilID = Int32.Parse(Request.Form["fteil" + j]),
                                MitarbeiterID = Int32.Parse(Request.Form["name" + i]),
                                DirStunden = float.Parse(Request.Form["dirzeit" + i], CultureInfo.InvariantCulture),
                                InDirStunden = float.Parse(Request.Form["indirzeit" + i], CultureInfo.InvariantCulture),
                                Stück = Int32.Parse(Request.Form["st" + j]),
                                ProduktionsanlageID = ProduktionsanlageID,
                                ErstelltAm = date,
                                Bemerkung = Request.Form["bemerkung" + i],
                                //Auswertung = auswertung,
                                EingetragenVon = ErstelltVon
                            };
                            MitarbeiterInSchichtList.Add(n);
                        }
                    }
                }
                // add all models into the DB
                foreach (var n in MitarbeiterInSchichtList)
                {
                    InsertedID.Add(SQLServer.Instance.InsertItem<MitarbeiterInSchicht>(n));
                }
                if(MitarbeiterInSchichtList.Count == 1)
                {
                    ViewBag.Message = "1 Eintrag gespeichert.";
                }
                else { 

                    if(TeileCount == 1)
                    {
                        ViewBag.Message = MitarbeiterCount + " Mitarbeiter und 1 Produkt erfasst";
                    }
                    else
                    {
                        ViewBag.Message = MitarbeiterCount + " Mitarbeiter und " + TeileCount + " Produkte erfasst";
                    }
                }
                return Create();
            }
            catch (Exception e)
            {

                // delete added models (if any)
                foreach (var id in InsertedID)
                {
                    SQLServer.Instance.RemoveItem<MitarbeiterInSchicht>(id);
                }
                SQLServer.Instance.RemoveItem<SchichtInfo>(s); 

                ViewBag.Message = "Die Eingabe ist falsch. Keine Mitarbeiter sind hinzugefügt worden." +
                    " Der ausführliche Grund: " + e.Message;

                return View(model);
            }
        }
        [HttpPost]
        public ActionResult CreateMEBA(NeuezeiterfassungMEBA model, FormCollection col)
        {
            ViewBag.Message = "";
            List<int> InsertedID = new List<int>();
            SchichtInfo s = new SchichtInfo();


            try
            {
                s.Datum = model.Datum;
                s.Art = model.Schicht;

                int SchichtInfoID = SQLServer.Instance.InsertItem<SchichtInfo>(s);
                int ProduktionsanlageID = model.Produktionsanlage;
                int MitarbeiterID = model.Name;

                List<MitarbeiterInSchicht> MitarbeiterInSchichtList = new List<MitarbeiterInSchicht>();

                var date = DateTime.Now;
                // truncate seconds and miliseconds
                date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, 0, date.Kind);
                //string ErstelltVon = System.Web.HttpContext.Current.User.Identity.Name;

                string ErstelltVon = "unbekannt";
                if (Session["UserName"] != null)
                {
                    ErstelltVon = Session["UserName"].ToString();
                }

                Dictionary<string, string> form = col.AllKeys.ToDictionary(k => k, v => col[v]);
                List<string> keyList = new List<string>(form.Keys);
                int TeileCount = 0;
                foreach (string name in keyList)
                {
                    if (name.Contains("fteil"))
                        TeileCount++;
                }

                for (int i = 0; i < TeileCount; i++)
                {
                    //auswertung = 0;
                    if ((float.Parse(Request.Form["dirzeit" + i]) + float.Parse(Request.Form["indirzeit" + i])) == 0)
                    {
                        foreach (var id in InsertedID)
                        {
                            SQLServer.Instance.RemoveItem<MitarbeiterInSchicht>(id);
                        }
                        ViewBag.Message = "Die Summe von direkten und indirekten Stunden darf  bei keinem Mitarbeiter 0 sein. Keine Mitarbeiter wurden erfasst.";
                        return View(model);
                    }
                    // float auswertung = ((Int32.Parse(Request.Form["st" + i]) / (float.Parse(Request.Form["dirzeit" + i]) + float.Parse(Request.Form["indirzeit" + i]))) * 100) / teZeit;
                    
                    MitarbeiterInSchicht n = new MitarbeiterInSchicht()
                    {
                        SchichtInfoID = SchichtInfoID,
                        FertigungsteilID = Int32.Parse(Request.Form["fteil" + i]),
                        MitarbeiterID = MitarbeiterID,
                        DirStunden = float.Parse(Request.Form["dirzeit" + i], CultureInfo.InvariantCulture),
                        InDirStunden = float.Parse(Request.Form["indirzeit" + i], CultureInfo.InvariantCulture),
                        Stück = Int32.Parse(Request.Form["st" + i]),
                        ProduktionsanlageID = ProduktionsanlageID,
                        ErstelltAm = date,
                        Bemerkung = Request.Form["bemerkung" + i],
                        EingetragenVon = ErstelltVon
                    };
                    MitarbeiterInSchichtList.Add(n);
                }

                // add all models into the DB
                foreach (var n in MitarbeiterInSchichtList)
                {
                    InsertedID.Add(SQLServer.Instance.InsertItem<MitarbeiterInSchicht>(n));
                }
                if(MitarbeiterInSchichtList.Count == 1)
                { 
                ViewBag.Message = "1 Produkt erfasst";
                }
                else
                {
                    ViewBag.Message = MitarbeiterInSchichtList.Count + " Produkte erfasst";
                }
                return CreateMEBA();
            }
            catch (Exception e)
            {
                // delete added models (if any)
                foreach (var id in InsertedID)
                {
                    SQLServer.Instance.RemoveItem<MitarbeiterInSchicht>(id);
                }
                SQLServer.Instance.RemoveItem<SchichtInfo>(s);


                ViewBag.Message = "Die Eingabe ist falsch. Keine Mitarbeiter sind hinzugefügt worden." +
                    "\nDer ausführliche Grund: " + e.Message;


                return View(model);
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

        /*
        private float calculateAuswertung (float stückzahl, float gettezeit, float dirst, float indirst)
        {
            float teZeit = (float)SQLServer.Instance.GetDecimal(gettezeit);
            float auswertung = 0;
            if ((model.InDirZeit + model.DirZeit) != 0)
            {
                auswertung = ((model.Stückzahl / (model.DirZeit + model.InDirZeit)) * 100) / teZeit;
            }
        }
        */
    }
}
