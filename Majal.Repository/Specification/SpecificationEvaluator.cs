﻿using Majal.Core.Entities;
using Majal.Core.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Majal.Repository.Specification
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> Query, ISpecification<T> specification)
        {
            if (specification.Criteria is not null)
            {
                Query = Query.Where(specification.Criteria);
            }
            if (specification.OrderByAsc is not null)
            {
                Query = Query.OrderBy(specification.OrderByAsc);
            }
            if (specification.OrderByDes is not null)
            {
                Query = Query.OrderByDescending(specification.OrderByDes);
            }
            if (specification.IsEnablePageSize)
            {
                Query = Query.Skip(specification?.Skip ?? 0).Take(specification?.Take ?? 10);
            }

            if (specification.Includes is not null && specification.Includes.Any())
            {
                Query = specification.Includes.Aggregate(Query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            }

            if (specification.ThenIncludes is not null && specification.ThenIncludes.Any())
            {
                foreach (var thenInclude in specification.ThenIncludes)
                {
                    Query = thenInclude(Query);
                }
            }

            return Query;
        }
    }
}