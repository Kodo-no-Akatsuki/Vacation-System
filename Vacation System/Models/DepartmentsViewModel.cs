using System.Collections.Generic;
using Vacation_System.ServiceReference;

namespace Vacation_System.Models
{
    public class DepartmentsViewModel
    {
       public List<DepartamentoMirror> Departamentos { get; set; }

       public bool DisplayCreate { get; set; }

        public DepartmentsViewModel()
        {
            Departamentos = new List<DepartamentoMirror>();
            DisplayCreate = false;
        }
    }
}