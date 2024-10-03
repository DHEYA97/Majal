using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Abstractions.Const.Errors
{
    public static class OurSystemError
    {
        public static readonly Error OurSystemNotFound =
           new ("النظام غير موجود", "لا يوجد نظام بهذه البيانات", StatusCodes.Status404NotFound);
    }
}
