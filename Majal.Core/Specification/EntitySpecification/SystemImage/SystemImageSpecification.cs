using Majal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Specification.EntitySpecification
{
    public class SystemImageSpecification : BaseSpecification<SystemImage>
    {
        public SystemImageSpecification()
        {
            Includes.Add(i => i.Image);
            Includes.Add(i => i.OurSystem);

        }
        public SystemImageSpecification(int? id) : base(x => x.OurSystemId == id)
        {
            Includes.Add(i => i.Image);
            Includes.Add(i => i.OurSystem);
        }
    }
}