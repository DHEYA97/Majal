using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Contract.Post
{
    public record PostResponse
    (
        int Id,
        string Title,
        string Body,
        DateTime CreatedOn,
        
        string PostCategory,
        string Url,
        string  CreatedBy
    );
}
