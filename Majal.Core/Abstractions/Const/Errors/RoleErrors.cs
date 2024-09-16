using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Abstractions.Const.Errors
{
    public class RoleErrors
    {
        public static readonly Error RoleNotFound =
            new("Role.NotFound", "No Role was found with the given ID", StatusCodes.Status404NotFound);
        
    }
}
