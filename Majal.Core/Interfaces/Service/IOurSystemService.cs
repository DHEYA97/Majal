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
        Task<Result> AddAsync(OurSystemRequest request, OurSystemSpecification sp);
        Task<Result> UpdateAsync(OurSystemRequest request, OurSystemSpecification sp);
        Task<Result> DeleteAsync(OurSystemSpecification sp);
    }
}
