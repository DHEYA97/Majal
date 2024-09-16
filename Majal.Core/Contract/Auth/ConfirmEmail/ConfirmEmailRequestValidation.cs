using FluentValidation;

namespace Majal.Core.Contract.Auth
{
    public class ConfirmEmailRequestValidation : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidation()
        {
            RuleFor(l => l.UserId)
                .NotEmpty();
            
            RuleFor(l => l.Code)
                .NotEmpty();
        }
    }

}
