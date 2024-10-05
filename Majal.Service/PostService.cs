using Azure;
using MailKit;
using Majal.Core.Abstractions;
using Majal.Core.Abstractions.Const.Errors;
using Majal.Core.Contract.Post;
using Majal.Core.Entities;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;
using Majal.Core.UnitOfWork;
using Mapster;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System.Security.Policy;

namespace Majal.Service
{
    public class PostService(IUnitOfWork unitOfWork,IImageService imageService) : IPostService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IImageService _imageService = imageService;
        public async Task<Result<IEnumerable<PostResponse>>> GetAllPostAsync(PostSpecification sp)
        {
            var Posts = await _unitOfWork.Repositories<Post>().GetAllWithSpecificationAsync(sp);
            var response = Posts.Adapt<IEnumerable<PostResponse>>();
            return Result.Success(response);
        }
        public async Task<Result<PostResponse>> GetPostAsync(PostSpecification sp)
        {
            var Post = await _unitOfWork.Repositories<Post>().GetByIdWithSpecificationAsync(sp);
            if (Post is null)
                return Result.Failure<PostResponse>(PostError.PostNotFound);
            var response = Post.Adapt<PostResponse>();
            return Result.Success(response);
        }
      
      public async Task<Result> UpdateAsync(PostRequest request, PostSpecification sp)
        {
            // بدء المعاملة
            await _unitOfWork.BeginTransactionAsync();
            string url = string.Empty;
            try
            {
                var Post = await _unitOfWork.Repositories<Post>().GetByIdWithSpecificationAsync(sp);
                if (Post is null)
                    return Result.Failure(PostError.PostNotFound);

                var PostCatFound = await _unitOfWork.Repositories<PostCategory>().GetByIdWithSpecificationAsync(new PostCategorySpecification(request.PostCategory));
                if (PostCatFound is null)
                    return Result.Failure(PostCategoryError.PostCategoryNotFound);


                Post.Title = request.Title;
                Post.Body = request.Body;
                Post.PostCategoryId = PostCatFound.Id;

                // حفظ أو تحديث الصورة

                if (!string.IsNullOrWhiteSpace(request.Url))
                {
                    if (!string.IsNullOrWhiteSpace(Post.Image.Url))
                    {
                        url = await _imageService.UpdateImageAsync(Post.Image.Id, request.Url, nameof(Post));
                    }
                    else
                    {
                        url = await _imageService.SaveImageAsync(request.Url, nameof(Post));
                    }
                    Post.Image.Url = url;
                }

                // تحديث الكائن
                _unitOfWork.Repositories<Post>().Update(Post);
                await _unitOfWork.CompleteAsync();

                // تأكيد المعاملة
                await _unitOfWork.CommitTransactionAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                // إلغاء المعاملة في حالة حدوث خطأ
                await _unitOfWork.RollbackTransactionAsync();

                // حذف الصورة في حال حدوث خطأ
                if (!string.IsNullOrWhiteSpace(url))
                {
                    await _imageService.DeleteImageAsync(url);
                }

                return Result.Failure(new Error("حصل خطأ", "حصل خطأ", 404));
            }
        }

        public async Task<Result> AddAsync(PostRequest request, PostSpecification sp)
        {
            // بدء المعاملة
            await _unitOfWork.BeginTransactionAsync();
            string url = string.Empty;
            try
            {
                var isPostFound = await _unitOfWork.Repositories<Post>().GetByIdWithSpecificationAsync(sp);
                if (isPostFound is not null)
                    return Result.Failure(PostError.PostNotFound);

                var PostCatFound = await _unitOfWork.Repositories<PostCategory>().GetByIdWithSpecificationAsync(new PostCategorySpecification(request.PostCategory));
                if (PostCatFound is null)
                    return Result.Failure(PostCategoryError.PostCategoryNotFound);

                var Post = new Post
                {
                    Title = request.Title,
                    Body = request.Body,
                    PostCategoryId = PostCatFound.Id
                };

                // حفظ الصورة
                if (!string.IsNullOrWhiteSpace(request.Url))
                {
                    url = await _imageService.SaveImageAsync(request.Url, nameof(Post));
                    Post.Image = new Image()
                    {
                        Url = url,
                        Type = ImageType.Post
                    };
                }

                // إضافة العميل
                await _unitOfWork.Repositories<Post>().AddAsync(Post);
                await _unitOfWork.CompleteAsync();

                // تأكيد المعاملة
                await _unitOfWork.CommitTransactionAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                // إلغاء المعاملة في حالة حدوث خطأ
                await _unitOfWork.RollbackTransactionAsync();

                // حذف الصورة في حال حدوث خطأ
                if (!string.IsNullOrWhiteSpace(url))
                {
                    await _imageService.DeleteImageAsync(url);
                }

                return Result.Failure(new Error("حصل خطأ", "حصل خطأ", 404));
            }
        }

        public async Task<Result> DeleteAsync(PostSpecification sp)
        {
            var Post = await _unitOfWork.Repositories<Post>().GetByIdWithSpecificationAsync(sp);
            if (Post is null)
                return Result.Failure(PostError.PostNotFound);

            if (!string.IsNullOrWhiteSpace(Post.Image.Url))
            {
                await _imageService.DeleteImageAsync(Post.Image.Url);
            }
            _unitOfWork.Repositories<Post>().Delete(Post);
            await _unitOfWork.CompleteAsync();
            
            return Result.Success();
        }
    }
}
