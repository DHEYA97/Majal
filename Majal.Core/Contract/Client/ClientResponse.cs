using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Contract.Client
{
    public record ClientResponse
    (
        int Id,
        string Name,
        string? Url
    );
}
