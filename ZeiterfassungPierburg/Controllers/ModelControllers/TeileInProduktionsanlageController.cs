using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models.ViewModels;
using ZeiterfassungPierburg.Models.Produktion;
using System.Data.SqlClient;

namespace ZeiterfassungPierburg.Controllers
{
    // need refactoring - add EditViewModel 
    public class TeileInProduktionsanlageController : Controller
    {
        // GET: TeileInProduktionsanlage
        public ActionResult Index()
        {
            ViewBag.TeileInProduktionsanlageMessage = TempData["Message"];

            var results = SQLServer.Instance.GetTeileInProduktionsanlageView<TeileInProduktionsanlageViewModel>();

            return View(results);
        }

        public ActionResult Edit(int id)
        {
            List<string> results = SQLServer.Instance.GetListTeileInProduktionsanlageEdit(id);
            CreateTeileInProduktionsanlageViewModel m = new CreateTeileInProduktionsanlageViewModel();
            
            m.ProduktionsanlageBezeichner = results[0];
            m.Produktionsanlage = Int32.Parse(results[1]);
            m.Fertigungsteil = Int32.Parse(results[2]);
            m.ID = id;

            return View(m);
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

                        // Create the first TeileInProduktionsanlage model
                        TeileInProduktionsanlage m = new TeileInProduktionsanlage() //first 
                        {
                            FertigungsteilID = Int32.Parse(Request.Form["Fertigungsteil"]),
                            ProduktionsanlageID = Int32.Parse(Request.Form["Produktionsanlage"])
                        };
                        list.Add(m);

                        Dictionary<string, string> form = col.AllKeys.ToDictionary(k => k, v => col[v]);

                        int TeileToAdd = (col.Count - 4) / 2; //Count how many additionaly entires there are; 
                        // 4 = number of entries in CollectionForm for the model

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
                        // TODO: Bedingungen für keine doppelte Eingaben für Band  und Teile

                        // add all models into the DB
                        foreach (var n in list)
                        {
                            InsertedID.Add(SQLServer.Instance.InsertItem<TeileInProduktionsanlage>(n));
                        }
                        if (list.Count == 1)
                        {
                            ViewBag.Message = list.Count + " Fertigungsteil erfolgreich hinzugefügt";
                        }
                        else
                        {
                            ViewBag.Message = list.Count + " Fertigungsteile erfolgreich hinzugefügt";
                        }

                        return Create();
                        /*
                         * 
                        // checks how many Maschine there are
                        int checkcount = SQLServer.Instance.GetNumber(@"select COUNT(Bezeichner) from 
TeileInProduktionsanlage left outer join Produktionsanlage on Produktionsanlage.ID = TeileInProduktionsanlage.ProduktionsanlageID
where Produktionsanlage.IstEineMaschine = 'false'");
                        // checks how many distinct Maschine there are
                        int checkcountDistinct = SQLServer.Instance.GetNumber(@"select count(distinct Bezeichner) from 
TeileInProduktionsanlage left outer join Produktionsanlage on Produktionsanlage.ID = TeileInProduktionsanlage.ProduktionsanlageID
where Produktionsanlage.IstEineMaschine = 'false'");

                        // if the user added more Fertigungsteile to a Band, they need to repeat the action
                        if (checkcount == checkcountDistinct)
                        {
                            if(list.Count ==1)
                            { 
                            ViewBag.Message = list.Count + " Fertigungsteil erfolgreich hinzugefügt";
                            }
                            else
                            {
                            ViewBag.Message = list.Count + " Fertigungsteile erfolgreich hinzugefügt";
                            }

                            return Create();

                        }
                        else { 
                        
                        foreach (var id in InsertedID)
                        {
                            SQLServer.Instance.RemoveItem<TeileInProduktionsanlage>(id);
                        }
                        ViewBag.Message = list.Count + " Fertigungsteile hinzugefügt.";
                        /*
                        ViewBag.Message = "Die Eingabe war falsch. Du hast einen Teil zu einem Band hinzugefügt, dem bereits ein Teil zugewiesen wurde." +
                                " Keine Teile wurden hinzugefügt.";

                        return View(model);
                        }
                        */

                    }
                    catch (Exception e)
                    {
                        // delete added models (if any)
                        foreach (var id in InsertedID)
                        {
                            SQLServer.Instance.RemoveItem<TeileInProduktionsanlage>(id);
                        }
                        ViewBag.Message = "Die Eingabe ist falsch. Keine Teile sind hinzugefügt worden.";
                        ViewBag.ErrorMessage = "Der Grund: " + e.Message;
                        return View(model);
                    }
                default:
                    throw new Exception();
            }
        }


        [HttpPost]
        public ActionResult Edit(CreateTeileInProduktionsanlageViewModel m, FormCollection col)
        {
            try
            {
                string fteil = Request.Form["Fertigungsteil"];
                string id = Request.Form["ID"];

                string sqlstring = @"
   UPDATE [dbo].[TeileInProduktionsanlage]
   SET    [FertigungsteilID] = " + fteil + " WHERE ID = " + id;

                int success = SQLServer.Instance.ExecuteCommand(sqlstring);
                TempData["Message"] = "Die Anlage wurde geändet.";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Message"] = "Die Anlage kann nicht bearbeitet werden.";

                return View(m);
            }
        }

        public ActionResult Delete(int id)
        {
            if (Convert.ToInt32(Session["AccessLayer"]) == 1 || Convert.ToInt32(Session["AccessLayer"]) == 2)
            {
                try
                {
                    SQLServer.Instance.RemoveItem<TeileInProduktionsanlage>(id);
                    return RedirectToAction("Index");

                    /*
                    int fteileCount = SQLServer.Instance.GetInt(@"SELECT COUNT(*)
  FROM[zeiterfassung].[dbo].[TeileInProduktionsanlage] left outer join Produktionsanlage on TeileInProduktionsanlage.ProduktionsanlageID = Produktionsanlage.ID
  where TeileInProduktionsanlage.ID = " + id);

                    if (fteileCount != 1)
                    {
                        SQLServer.Instance.RemoveItem<TeileInProduktionsanlage>(id);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "Eine Produktionsanlage muss mindestens einen Teil haben.";

                        return RedirectToAction("Index");
                    }
                    */
                }
                catch
                {
                    TempData["Message"] = "Der Eintrag konnte nicht gelöscht werden.";
                    //return Index();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
