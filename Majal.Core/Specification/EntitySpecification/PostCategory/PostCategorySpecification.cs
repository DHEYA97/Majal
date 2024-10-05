using Majal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Specification.EntitySpecification
{
    public class PostCategorySpecification : BaseSpecification<PostCategory>
    {
        public PostCategorySpecification()
        {
            Includes.Add(i => i.posts);
        }

        public PostCategorySpecification(int? id) : base(x=>x.Id == id)
        {
            Includes.Add(i => i.posts);
        }

        public PostCategorySpecification(string? Name) : base(x => x.Name == Name)
        {
            Includes.Add(i => i.posts);
        }
    }
}