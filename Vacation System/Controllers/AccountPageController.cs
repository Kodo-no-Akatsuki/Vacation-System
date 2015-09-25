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
		[HttpGet]
		public ActionResult Dashboard()
		{
			ServiceClient service = new ServiceClient();
			Empleado user = Session["User"] as Empleado;
			List<PendingVacations> pendientes = new List<PendingVacations>();

			if (user == null) return LogOut();

			Session["User"] = service.LoadVacaciones(Session["User"] as Empleado);
			user = Session["User"] as Empleado;
			user.YearC = (int)((DateTime.Now - user.User.FechaIngreso).TotalDays) / 365;
			user.DiasTomadosAnteriormente = user.Vacaciones.Where(vacacion => vacacion.Year == DateTime.Now.Year && vacacion.Estatus.Descripcion.Equals("Aprobado")).Sum(vacacion => vacacion.DiasSolicitados);

			user.DiasPendientesPorTomar = 23 - user.DiasTomadosAnteriormente;
			user.FechasNoDisponibles = user.Calendar.Select(x => x.fecha).ToArray();

			user.Vacaciones = user.Vacaciones.OrderBy(x => x.Year).ToArray();

			for (int i = 0; i < user.Vacaciones.Length; i++)
			{
				int available = 23;

				if (user.Vacaciones[i].DiasSolicitados < 23 && user.Vacaciones[i].Estatus.Descripcion.Equals("Aprobado"))
				{
					available -= user.Vacaciones[i].DiasSolicitados;
					while (i != (user.Vacaciones.Length - 1) && user.Vacaciones[i + 1].Year == user.Vacaciones[i].Year && user.Vacaciones[i+1].Estatus.Descripcion.Equals("Aprobado"))
					{
						available -= user.Vacaciones[i + 1].DiasSolicitados;
						i++;
					}

					pendientes.Add(new PendingVacations
					{
						PendingDays = available,
						PendingYear = user.Vacaciones[i].Year
					});
				}
			}

			ViewBag.VacacionesPendientes = pendientes;
			ViewBag.NotificationScript = "new PNotify({title: 'Enhorabuena', text: 'La solicitud ha sido enviada!', type: 'success'});";
			service.Close();

			return View(user);
		}

		[HttpPost]
		public RedirectToRouteResult Dashboard(VacacionesMirror vacaciones)
		{
			ServiceClient service = new ServiceClient();
			Empleado emp = (Session["User"] as Empleado);

			vacaciones.TalentoHumano = (Session["User"] as Empleado).User.TalentoHumano;
			vacaciones.FechaSolicitud = DateTime.Now;
			vacaciones.Year = DateTime.Today.Year;
			emp.Notification = true;

			service.AddVacation(vacaciones);
			service.Close();

			return RedirectToAction("Dashboard");
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