using Azure;
using MailKit;
using Majal.Core.Abstractions;
using Majal.Core.Abstractions.Const.Errors;
using Majal.Core.Contract;
using Majal.Core.Contract.PostCategory;
using Majal.Core.Contract.PostCategory;
using Majal.Core.Entities;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;
using Majal.Core.UnitOfWork;
using Mapster;


namespace Majal.Service
{
    public class PostCategoryService(IUnitOfWork unitOfWork) : IPostCategoryService 
    { 
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result<IEnumerable<PostCategoryResponse>>> GetAllPostCategoryAsync(PostCategorySpecification sp)
        {
            var postCategorys = await _unitOfWork.Repositories<PostCategory>().GetAllWithSpecificationAsync(sp);
            var response = postCategorys.Adapt<IEnumerable<PostCategoryResponse>>();
            return Result.Success(response);
        }
        public async Task<Result<PostCategoryResponse>> GetPostCategoryAsync(PostCategorySpecification sp)
        {
            var postCategory = await _unitOfWork.Repositories<PostCategory>().GetByIdWithSpecificationAsync(sp);
            if (postCategory is null)
                return Result.Failure<PostCategoryResponse>(PostCategoryError.PostCategoryNotFound);
            var response = postCategory.Adapt<PostCategoryResponse>();
            return Result.Success(response);
        }

        public async Task<Result> UpdateAsync(PostCategoryRequest request, PostCategorySpecification sp)
        {
            // بدء المعاملة
            await _unitOfWork.BeginTransactionAsync();
            string url = string.Empty;
            try
            {
                var postCategory = await _unitOfWork.Repositories<PostCategory>().GetByIdWithSpecificationAsync(sp);
                if (postCategory is null)
                    return Result.Failure(PostCategoryError.PostCategoryNotFound);

                postCategory = request.Adapt(postCategory);

                _unitOfWork.Repositories<PostCategory>().Update(postCategory);
                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure(new Error("حصل خطأ", "حصل خطأ", 404));
            }
        }

        public async Task<Result> AddAsync(PostCategoryRequest request, PostCategorySpecification sp)
        {
            // بدء المعاملة
            await _unitOfWork.BeginTransactionAsync();
            string url = string.Empty;
            try
            {
                var isPostCategoryFound = await _unitOfWork.Repositories<PostCategory>().GetByIdWithSpecificationAsync(sp);
                if (isPostCategoryFound is not null)
                    return Result.Failure(PostCategoryError.PostCategoryNotFound);

                var postCategory = new PostCategory
                {
                    Name = request.Name,
                };

                await _unitOfWork.Repositories<PostCategory>().AddAsync(postCategory);
                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure(new Error("حصل خطأ", "حصل خطأ", 404));
            }
        }

        public async Task<Result> DeleteAsync(PostCategorySpecification sp)
        {
            var postCategory = await _unitOfWork.Repositories<PostCategory>().GetByIdWithSpecificationAsync(sp);
            if (postCategory is null)
                return Result.Failure(PostCategoryError.PostCategoryNotFound);
            
            _unitOfWork.Repositories<PostCategory>().Delete(postCategory);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
    }
}
