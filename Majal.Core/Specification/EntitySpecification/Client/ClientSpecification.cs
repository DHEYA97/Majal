using Majal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Specification.EntitySpecification
{
    public class ClientSpecification : BaseSpecification<Client>
    {
        public ClientSpecification()
        {
            Includes.Add(i => i.Image);
        }

        public ClientSpecification(int? id) : base(x=>x.Id == id)
        {
            Includes.Add(i => i.Image);
        }
    }
}