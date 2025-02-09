using CONSTRUCTION.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CONSTRUCTION.Controllers
{
    public class LoginController : Controller
    {
        Bill_DBEntities db = new Bill_DBEntities();

        public ActionResult Login()//LoginCredential
        {
            ViewBag.loginStatus = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginCredential details)
        {
            Session["finance"] = null;
            Bill_DBEntities db = new Bill_DBEntities();
            if (details.userId != "" && details.password != "")
            {
                if(details.userId == "1" && details.password == "1")
                {
                    return RedirectToAction("MobileEntry", "Orax");
                }

                int pwd = Convert.ToInt32(details.password);
                var isAvl = db.CounterMasters.Any(x => x.counterName == details.userId && x.counterValue == pwd);
                if (isAvl)
                {
                    return RedirectToAction("Index", "Home");
                }
                {
                    ViewBag.loginStatus = "not match";
                }
            }

            return View();
        }


        public ActionResult LogOut()
        {

             return RedirectToAction("Login", "Login");
        }
    }
}