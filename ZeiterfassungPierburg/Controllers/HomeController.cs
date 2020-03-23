using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
                        
            //ViewBag.Name = System.Web.HttpContext.Current.User.Identity.Name;

            // Productivity for each Produktionsanlage
            /*
            ViewBag.ProduktivitätBerechnungProBandLastMonth = d.ProduktivitätBerechnungProBandLastMonth;
            ViewBag.ProduktivitätBerechnungProMaschineLastMonth = d.ProduktivitätBerechnungProMaschineLastMonth;

            // Chart for productivity in each month 
            ViewBag.ProduktivitätLast12Months = d.ProduktivitätLast12Months;
            */
            /*
            // Statistics for Stücke
            // TODO: can be simplified with a loop or completely removed as it doesn't have a real purpose
            ViewBag.StückeToday = d.StückeToday;
            ViewBag.StückeMinusOneDay = d.StückeMinusOneDay;
            //ViewBag.StückeMinusTwoDays = d.StückeMinusTwoDays;
            ViewBag.StückeMinusThreeDays = d.StückeMinusThreeDays;
            ViewBag.StückeMinusFourDays = d.StückeMinusFourDays;
            ViewBag.StückeMinusFiveDays = d.StückeMinusFiveDays;
            ViewBag.StückeMinusSixDays = d.StückeMinusSixDays;
            ViewBag.StückeWoche = d.StückeWoche;
            */

            // Boxes counting the number of queries in selected tables 
            /*
            ViewBag.Produktionsanlagen = d.Produktionsanlagen;
            ViewBag.Fertigungsteile = d.Fertigungsteile;
            ViewBag.MitarbeiterAnzahl = d.MitarbeiterAnzahl;
            ViewBag.Zeiterfassungen = d.ZeiterfassungenAnzahl;
            */
            return View(d);
        }
    }
}