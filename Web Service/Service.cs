using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Web_Service.Mirror_Classes;

namespace Web_Service
{
   
    public class Service : IService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public Empleado LogIn(string email, string password)
        {
            VacationEntities entities = new VacationEntities();
            Empleado emp = new Empleado();

            var userResults = (from u in entities.Usuarios
                               where u.email == email && u.password == password && u.activo
                               select u);
            
            if (userResults != null && userResults.Any())
            {
                Usuarios user = userResults.FirstOrDefault();

                emp.User = new UserMirror(user);

                var departamentos = (from j in user.tbl_jerarquia
                    where j.talento_humano == user.talento_humano
                    select j.tbl_departamento);

                var vacaciones = (from v in user.tbl_vacaciones
                    select v);

                var estatuses = from v in user.tbl_vacaciones
                    where v.talento_humano == user.talento_humano
                    select v.tbl_estatus;

                var calendario = from c in user.tbl_calendario
                    select c;

                var tipoDias = from d in user.tbl_calendario
                    select d.tbl_tipo_dia;

                //REVISAR ESTOS QUERIES-------------------------------------------------

                var roles = from r in entities.Roles
                            from u in r.tbl_usuarios
                            where r.rolesid == u.talento_humano
                            select r;

                var permisos = from p in entities.Permisos
                               from r in roles
                               where p.permisosid == r.rolesid
                               select p;

                foreach (Roles rol in roles)
                {
                    emp.Roles.Add(new RolesMirror
                    {
                        Activo = rol.activo,
                        Descripcion = rol.descripcion,
                        Id = rol.rolesid
                    });
                }

                foreach (Permisos permiso in permisos)
                {
                    emp.Permisos.Add(new PermisosMirror
                    {
                        Activo = permiso.activo,
                        Descripcion = permiso.descripcion,
                        PermisosId = permiso.permisosid
                    });
                }

                //------------------------------------------------------------------------

                foreach (Departamento dep in departamentos)
                {
                    emp.Departamento.Add(new DepartamentoMirror
                    {
                        Activo = dep.activo,
                        DepartamentoId = dep.departamentoid,
                        Descripcion = dep.descripcion
                    });
                }

                foreach (Vacaciones vac in vacaciones)
                {
                    emp.Vacaciones.Add(new VacacionesMirror
                    {
                        DiasSolicitados = vac.dias_solicitados,
                        EstatusId = vac.estatusid,
                        FechaAprobacion = vac.fecha_de_aprobacion,
                        FechaEntrada = vac.fecha_entrada,
                        FechaSalida = vac.fecha_salida,
                        FechaSolicitud = vac.fecha_solicitud,
                        TalentoHumano = vac.talento_humano,
                        VacacionesId = vac.vacacionesid,
                        Year = vac.year
                    });
                }

                foreach (Estatus estatus in estatuses)
                {
                    emp.Status.Add(new StatusMirror
                    {
                        Activo = estatus.activo,
                        Descripcion = estatus.descripcion,
                        Id = estatus.estatusid
                    });
                }

                foreach (Calendario calendar in calendario)
                {
                    emp.Calendar.Add(new CalendarMirror
                    {
                        fecha = calendar.fecha,
                        TalentoHumanoEmpleado = calendar.talento_humano_empleado,
                        TalentoHumanoJefe = calendar.talento_humano_jefe,
                        TipoDiaId = calendar.tipo_dia_id
                    });
                }

                foreach (TipoDia dia in tipoDias)
                {
                    emp.TipoDia.Add(new TipoDiaMirror
                    {
                        Descripcion = dia.descripcion,
                        TipoDiaId = dia.tipo_dia_id
                    });
                }

                return emp;
            }

            return null;
        }

        public void CreateUser(Empleado empleado)
        {
            VacationEntities entities = new VacationEntities();

            UserMirror userMirror = empleado.User;
            Usuarios user = new Usuarios();

            user.talento_humano = userMirror.TalentoHumano;
            user.primer_nombre = userMirror.PrimerNombre;
            user.segundo_nombre = userMirror.SegundoNombre;
            user.primer_apellido = userMirror.PrimerApellido;
            user.segundo_apellido = userMirror.SegundoApellido;
            user.email = userMirror.Email;
            user.fecha_ingreso = DateTime.Parse(userMirror.FechaIngreso.ToString());
            user.fecha_creacion = DateTime.Now;
            user.password = userMirror.Password;
            user.activo = true;

            foreach (DepartamentoMirror deptoMirror in empleado.Departamento)
            {
                Departamento depto = new Departamento();

                depto.departamentoid = deptoMirror.DepartamentoId;
                depto.descripcion = deptoMirror.Descripcion;
                depto.activo = true;

                user.tbl_departamento.Add(depto);
            }

            foreach (RolesMirror rolMirror in empleado.Roles)
            {
                Roles rol = new Roles();

                rol.rolesid = rolMirror.Id;
                rol.descripcion = rolMirror.Descripcion;
                rol.activo = true;

                user.tbl_roles.Add(rol);
            }

            entities.Usuarios.Add(user);
            entities.SaveChanges();
        }

        public void CreateDepartment(DepartamentoMirror depto)
        {
            VacationEntities entities = new VacationEntities();
            Departamento departamento = new Departamento
            {
                activo = depto.Activo,
                departamentoid = depto.DepartamentoId,
                descripcion = depto.Descripcion
            };

            entities.Departamentoes.Add(departamento);
            entities.SaveChanges();
        }

        public void CreateRol(RolesMirror mirror)
        {
            VacationEntities entities = new VacationEntities();

            Roles rol = new Roles
            {
                activo = mirror.Activo,
                descripcion = mirror.Descripcion,
                rolesid = mirror.Id
            };

            entities.Roles.Add(rol);
            entities.SaveChanges();
        }
    }
}
