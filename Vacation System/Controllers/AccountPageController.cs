using System;
using System.Web.Mvc;
using Vacation_System.ServiceReference;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Vacation_System.Models;
using Web_Service;

namespace Vacation_System.Controllers
{
	public class AccountPageController : Controller
	{
		public ActionResult Dashboard()
		{
			Empleado user = Session["User"] as Empleado;
			List<PendingVacations> pendientes = new List<PendingVacations>();

			if (user == null) return LogOut();

			user.YearC = (int)((DateTime.Now - user.User.FechaIngreso).TotalDays) / 365;
			user.DiasTomadosAnteriormente = user.Vacaciones.Where(vacacion => vacacion.Year == DateTime.Now.Year && vacacion.EstatusId == 1).Sum(vacacion => vacacion.DiasSolicitados);
			user.DiasPendientesPorTomar = 23 - user.DiasTomadosAnteriormente;
			user.FechasNoDisponibles = user.Calendar.Select(x => x.fecha).ToArray();

			foreach (var year in user.Vacaciones)
			{
				if (year.DiasSolicitados < 23)
				{
					pendientes.Add(new PendingVacations
					{
						PendingDays = 23 - year.DiasSolicitados,
						PendingYear = year.Year
					});
				}
			}

		    ViewBag.VacacionesPendientes = pendientes;
		    ViewBag.NotificationScript = "new PNotify({title: 'Enhorabuena', text: 'La solicitud ha sido enviada!', type: 'success'});";

			return View(user);
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