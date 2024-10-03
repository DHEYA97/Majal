using Majal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Specification.EntitySpecification
{
    public class ImageSpecification : BaseSpecification<Image>
    {
        public ImageSpecification()
        {
            
        }

        public ImageSpecification(int id) : base(x=>x.Id == id)
        {
        }
    }
}