using System.Web.Mvc;
using Vacation_System.ServiceReference;
using System.Collections.Generic;
using System.Diagnostics;
using Web_Service;
using Vacation_System.Models;

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

			Empleado emp = Session["User"] as Empleado;
			bool valido = false;

			foreach (var permiso in emp.Permisos)
			{
				if (permiso.PermisosId == (int) PermisosEnum.EditarDepartamento ||
					permiso.PermisosId == (int) PermisosEnum.CrearDepartamento)
				{
					valido = true;
				}
			}

			if (!valido)
			{
				return RedirectToAction("error404", "Error");
			}

			ServiceClient service = new ServiceClient();

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

			Empleado emp = Session["User"] as Empleado;
			bool valido = false;

			foreach (var permiso in emp.Permisos)
			{
				if (permiso.PermisosId == (int)PermisosEnum.EditarRol ||
					permiso.PermisosId == (int)PermisosEnum.CrearRol)
				{
					valido = true;
				}
			}

			if (!valido)
			{
				return RedirectToAction("error404", "Error");
			}

			return View(new CreateRoleModel());
		}

		[HttpPost]
		public RedirectToRouteResult Roles(CreateRoleModel crm)
		{
            if (crm.NuevoRol.Descripcion == null)
                return RedirectToAction("Roles");

            ServiceClient service = new ServiceClient();

			service.CreateRol(crm.NuevoRol);

			service.Close();

			return RedirectToAction("Roles");
		}
		
		
		public RedirectToRouteResult LogOut()
		{
			Session["User"] = null;

			return RedirectToAction("Index", "LogIn");
		}

		public ActionResult Register()
		{
			Empleado emp = Session["User"] as Empleado;
			bool valido = false;

			foreach (var permiso in emp.Permisos)
			{
				if (permiso.PermisosId == (int)PermisosEnum.CrearUsuario)
				{
					valido = true;
				}
			}

			if (!valido)
			{
				return RedirectToAction("error404", "Error");
			}

			return View(Session["User"] as Empleado);
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