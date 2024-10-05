using Majal.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace Majal.Core.Specification
{
    public interface ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> ThenIncludes { get; }
        public Expression<Func<T, object>> OrderByAsc { get; set; }
        public Expression<Func<T, object>> OrderByDes { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool IsEnablePageSize { get; set; }

    }
}
