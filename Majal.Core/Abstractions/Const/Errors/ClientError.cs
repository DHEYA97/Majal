using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Abstractions.Const.Errors
{
    public static class ClientError
    {
        public static readonly Error ClientNotFound =
           new ("العميل غير موجود", "لا يوجد عميل", StatusCodes.Status404NotFound);
    }
}
