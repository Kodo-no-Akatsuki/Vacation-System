﻿using System;
using System.Collections.Generic;
using Vacation_System.ServiceReference;
using System.Linq;
using System.Web;

namespace Vacation_System.Models
{
    public class CreateRoleModel
    {
        public IEnumerable<RolesMirror> Roles { get; set; }

        public RolesMirror RolActual { get; set; }

    }
}