using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int? ImageId { get; set; }
        public Image Image { get; set; }
        public int PostCategoryId { get; set; }
        public PostCategory PostCategory { get; set; }
    }
}
