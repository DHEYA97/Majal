using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Contract.OurSystem
{
    public record OurSystemsResponse
    (
        int Id,
        string Name,
        string MainImage
    );
}
