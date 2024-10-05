using FluentValidation;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;

namespace Majal.Core.Contract.Post
{
    public class PostRequestValidation : AbstractValidator<PostRequest>
    {
        private readonly IPostService _postService;
        public PostRequestValidation(IPostService postService)
        {
            _postService = postService;

            RuleFor(l => l.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("يجب كتابة الاسم")
            .Must(BeUniqueName)
            .WithMessage("الاسم موجود مسبقًا.");


            RuleFor(l => l.Body)
                .NotNull()
                .NotEmpty()
                .WithMessage("يجب كتابة محتى");

            RuleFor(l => l.Body)
                .NotNull()
                .NotEmpty()
                .WithMessage("يجب كتابة محتى");
        }
       private bool BeUniqueName(PostRequest request, string title)
        {
            var id = request.Id; // تأكد من أن كلاس ClientRequest يحتوي على خاصية Id

            if (string.IsNullOrWhiteSpace(title))
            {
                return true;
            }

            var clients =  _postService.GetAllPostAsync(new PostSpecification()).Result;

            return !clients.Value.Any(c => c.Title == title && c.Id != id);
        }
    }
}
