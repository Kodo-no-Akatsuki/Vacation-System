using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Globalization;
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


                foreach (Roles rol in user.tbl_roles)
                {
                    emp.Roles.Add(new RolesMirror
                    {
                        Activo = rol.activo,
                        Descripcion = rol.descripcion,
                        Id = rol.rolesid
                    });

                    foreach(Permisos permiso in rol.tbl_permisos)
                    {
                        
                        bool skip = false;

                        foreach(PermisosMirror perm in emp.Permisos)
                        {
                            if (permiso.descripcion.Equals(perm.Descripcion))
                            {
                                skip = true;
                                break;
                            }
                        }
                            
                        if(!skip)
                        {
                            emp.Permisos.Add(new PermisosMirror
                            {
                                Activo = permiso.activo,
                                Descripcion = permiso.descripcion,
                                PermisosId = permiso.permisosid
                            });
                        }
                        
                    }
                }

                foreach (Departamento dep in user.tbl_departamento)
                {
                    emp.Departamento.Add(new DepartamentoMirror
                    {
                        Activo = dep.activo,
                        DepartamentoId = dep.departamentoid,
                        Descripcion = dep.descripcion
                    });
                }

                foreach (Vacaciones vac in user.tbl_vacaciones)
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

                    emp.Status.Add(new StatusMirror
                    {
                        Activo = vac.tbl_estatus.activo,
                        Descripcion = vac.tbl_estatus.descripcion,
                        Id = vac.tbl_estatus.estatusid
                    });

                }

                foreach (Calendario calendar in user.tbl_calendario)
                {
                    emp.Calendar.Add(new CalendarMirror
                    {
                        fecha = calendar.fecha,
                        TalentoHumanoEmpleado = calendar.talento_humano_empleado,
                        TalentoHumanoJefe = calendar.talento_humano_jefe,
                        TipoDiaId = calendar.tipo_dia_id
                    });


                    emp.TipoDia.Add(new TipoDiaMirror
                    {
                       Descripcion = calendar.tbl_tipo_dia.descripcion,
                       TipoDiaId = calendar.tbl_tipo_dia.tipo_dia_id
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
            CultureInfo provider = new CultureInfo("en-US");
            string format = "g";
            string datestring = userMirror.FechaIngreso.ToString("MM-dd-yyyy");
            user.talento_humano = userMirror.TalentoHumano;
            user.primer_nombre = userMirror.PrimerNombre;
            user.segundo_nombre = userMirror.SegundoNombre;
            user.primer_apellido = userMirror.PrimerApellido;
            user.segundo_apellido = userMirror.SegundoApellido;
            user.email = userMirror.Email;
            user.fecha_ingreso = DateTime.Parse(datestring);
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

            foreach(PermisosMirror permiso in mirror.Permisos)
            {
                Permisos perm = new Permisos
                {
                    activo = permiso.Activo,
                    descripcion = permiso.Descripcion,
                    permisosid = permiso.PermisosId
                };

                rol.tbl_permisos.Add(perm);
            }

            entities.Roles.Add(rol);
            entities.SaveChanges();
        }

        public List<DepartamentoMirror> LoadDepartments()
        {
            VacationEntities entities = new VacationEntities();
            List<DepartamentoMirror> querydepartamentos = new List<DepartamentoMirror>();

            var query = (from d in entities.Departamentoes
                group d by d.descripcion
                into uniquedeptos
                select uniquedeptos.FirstOrDefault()).ToList();

            if (!query.Any())
                return null;

            for (int i = 0; i < query.Count; i++)
            {
                querydepartamentos.Add(new DepartamentoMirror
                {
                    Activo = query[i].activo,
                    DepartamentoId = query[i].departamentoid,
                    Descripcion = query[i].descripcion
                });
            }

            return querydepartamentos;
        }

        public List<RolesMirror> LoadRoles()
        {
            VacationEntities entities = new VacationEntities();
            List<RolesMirror> roles = new List<RolesMirror>();

            var query = (from r in entities.Roles
                group r by r.descripcion
                into uniqueRoles
                select uniqueRoles.FirstOrDefault()).ToList();

            if (!query.Any())
                return null;

            for (int i = 0; i < query.Count; i++)
            {
                List<PermisosMirror> permisos = new List<PermisosMirror>();

                foreach(Permisos p in query[i].tbl_permisos)
                {
                      permisos.Add(new PermisosMirror
                      {
                          Activo = p.activo,
                          Descripcion = p.descripcion,
                          PermisosId = p.permisosid
                      });
                }

                roles.Add(new RolesMirror
                {
                    Activo = query[i].activo,
                    Descripcion = query[i].descripcion,
                    Id = query[i].rolesid,
                    Permisos = permisos
                });
            }

            return roles;
        }

        public List<string> LoadPermisosData()
        {
            VacationEntities entities = new VacationEntities();
            List<string> permisos = new List<string>();

            var query = (from p in entities.Permisos
                         group p by p.descripcion
                             into uniquePermisos
                             select uniquePermisos.FirstOrDefault()).ToList();

            if (!query.Any())
                return null;

            foreach(Permisos permiso in query)
            {
                permisos.Add(permiso.descripcion);
            }

            return permisos;
        }
    }
}
