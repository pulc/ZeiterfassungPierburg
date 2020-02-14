﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;
using ZeiterfassungPierburg.Models.ViewModel.MitarbeiterInschichtViewModel;

namespace ZeiterfassungPierburg.Controllers
{
    public class MitarbeiterInSchichtController : Controller
    {
        // GET: MitarbeiterInSchicht
        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];

            var results = SQLServer.Instance.GetMitarbeiterInSchichtModel<MitarbeiterInschichtViewModel>();
            return View(results);
        }
        public ActionResult IndexMEBA()
        {
            var results = SQLServer.Instance.GetMitarbeiterInSchichtModelMEBA<MitarbeiterInschichtViewModel>();
            return View(results);
        }

        // GET: MitarbeiterInSchicht/Edit/5
        public ActionResult Edit(int id)
        {
            var results = SQLServer.Instance.GetItems<MitarbeiterInSchicht>("id = " + id.ToString());
            if (results.Count() != 1)
            {
                TempData["Message"] = "Der Eintrag wurde nicht gefunden";

                // todo: implement proper error message to be displayed
                return RedirectToAction("Index");
            }
            else
            return View(results.First());
        }

        // GET: MitarbeiterInSchicht/Create
        public ActionResult Create()
        {
            return RedirectToAction("Index", "NeuezeiterfassungController");
        }

        // GET: MitarbeiterInSchicht/Create
        public ActionResult Eintragen(bool istEingetragen, int id)
        {

            bool? val = SQLServer.Instance.GetBoolean("Select [IstInSAPEingetragen] from [MitarbeiterInSchicht] where ID=" + id);
            // TODO: find out istEingetrage for each click
            string cmd = "";
            if(val == true)
            { 
            cmd =
               "update MitarbeiterInSchicht set istInSAPeingetragen = 'false' where ID = " + id;
            }
            else if (val == false)
            {
                cmd =
               "update MitarbeiterInSchicht set istInSAPeingetragen = 'true' where ID = " + id;
            }
            else
            {
                TempData["Message"] = "Ein Fehler ist aufgetreten";
                return RedirectToAction("Index");

            }
            int success = SQLServer.Instance.ExecuteCommand(cmd);

            return Json(success, JsonRequestBehavior.AllowGet);
        }



        // POST: MitarbeiterInSchicht/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {

                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(MitarbeiterInSchicht m)
        {
            try
            {
                SQLServer.Instance.EditItem<MitarbeiterInSchicht>(m);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(m);
            }
        }

        [Authorize(Users = Startup.Administrators)]
        public ActionResult Delete(int id)
        {
            try
            {
                if (Convert.ToInt32(Session["AccessLayer"]) == 1 || Convert.ToInt32(Session["AccessLayer"]) == 2)
                {
                    SQLServer.Instance.RemoveItem<MitarbeiterInSchicht>(id);
                }
                return RedirectToAction("Index");
            }
            catch (InvalidCastException e)
            {
                TempData["Message"] = "Der Eintrag konnte nicht gelöscht werden. Der Grund:" +e;
                //return Index();
                return RedirectToAction("Index");
            }
        }
       
    }
}
