
using Vacation_System.ServiceReference;

namespace Vacation_System.Models
{
    public class ProfileViewModel
    {
        public Empleado ProfileEmpleado { get; set; }

        public bool DisplayEditStatusBtn { get; set; }

        public bool DisplayEditBtn { get; set; }
    }
}