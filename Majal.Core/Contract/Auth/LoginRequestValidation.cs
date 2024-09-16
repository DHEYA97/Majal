using FluentValidation;
using Majal.Core.Contract.Auth;

namespace Majal.Core.Contract.Auth
{
    public class LoginRequestValidation : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidation() 
        {
            RuleFor(l => l.Email)
                   .NotEmpty()
                   .EmailAddress();
        }
    }
}
