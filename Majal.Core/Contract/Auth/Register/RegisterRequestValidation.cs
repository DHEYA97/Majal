using FluentValidation;
using Majal.Core.Abstractions.Const;

namespace Majal.Core.Contract.Auth
{
    public class RegisterRequestValidation : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidation()
        {
            RuleFor(l => l.Email)
                   .NotEmpty()
                   .EmailAddress();

            RuleFor(l => l.Password)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

            RuleFor(l => l.FirstName)
                .NotEmpty();

        }
    }

}
