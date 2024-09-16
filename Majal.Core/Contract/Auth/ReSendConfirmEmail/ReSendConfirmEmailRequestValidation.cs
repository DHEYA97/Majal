using FluentValidation;

namespace Majal.Core.Contract.Auth
{
    public class ReSendConfirmEmailRequestValidation : AbstractValidator<ReSendConfirmEmailRequest>
    {
        public ReSendConfirmEmailRequestValidation()
        {
            
            RuleFor(l => l.Email)
                .NotEmpty();
        }
    }

}
