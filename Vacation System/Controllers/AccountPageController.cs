using System.Web.Mvc;
using Vacation_System.ServiceReference;
using System.Collections.Generic;
using System.Diagnostics;

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

		[HttpGet]
		public ActionResult Departments()
		{
			if (Session["User"] == null) return LogOut();

			ServiceClient service = new ServiceClient();

			//ViewBag.Empleado = (Session["User"] as Empleado);
		    ViewData["Empleado"] = (Session["User"] as Empleado);

			return View(service.LoadDepartments());
		}

		[HttpPost]
		public RedirectToRouteResult Departments(DepartamentoMirror departamentoMirror)
		{
			if(departamentoMirror.Descripcion == null)
				return RedirectToAction("Departments");

			ServiceClient service = new ServiceClient();
			departamentoMirror.Activo = true;

			service.CreateDepartment(departamentoMirror);

			service.Close();

			return RedirectToAction("Departments");
		}

		
		[HttpGet]
		public ActionResult Roles()
		{
			if (Session["User"] == null) return LogOut();

			ServiceClient service = new ServiceClient();

			return View(service.LoadRoles());
		}

		[HttpPost]
		public RedirectToRouteResult Roles(RolesMirror rol)
		{
			if (rol.Descripcion == null)
				return RedirectToAction("Roles");

			ServiceClient service = new ServiceClient();
			rol.Activo = true;

			service.CreateRol(rol);

			service.Close();

			return RedirectToAction("Roles");
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