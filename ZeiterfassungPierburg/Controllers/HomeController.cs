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
        //[Authorize(Users = Startup.Administrators)]
        public ActionResult Index()
        {
            Dashboard d = new Dashboard();

            ViewBag.Name = System.Web.HttpContext.Current.User.Identity.Name;

            //ViewBag.Produktivität = d.Produktivität;
            ViewBag.ProduktivitätLast12Months = d.ProduktivitätLast12Months;
            ViewBag.ProduktivitätMaschinen = d.ProduktivitätMaschinen;

            ViewBag.MitarbeiterAnzahl = d.MitarbeiterAnzahl;

            ViewBag.StückeToday = d.StückeToday;
            ViewBag.StückeMinusOneDay = d.StückeMinusOneDay;
            ViewBag.StückeMinusTwoDays = d.StückeMinusTwoDays;
            ViewBag.StückeMinusThreeDays = d.StückeMinusThreeDays;
            ViewBag.StückeMinusFourDays = d.StückeMinusFourDays;
            ViewBag.StückeMinusFiveDays = d.StückeMinusFiveDays;
            ViewBag.StückeMinusSixDays = d.StückeMinusSixDays;
            ViewBag.Produktionsanlagen = d.Produktionsanlagen;
            ViewBag.Fertigungsteile = d.Fertigungsteile;
            ViewBag.StückeWoche = d.StückeWoche;
            ViewBag.Zeiterfassungen = d.ZeiterfassungenAnzahl;
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


        // FOR LOGIN PURPOSES
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserProfile objUser)
        {
            if (ModelState.IsValid)
            {
                using (Authentication db = new Authentication())
                {
                    var obj = db.UserProfiles.Where(a => a.UserName.Equals(objUser.UserName) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.UserId.ToString();
                        Session["UserName"] = obj.UserName.ToString();
                        Session["AccessLayer"] = obj.AccessLayer.ToString();

                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.Login = "Benutzername oder Passwort sind falsch.";
            return View(objUser);
        }

        public ActionResult DeleteSession()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["AccessLayer"] = null;

            return Redirect("~/Home/Index");

        }
    }
}