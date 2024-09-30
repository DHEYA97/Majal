using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Entities
{
    public class Feature : BaseEntity
    {
        public string Content { get; set; }
        public OurSystem OurSystem { get; set; }
    }
}
