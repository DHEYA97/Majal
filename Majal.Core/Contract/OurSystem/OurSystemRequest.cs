using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Contract.OurSystem
{
    public record OurSystemRequest
    (
        int? Id,
        string Name,
        string MainImage,
        string MainContentMedia,
        string Content,
        bool HasDemo,
        string? DemoUrl,
        List<string>? SystemImages,
        List<string>? Features

    );
}
