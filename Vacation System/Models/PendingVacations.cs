using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vacation_System.Models
{
    public class PendingVacations
    {
        public int PendingDays { get; set; }
        public int PendingYear { get; set; }

        public PendingVacations()
        {
            PendingDays = 0;
            PendingYear = 0;
        }
    }
}