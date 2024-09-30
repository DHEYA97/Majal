using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Entities
{
    public class PostCategory : BaseEntity
    {
        public string Name {  get; set; }
        public ICollection<Post> posts { get; set; }
    }
}
