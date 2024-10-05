using Majal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Specification.EntitySpecification
{
    public class OurSystemSpecification : BaseSpecification<OurSystem>
    {
        public OurSystemSpecification()
        {
            Includes.Add(i => i.Image);
            Includes.Add(i => i.Features);

            AddOrderByDes(c => c.CreatedOn);
        }
        public OurSystemSpecification(int? id) : base(x => x.Id == id)
        {
            Includes.Add(i => i.Features);
            Includes.Add(i => i.Image);

            ThenIncludes.Add(query => query
                        .Include(e => e.SystemImages)
                        .ThenInclude(ed => ed.Image));
        }
    }
}