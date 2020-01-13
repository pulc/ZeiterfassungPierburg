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


        // POST: NeueZeiterfassung/Create
        public ActionResult AddMitarbeiterHTML()
        {
            //ViewBag.NewMitarbeiter = PartialView("~/Views/NeueZeiterfassung/CreatePartial.cshtml", new NeueZeiterfassung());
            //NeueZeiterfassung n = new NeueZeiterfassung();
            //ViewBag.NewMitarbeiter = n.FertigungteilList;

            //return PartialView("~/Views/NeueZeiterfassung/CreatePartial.cshtml", new NeueZeiterfassung());
            return View("Create", temp);
        }

        [HttpPost]
        public ActionResult Create(string submit, NeueZeiterfassung model, FormCollection col)
        {
            ViewBag.Message = "";
            List<int> InsertedID = new List<int>();



            switch (submit)
            {
                case "addMitarbeiter":
                    temp = model;
                    return AddMitarbeiterHTML();

                case "Abschicken":

                    try
                    {
                        SchichtInfo s = new SchichtInfo()
                        {
                            Datum = model.Datum,
                            Art = model.Schicht
                        };
                        int SchichtInfoID = SQLServer.Instance.InsertItem<SchichtInfo>(s);
                        int ProduktionsanlageID = model.Produktionsanlage;
                        List<MitarbeiterInSchicht> MitarbeiterInSchichtList = new List<MitarbeiterInSchicht>();


                        // Create the first Mitarbeiter model
                        MitarbeiterInSchicht m = new MitarbeiterInSchicht() //first 
                        {
                            SchichtInfoID = SchichtInfoID,
                            FertigungsteilID = model.Fertigungsteil,
                            MitarbeiterID = model.Name,
                            DirStunden = model.DirZeit,
                            InDirStunden = model.InDirZeit,
                            Stück = model.Stückzahl,
                            ProduktionsanlageID = ProduktionsanlageID,
                            ErstelltAm = DateTime.Now


                        };
                        MitarbeiterInSchichtList.Add(m);

                        Dictionary<string, string> form = col.AllKeys.ToDictionary(k => k, v => col[v]);

                        int MitarbeiterToAdd = (col.Count - 10) / 5; //Count how many additionaly Mitarbeiter model there are

                        if (MitarbeiterToAdd != 0)
                        {
                            for (int i = 1; i <= MitarbeiterToAdd; i++)
                            {
                                MitarbeiterInSchicht n = new MitarbeiterInSchicht()
                                {
                                    SchichtInfoID = SchichtInfoID,
                                    FertigungsteilID = Int32.Parse(Request.Form["fteil" + i]),
                                    MitarbeiterID = Int32.Parse(Request.Form["name" + i]),
                                    DirStunden = float.Parse(Request.Form["dirzeit" + i]),
                                    InDirStunden = float.Parse(Request.Form["indirzeit" + i]),
                                    Stück = Int32.Parse(Request.Form["st" + i]),
                                    ProduktionsanlageID = ProduktionsanlageID,
                                    ErstelltAm = DateTime.Now
                                };
                                MitarbeiterInSchichtList.Add(n);
                            }
                        }
                        // add all models into the DB
                        foreach (var n in MitarbeiterInSchichtList)
                        {
                            InsertedID.Add(SQLServer.Instance.InsertItem<MitarbeiterInSchicht>(n));
                        }
                        ViewBag.Message = MitarbeiterInSchichtList.Count + " Mitarbeiter erfasst";
                        return Create();
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

                       return View("Create", model);
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

        private int ConvertStringToInt(string intString)
        {
            int? c = null;
            c = (Int32.TryParse(intString, out int i) ? c : (int?)null);

            return i;
        }


    }
}
