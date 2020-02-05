using System;
using System.Collections.Generic;
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
            var lstCities = new SelectList(new[] { "Es gibt keine Teile für diese Produktionsanalage. Du musst erstmal bei 'Teile bei Anlagen' eine hinzufügen" });
            ViewBag.Cities = lstCities;


            ModelState.Clear();
            var model = new NeueZeiterfassung();
            model.Datum = DateTime.Today;

            //ViewBag.FertigungsteileList = model.FertigungteilList;

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

        public JsonResult FetchFertigungsteile(int produktionsanlageID) // its a GET, not a POST
        {
            List<string> fteile = SQLServer.Instance.GetFertigungsteilList(produktionsanlageID);

            return Json(fteile, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Create(NeueZeiterfassung model, FormCollection col)
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
                List<MitarbeiterInSchicht> MitarbeiterInSchichtList = new List<MitarbeiterInSchicht>();

                var date = DateTime.Now;
                // truncate seconds and miliseconds
                date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, 0, date.Kind);
                string ErstelltVon = System.Web.HttpContext.Current.User.Identity.Name;

                /*
                string getTeZeit = @"select teZEIT from 
Fertigungsteil
where ID = " + FertigungsTeilID;

                float teZeit = (float) SQLServer.Instance.GetDecimal(getTeZeit);
                if(teZeit == 0)
                {
                    ViewBag.Message = "Kein Mitarbeiter wurde hinzugefügt. Es gibt kein Teil der Produktionsanlage zugeteilt oder die teZeit des Teiles ist 0.";
                    return View(model);

                }
                */

                Dictionary<string, string> form = col.AllKeys.ToDictionary(k => k, v => col[v]);
                List<string> keyList = new List<string>(form.Keys);
                int TeileCount = 0;
                int MitarbeiterCount = 0;
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
                        //float auswertung = 0;
                        if ((float.Parse(Request.Form["dirzeit" + i]) + float.Parse(Request.Form["indirzeit" + i])) == 0)
                        {
                            foreach (var id in InsertedID)
                            {
                                SQLServer.Instance.RemoveItem<MitarbeiterInSchicht>(id);
                            }
                            ViewBag.Message = "Die Summe von direkten und indirekten Stunden darf nicht 0 sein. Keine Mitarbeiter wurden erfasst.";
                            return View(model);
                        }
                        // auswertung = ((Int32.Parse(Request.Form["st" + i]) / (float.Parse(Request.Form["dirzeit" + i]) + float.Parse(Request.Form["indirzeit" + i]))) * 100) / teZeit;

                        // TODO: berechnen Fertigungsteil ID 
                        for (int j = 0; j < TeileCount; j++)
                        {
                            string getNumber = $"select id from Fertigungsteil where Bezeichnung ='{Request.Form["fteil" + j]}'";
                            int FertigungsTeilID = SQLServer.Instance.GetNumber(getNumber);

                            MitarbeiterInSchicht n = new MitarbeiterInSchicht()
                            {
                                SchichtInfoID = SchichtInfoID,
                                FertigungsteilID = FertigungsTeilID,
                                MitarbeiterID = Int32.Parse(Request.Form["name" + i]),
                                DirStunden = float.Parse(Request.Form["dirzeit" + i]),
                                InDirStunden = float.Parse(Request.Form["indirzeit" + i]),
                                Stück = Int32.Parse(Request.Form["st" + i]),
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
                ViewBag.Message = MitarbeiterInSchichtList.Count + " Einträge gemacht.";
                return Create();
            }
            catch (Exception e)
            {
                SQLServer.Instance.RemoveItem<SchichtInfo>(s);

                // delete added models (if any)
                foreach (var id in InsertedID)
                {
                    SQLServer.Instance.RemoveItem<MitarbeiterInSchicht>(id);
                }
                ViewBag.Message = "Die Eingabe ist falsch. Keine Mitarbeiter sind hinzugefügt worden." +
                    "\nDer ausführliche Grund: " + e.Message;

                return View(model);
            }
        }
        [HttpPost]
        public ActionResult CreateMEBA(NeuezeiterfassungMEBA model, FormCollection col)
        {
            ViewBag.Message = "";
            List<int> InsertedID = new List<int>();

            try
            {
                SchichtInfo s = new SchichtInfo()
                {
                    Datum = model.Datum,
                    Art = model.Schicht
                };

                int SchichtInfoID = SQLServer.Instance.InsertItem<SchichtInfo>(s);
                int ProduktionsanlageID = model.Produktionsanlage;
                int MitarbeiterID = model.Name;

                List<MitarbeiterInSchicht> MitarbeiterInSchichtList = new List<MitarbeiterInSchicht>();

                var date = DateTime.Now;
                // truncate seconds and miliseconds
                date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, 0, date.Kind);
                string ErstelltVon = System.Web.HttpContext.Current.User.Identity.Name;

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
                        ViewBag.Message = "Die Summe von direkten und indirekten Stunden darf nicht 0 sein. Keine Mitarbeiter wurden erfasst.";
                        return View(model);
                    }
                    // float auswertung = ((Int32.Parse(Request.Form["st" + i]) / (float.Parse(Request.Form["dirzeit" + i]) + float.Parse(Request.Form["indirzeit" + i]))) * 100) / teZeit;

                    string getNumber = $"select id from Fertigungsteil where Bezeichnung ='{Request.Form["fteil" + i]}'";
                    int FertigungsTeilID = SQLServer.Instance.GetNumber(getNumber);

                    MitarbeiterInSchicht n = new MitarbeiterInSchicht()
                    {
                        SchichtInfoID = SchichtInfoID,
                        FertigungsteilID = FertigungsTeilID,
                        MitarbeiterID = MitarbeiterID,
                        DirStunden = float.Parse(Request.Form["dirzeit" + i]),
                        InDirStunden = float.Parse(Request.Form["indirzeit" + i]),
                        Stück = Int32.Parse(Request.Form["st" + i]),
                        ProduktionsanlageID = ProduktionsanlageID,
                        ErstelltAm = date,
                        Bemerkung = Request.Form["bemerkung" + i],
                        //Auswertung = auswertung,
                        EingetragenVon = ErstelltVon
                    };
                    MitarbeiterInSchichtList.Add(n);
                }

                // add all models into the DB
                foreach (var n in MitarbeiterInSchichtList)
                {
                    InsertedID.Add(SQLServer.Instance.InsertItem<MitarbeiterInSchicht>(n));
                }
                ViewBag.Message = MitarbeiterInSchichtList.Count + " Produkte erfasst";
                return CreateMEBA();
            }
            catch (Exception e)
            {
                // delete added models (if any)
                foreach (var id in InsertedID)
                {
                    SQLServer.Instance.RemoveItem<MitarbeiterInSchicht>(id);
                }
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

        private int ConvertStringToInt(string intString)
        {
            int? c = null;
            c = (Int32.TryParse(intString, out int i) ? c : (int?)null);

            return i;
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
