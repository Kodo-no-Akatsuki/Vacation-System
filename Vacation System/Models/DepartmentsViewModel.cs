using System.Collections.Generic;
using Vacation_System.ServiceReference;

namespace Vacation_System.Models
{
    public class DepartmentsViewModel
    {
       public List<DepartamentoMirror> Departamentos { get; set; }

       public string DisplayCreate { get; set; }

       public bool AllowDeactivate { get; set; }

        public DepartmentsViewModel()
        {
            Departamentos = new List<DepartamentoMirror>();
            DisplayCreate = "hidden";
            AllowDeactivate = false;
        }
    }
}