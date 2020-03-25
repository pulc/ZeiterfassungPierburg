using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;

namespace ZeiterfassungPierburg.Controllers
{
    public class ZugriffsrechteController : Controller
    {
        // GET: Zugriffsrechte
        public ActionResult Index()
        {
            ViewBag.ZugriffsrechteMessage = TempData["Message"];

            var results = SQLServer.Instance.GetItems<Zugriffsrechte>();

            return View(results);
        }

        // GET: Zugriffsrechte/Edit/5
        public ActionResult Edit(int id)
        {
            var results = SQLServer.Instance.GetItems<Zugriffsrechte>("id = " + id.ToString());
            if (results.Count() != 1)
            {
                // todo: implement proper error message to be displayed
                return HttpNotFound("Der Zugriffsrecht wurde nicht gefunden.");
            }
            else
                return View(results.First());
        }

        // GET: Produktionsanlage/Create
        public ActionResult Create()
        {
            Zugriffsrechte m = new Zugriffsrechte();
            return View(m);
        }

        [HttpPost]
        public ActionResult Create(Zugriffsrechte m)
        {
            try
            {
                int countBenutzername = SQLServer.Instance.CountResults("SELECT COUNT(*) FROM Zugriffsrechte where Benutzername = '" + m.Benutzername + "'");
                if(countBenutzername > 0)
                {
                    ViewBag.Message = "Der Benutzer mit diesem Benutzernamen existiert bereits. Wähle einen anderen Benutzernamen aus.";
                    return View(m);
                }
                SQLServer.Instance.InsertItem<Zugriffsrechte>(m);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ZugriffsrechteMessage = "Es ist ein Fehler aufgetreten. Kein Zugriffsrecht wurde hinzugefügt. Grund: " + e;
                return View(m);
            }
        }

        [HttpPost]
        public ActionResult Edit(Zugriffsrechte m)
        {
            try
            {
                SQLServer.Instance.EditItem<Zugriffsrechte>(m);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(m);
            }
        }
        // GET: Zugriffsrechte/Delete/5
        public ActionResult Delete(int id)
        {
            if (Convert.ToInt32(Session["AccessLayer"]) == 1)
            {
                try
                {
                    SQLServer.Instance.RemoveItem<Zugriffsrechte>(id);
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["Message"] = "Der Zugriffsrechte konnte nicht gelöscht werden.";
                    //return Index();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        // For login purposes
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Zugriffsrechte o)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // get password for the submitted username
                    string pw = SQLServer.Instance.GetOneString("Password", "Zugriffsrechte", "Benutzername = '" + o.Benutzername + "'");

                    // if the password from the database equals the submitted pw
                    if (pw == o.Password)
                    {
                        Session["UserName"] = o.Benutzername; //set the username for the session

                        // get the accesss layer based on the username
                        int al = SQLServer.Instance.GetInt("Select Zugriffsebene from Zugriffsrechte where Benutzername = '" + o.Benutzername + "'");
                        Session["AccessLayer"] = al;

                        return RedirectToAction("Index", "Home", null);
                    }
                    else
                    {
                        ViewBag.Login = "Benutzername oder Passwort sind falsch.";
                        return View(o);
                    }
                }
                else
                {
                    ViewBag.Login = "Benutzername oder Passwort sind falsch.";
                    return View(o);
                }
            }
            catch
            {
                ViewBag.Login = "Benutzername oder Passwort sind falsch.";
                return View(o);
            }
        }

        public ActionResult DeleteSession()
        {
            Session["UserName"] = null;
            Session["AccessLayer"] = null;

            return Redirect("~/Home/Index");
        }

        public ActionResult Deny()
        {
            return Redirect("~/Shared/ErrorAccessDenied");
        }
    }

}
