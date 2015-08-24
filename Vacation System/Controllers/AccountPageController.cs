using System.Web.Mvc;
using Vacation_System.ServiceReference;

namespace Vacation_System.Controllers
{
    public class AccountPageController : Controller
    {
        public ActionResult Dashboard()
        {
            if (Session["User"] == null) return LogOut();

            return View(Session["User"] as Empleado);
        }

        public ActionResult Profile()
        {
            if (Session["User"] == null) return LogOut();

            return View(Session["User"] as Empleado);
        }

        public ActionResult Departments()
        {
            if (Session["User"] == null) return LogOut();

            return View(Session["User"] as Empleado);
        }
        public RedirectToRouteResult LogOut()
        {
            Session["User"] = null;

            return RedirectToAction("Index", "LogIn");
        }

        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public string Register(Empleado emp)
        {
            ServiceClient service = new ServiceClient();

            service.CreateUser(emp);

            service.Close();

            return "Se ha creado el usuario con exito";
        }
    }
}