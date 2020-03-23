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
            return View(d);
        }
    }
}