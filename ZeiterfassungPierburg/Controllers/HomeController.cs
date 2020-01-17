using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models.ViewModels;

namespace ZeiterfassungPierburg.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            Dashboard d = new Dashboard();
            ViewBag.MitarbeiterAnzahl = d.MitarbeiterAnzahl;
            ViewBag.StückeHeute = d.StückeHeute;
            ViewBag.StückeMinusOneDay = d.StückeMinusOneDay;
            ViewBag.StückeMinusTwoDays = d.StückeMinusTwoDays;

            ViewBag.Produktionsanlagen = d.Produktionsanlagen;
            ViewBag.Fertigungsteile = d.Fertigungsteile;
            ViewBag.StückeWoche = d.StückeWoche;
            ViewBag.Zeiterfassungen = d.ZeiterfassungenAnzahl;
            /*
            ViewBag.StückeMinusThreeDays = d.StückeMinusThreeDays;
            ViewBag.StückeMinusFourDays = d.StückeMinusFourDays;
            */
            ViewBag.StückeWoche = d.StückeWoche;
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}