using System;
using System.Web.Mvc;
using Vacation_System.ServiceReference;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Vacation_System.Models;
using Web_Service;
using Vacation_System.Models;

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

			ProfileViewModel pvm = new ProfileViewModel
			{
				ProfileEmpleado = Session["User"] as Empleado,
				DisplayEditStatusBtn = false,
				DisplayEditBtn = true
			};

			return View(pvm);
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
		public RedirectToRouteResult EditDepartment(DepartamentoMirror deptoEditado, string estatus)
		{
			if (deptoEditado == null) return RedirectToAction("Departments");

			ServiceClient service = new ServiceClient();

			if (estatus != null)
			{
			    deptoEditado.Activo = !(estatus.Equals("True"));
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

			ServiceClient service = new ServiceClient();

			RegisterViewModel rvm = new RegisterViewModel
			{
				Empleado = emp,
				Departamentos = service.LoadDepartments().ToList(),
				Roles = service.LoadRoles().ToList()
			};

			service.Close();

			return View(rvm);
		}

		[HttpPost]
		public RedirectToRouteResult Register(Empleado emp)
		{
			ServiceClient service = new ServiceClient();
			
			service.CreateUser(emp);

			service.Close();

			return RedirectToAction("Dashboard");
		}

		[HttpGet]
		public ActionResult Users()
		{
			ServiceClient client = new ServiceClient();

			UsersViewModel uvm = new UsersViewModel();

			uvm.Users = client.LoadUsers((Session["User"] as Empleado).User.Email).ToList();

			client.Close();

			return View(uvm);
		}

		public ActionResult UserProfile(int talentoHumano)
		{
			if (Session["User"] == null) return LogOut();

			ServiceClient service = new ServiceClient();

			ProfileViewModel pvm = new ProfileViewModel
			{
				ProfileEmpleado = service.LoadEmpleado(talentoHumano),
			};

			pvm.DisplayEditStatusBtn = (from p in (Session["User"] as Empleado).Permisos
								  where p.PermisosId == (int)PermisosEnum.DesactivarUsuario
								  select p).FirstOrDefault() != null;

			pvm.DisplayEditBtn = (from p in (Session["User"] as Empleado).Permisos
				where p.PermisosId == (int) PermisosEnum.EditarUsuario
				select p).FirstOrDefault() != null;
			
			service.Close();

			return View("Profile", pvm);
		}

		[HttpPost]
		public RedirectToRouteResult DesactivarUsuario(int talentoHumano, string edit)
		{
			ServiceClient service = new ServiceClient();

			service.EditUserStatus(talentoHumano, edit == "Activar");

			service.Close();

			return RedirectToAction("UserProfile", new { talentoHumano });
		}

		public ActionResult EditUser(int talentoHumano)
		{
			ServiceClient service = new ServiceClient();

			EditUserViewModel euvm = new EditUserViewModel
			{
				Empleado = service.LoadEmpleado(talentoHumano),
				Departamentos = service.LoadDepartments().ToList(),
				Roles = service.LoadRoles().ToList()
			};

			euvm.DisplayRolesAndDepartments = (Session["User"] as Empleado).User.TalentoHumano != 
				euvm.Empleado.User.TalentoHumano && 
				(from p in (Session["User"] as Empleado).Permisos
								  where p.PermisosId == (int)PermisosEnum.EditarUsuario
								  select p).FirstOrDefault() != null;

			service.Close();

			return View(euvm);
		}

		public RedirectToRouteResult EditarUser(Empleado empleado, string status)
		{
			ServiceClient service = new ServiceClient();

			empleado.User.Activo = status == "Activo";

			service.EditUser(empleado);

			service.Close();

			return RedirectToAction("Dashboard");
		}
	}
}