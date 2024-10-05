using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Abstractions.Const.Errors
{
    public static class PostCategoryError
    {
        public static readonly Error PostCategoryNotFound =
           new ("القسم غير موجود", "لا يوجد قسم", StatusCodes.Status404NotFound);
    }
}
