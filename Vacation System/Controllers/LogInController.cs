using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vacation_System.Models;
using Vacation_System.ServiceReference;

namespace Vacation_System.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                return RedirectToAction("Dashboard", "AccountPage");
            }

            return View("LogIn");
        }

        [HttpPost]
        public RedirectToRouteResult Index(LoginModel log)
        {
            ServiceClient service = new ServiceClient();

            Empleado emp = service.LogIn(log.Email, log.Password);

            if (emp != null)
            {
                Session["User"] = emp;
                return RedirectToAction("Dashboard", "AccountPage");
            }

            Session["User"] = null;

            service.Close();
            return RedirectToAction("Index");
        }
    }
}