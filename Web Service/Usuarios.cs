//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuarios
    {
        public Usuarios()
        {
            this.tbl_jerarquia = new HashSet<Jerarquia>();
            this.tbl_jerarquia1 = new HashSet<Jerarquia>();
            this.tbl_log_vacaciones = new HashSet<LogVacaciones>();
            this.tbl_vacaciones = new HashSet<Vacaciones>();
            this.tbl_departamento = new HashSet<Departamento>();
            this.tbl_roles = new HashSet<Roles>();
            this.tbl_calendario = new HashSet<Calendario>();
            this.tbl_calendario1 = new HashSet<Calendario>();
        }
    
        public int talento_humano { get; set; }
        public string email { get; set; }
        public string primer_nombre { get; set; }
        public string segundo_nombre { get; set; }
        public string primer_apellido { get; set; }
        public string segundo_apellido { get; set; }
        public System.DateTime fecha_ingreso { get; set; }
        public System.DateTime fecha_creacion { get; set; }
        public string password { get; set; }
        public bool activo { get; set; }
    
        public virtual ICollection<Jerarquia> tbl_jerarquia { get; set; }
        public virtual ICollection<Jerarquia> tbl_jerarquia1 { get; set; }
        public virtual ICollection<LogVacaciones> tbl_log_vacaciones { get; set; }
        public virtual ICollection<Vacaciones> tbl_vacaciones { get; set; }
        public virtual ICollection<Departamento> tbl_departamento { get; set; }
        public virtual ICollection<Roles> tbl_roles { get; set; }
        public virtual ICollection<Calendario> tbl_calendario { get; set; }
        public virtual ICollection<Calendario> tbl_calendario1 { get; set; }
    }
}
