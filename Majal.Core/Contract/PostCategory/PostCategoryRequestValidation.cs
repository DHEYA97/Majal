using FluentValidation;
using Majal.Core.Entities;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;

namespace Majal.Core.Contract.PostCategory
{
    public class PostCategoryRequestValidation : AbstractValidator<PostCategoryRequest>
    {
        private readonly IPostCategoryService _PostCategory;
        public PostCategoryRequestValidation(IPostCategoryService PostCategory)
        {
            _PostCategory = PostCategory;

            RuleFor(l => l.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("يجب كتابة الاسم")
            .Must(BeUniqueName)
            .WithMessage("الاسم موجود مسبقًا.");

        }
       private bool BeUniqueName(PostCategoryRequest request, string name)
        {
            var id = request.Id; // تأكد من أن كلاس ClientRequest يحتوي على خاصية Id

            if (string.IsNullOrWhiteSpace(name))
            {
                return true;
            }

            var postCategory = _PostCategory.GetAllPostCategoryAsync(new PostCategorySpecification()).Result;

            return !postCategory.Value.Any(c => c.Name == name && c.Id != id);
        }
    }
}
