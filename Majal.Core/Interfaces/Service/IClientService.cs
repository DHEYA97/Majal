using Majal.Core.Abstractions;
using Majal.Core.Contract.Client;
using Majal.Core.Entities;
using Majal.Core.Specification.EntitySpecification;

namespace Majal.Core.Interfaces.Service
{
    public interface IClientService
    {
        Task<Result<IEnumerable<ClientResponse>>> GetAllClientAsync(ClientSpecification sp);
        Task<Result<ClientResponse>> GetClientAsync(ClientSpecification sp);
        Task<Result> UpdateAsync(ClientRequest request, ClientSpecification sp);
        Task<Result> AddAsync(ClientRequest request, ClientSpecification sp);
        Task<Result> DeleteAsync(ClientSpecification sp);
    }
}
