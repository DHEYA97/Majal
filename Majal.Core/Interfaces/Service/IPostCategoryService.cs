using Majal.Core.Abstractions;
using Majal.Core.Contract.PostCategory;
using Majal.Core.Contract.PostCategory;
using Majal.Core.Entities;
using Majal.Core.Specification.EntitySpecification;

namespace Majal.Core.Interfaces.Service
{
    public interface IPostCategoryService
    {
        Task<Result<IEnumerable<PostCategoryResponse>>> GetAllPostCategoryAsync(PostCategorySpecification sp);
        Task<Result<PostCategoryResponse>> GetPostCategoryAsync(PostCategorySpecification sp);
        Task<Result> UpdateAsync(PostCategoryRequest request, PostCategorySpecification sp);
        Task<Result> AddAsync(PostCategoryRequest request, PostCategorySpecification sp);
        Task<Result> DeleteAsync(PostCategorySpecification sp);
    }
}
