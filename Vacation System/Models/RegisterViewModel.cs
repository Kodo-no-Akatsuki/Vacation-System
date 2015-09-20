using System.Collections.Generic;
using Vacation_System.ServiceReference;

namespace Vacation_System.Models
{
    public class RegisterViewModel
    {
        public Empleado Empleado { get; set; }

        public List<DepartamentoMirror> Departamentos { get; set; }

        public List<RolesMirror> Roles { get; set; } 
    }
}