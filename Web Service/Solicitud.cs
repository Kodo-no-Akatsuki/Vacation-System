using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Service.Mirror_Classes;

namespace Web_Service
{
    public class Solicitud
    {
        public VacacionesMirror VacData { get; set; }

        public UserMirror EmployeeData { get; set; }
    }
}
