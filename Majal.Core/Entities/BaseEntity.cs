using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}