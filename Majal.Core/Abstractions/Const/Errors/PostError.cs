using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Abstractions.Const.Errors
{
    public static class PostError
    {
        public static readonly Error PostNotFound =
           new ("المقال غير موجود", "لا يوجد مقال", StatusCodes.Status404NotFound);
    }
}
