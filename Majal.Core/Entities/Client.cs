using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Entities
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public int? ImageId { get; set; }
        public Image Image { get; set; }
    }
}
