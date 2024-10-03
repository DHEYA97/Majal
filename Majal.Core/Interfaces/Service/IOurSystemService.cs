using Majal.Core.Abstractions;
using Majal.Core.Contract.Client;
using Majal.Core.Contract.OurSystem;
using Majal.Core.Entities;
using Majal.Core.Specification.EntitySpecification;

namespace Majal.Core.Interfaces.Service
{
    public interface IOurSystemService
    {
        Task<Result<IEnumerable<OurSystemsResponse>>> GetAllSystemAsync(OurSystemSpecification sp);
        Task<Result<OurSystemResponse>> GetSystemByIdAsync(OurSystemSpecification sp);
    }
}
