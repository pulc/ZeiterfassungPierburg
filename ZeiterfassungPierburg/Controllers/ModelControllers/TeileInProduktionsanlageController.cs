using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models.ViewModels;
using ZeiterfassungPierburg.Models.Produktion;

namespace ZeiterfassungPierburg.Controllers
{
    public class TeileInProduktionsanlageController : Controller
    {
        // GET: TeileInProduktionsanlage
        public ActionResult Index()
        {
            ViewBag.TeileInProduktionsanlageMessage = TempData["Message"];

            var results = SQLServer.Instance.GetTeileInProduktionsanlageView<TeileInProduktionsanlageViewModel>();

            return View(results);
        }

        // GET: TeileInProduktionsanlage/Edit/5
        public ActionResult Edit(int id)
        {
            var results = SQLServer.Instance.GetItems<TeileInProduktionsanlage>("id = " + id.ToString());
            if (results.Count() != 1)
            {
                // todo: implement proper error message to be displayed
                return HttpNotFound("Der TeileInProduktionsanlage wurde nicht gefunden.");
            }
            else
                return View(results.First());
        }


        // GET: Produktionsanlage/Create
        public ActionResult Create()
        {
            ModelState.Clear();
            CreateTeileInProduktionsanlageViewModel m = new CreateTeileInProduktionsanlageViewModel();
            return View(m);
        }

        [HttpPost]
        public ActionResult Create(string submit, CreateTeileInProduktionsanlageViewModel model, FormCollection col)
        {
            ViewBag.Message = "";
            List<int> InsertedID = new List<int>();

            switch (submit)
            {
  
                case "Abschicken":

                    try
                    {

                        List<TeileInProduktionsanlage> list = new List<TeileInProduktionsanlage>();

                        // Create the first Mitarbeiter model
                        TeileInProduktionsanlage m = new TeileInProduktionsanlage() //first 
                        {
                            FertigungsteilID = Int32.Parse(Request.Form["Fertigungsteil"]),
                            ProduktionsanlageID = Int32.Parse(Request.Form["Produktionsanlage"])
                        };
                        list.Add(m);

                        Dictionary<string, string> form = col.AllKeys.ToDictionary(k => k, v => col[v]);

                        int TeileToAdd = (col.Count - 4) / 2; //Count how many additionaly entires there are

                        if (TeileToAdd != 0)
                        {
                            for (int i = 1; i <= TeileToAdd; i++)
                            {
                                TeileInProduktionsanlage n = new TeileInProduktionsanlage()
                                {
                                    FertigungsteilID = Int32.Parse(Request.Form["fteil" + i]),
                                    ProduktionsanlageID = Int32.Parse(Request.Form["panlage" + i])
                                };
                                list.Add(n);
                            }
                        }
                        // add all models into the DB
                        foreach (var n in list)
                        {
                            InsertedID.Add(SQLServer.Instance.InsertItem<TeileInProduktionsanlage>(n));
                        }
                        ViewBag.Message = list.Count + " Fertigungsteile erfolgreich hinzugefügt";
                        return Create();
                    }
                    catch (Exception e)
                    {
                        // delete added models (if any)
                        foreach (var id in InsertedID)
                        {
                            SQLServer.Instance.RemoveItem<TeileInProduktionsanlage>(id);
                        }
                        ViewBag.Message = "Die Eingabe ist falsch. Keine Teile sind hinzugefügt worden.";
                        ViewBag.ErrorMessage = "Der Grund: " +e.Message;
                        return View(model);
                    }
                default:
                    throw new Exception();
            }
        }

        [HttpPost]
        public ActionResult Edit(TeileInProduktionsanlage m)
        {
            try
            {
                SQLServer.Instance.EditItem<TeileInProduktionsanlage>(m);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(m);
            }
        }
        // GET: TeileInProduktionsanlage/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                SQLServer.Instance.RemoveItem<TeileInProduktionsanlage>(id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Der TeileInProduktionsanlage konnte nicht gelöscht werden.";
                //return Index();
                return RedirectToAction("Index");
            }
        }
    }
}
