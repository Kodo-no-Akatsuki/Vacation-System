using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using Web_Service.Mirror_Classes;

namespace Web_Service
{
    [DataContract]
    public class Empleado
    {
        [DataMember]
        public UserMirror User { get; set; }

        [DataMember]
        public List<DepartamentoMirror> Departamento { get; set; }

        [DataMember]
        public List<JerarquiaMirror> Jerarquias { get; set; }

        [DataMember]
        public List<LogVacacionesMirror> LogVacaciones { get; set; }

        [DataMember]
        public List<PermisosMirror> Permisos { get; set; }

        [DataMember]
        public List<RolesMirror> Roles { get; set; }

        [DataMember]
        public List<StatusMirror> Status { get; set; }

        [DataMember]
        public List<TipoDiaMirror> TipoDia { get; set; }

        [DataMember]
        public List<VacacionesMirror> Vacaciones { get; set; }

        [DataMember]
        public List<CalendarMirror> Calendar { get; set; }

        //-------------------------Datos relacionados a las vacaciones del empleado (No pueden ir en VacacionesMirror)------------------------

        [DataMember]
        public int DiasTomadosAnteriormente { get; set; }

        [DataMember]
        public int DiasPendientesPorTomar { get; set; }

        [DataMember]
        public List<DateTime> FechasNoDisponibles { get; set; }

        [DataMember]
        public int YearC { get; set; }


        public Empleado()
        {
            Departamento = new List<DepartamentoMirror>();
            Jerarquias = new List<JerarquiaMirror>();
            LogVacaciones = new List<LogVacacionesMirror>();
            Permisos = new List<PermisosMirror>();
            Roles = new List<RolesMirror>();
            Status = new List<StatusMirror>();
            TipoDia = new List<TipoDiaMirror>();
            Vacaciones = new List<VacacionesMirror>();
            Calendar = new List<CalendarMirror>();

            YearC = 0;
            FechasNoDisponibles = null;
            DiasTomadosAnteriormente = 0;
            DiasPendientesPorTomar = 0;

        }
    }
}
