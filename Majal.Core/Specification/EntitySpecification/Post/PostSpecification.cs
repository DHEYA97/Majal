using Majal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Specification.EntitySpecification
{
    public class PostSpecification : BaseSpecification<Post>
    {
        public PostSpecification()
        {
            Includes.Add(i => i.Image);
            Includes.Add(i => i.PostCategory);
            Includes.Add(i => i.CreatedBy);

        }

        public PostSpecification(int? id) : base(x=>x.Id == id)
        {
            Includes.Add(i => i.Image);
            Includes.Add(i => i.PostCategory);
            Includes.Add(i => i.CreatedBy);
        }
    }
}