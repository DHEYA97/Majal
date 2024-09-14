using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Abstractions.Const.Errors
{
    public static class EmployeeError
    {
        public static readonly Error EmployeeNotFound =
           new("Employee.NotFound", "No Employee was found", 404);
    }
}
