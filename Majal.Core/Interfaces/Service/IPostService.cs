using Majal.Core.Abstractions;
using Majal.Core.Contract.Post;
using Majal.Core.Entities;
using Majal.Core.Specification.EntitySpecification;

namespace Majal.Core.Interfaces.Service
{
    public interface IPostService
    {
        Task<Result<IEnumerable<PostResponse>>> GetAllPostAsync(PostSpecification sp);
        Task<Result<PostResponse>> GetPostAsync(PostSpecification sp);
        Task<Result> UpdateAsync(PostRequest request, PostSpecification sp);
        Task<Result> AddAsync(PostRequest request, PostSpecification sp);
        Task<Result> DeleteAsync(PostSpecification sp);
    }
}
