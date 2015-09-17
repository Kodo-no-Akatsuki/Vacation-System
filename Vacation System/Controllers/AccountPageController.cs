using System.Web.Mvc;
using Vacation_System.ServiceReference;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

			var emp = Session["User"] as Empleado;
            var dvm = new DepartmentsViewModel();

		    var valido = false;

			foreach (var permiso in emp.Permisos)
			{
				switch (permiso.PermisosId)
				{
				    case (int) PermisosEnum.EditarDepartamento:
				        valido = true;
				        break;
				    case (int) PermisosEnum.CrearDepartamento:
				        dvm.DisplayCreate = "button";
				        break;
				    case (int) PermisosEnum.DesactivarDepartamento:
				        dvm.AllowDeactivate = true;
				        break;
				}
			}

			if (!valido)
			{
				return RedirectToAction("error404", "Error");
			}

			ServiceClient service = new ServiceClient();

		    dvm.Departamentos = service.LoadDepartments().ToList();

            service.Close();

			return View(dvm);
		}

		[HttpPost]
		public RedirectToRouteResult CreateDepartment(DepartamentoMirror departamentoMirror)
		{
			if(departamentoMirror.Descripcion == null)
				return RedirectToAction("Departments");

			ServiceClient service = new ServiceClient();

			service.CreateDepartment(departamentoMirror);

			service.Close();

			return RedirectToAction("Departments");
		}

	    [HttpPost]
	    public RedirectToRouteResult EditDepartment(DepartamentoMirror deptoEditado)
	    {
	        if (deptoEditado == null) return RedirectToAction("Departments");

            ServiceClient service = new ServiceClient();

	        if (Request.Form["DesactivarDepto"] != null)
	        {
	            deptoEditado.Activo = Request.Form["DesactivarDepto"] != "Si";
	        }

            service.EditDepartment(deptoEditado);

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
		public RedirectToRouteResult CreateRole(RolesMirror rolCreado)
		{
            if (rolCreado == null) return RedirectToAction("Error500", "Error");

            ServiceClient service = new ServiceClient();

            service.CreateRol(rolCreado);

            service.Close();

            return RedirectToAction("Roles");
		}

	    [HttpPost]
	    public RedirectToRouteResult EditRole(RolesMirror rolEditado)
	    {
	        if (rolEditado == null) return RedirectToAction("Error500", "Error");
            
	        ServiceClient service = new ServiceClient();

            service.SaveRoleChanges(rolEditado);

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