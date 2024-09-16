﻿using FluentValidation;
using Majal.Core.Abstractions.Const;

namespace Majal.Core.Contract.Auth
{
    public class ResetPasswordRequestValidation : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidation()
        {
            RuleFor(l => l.Email)
                .NotEmpty()
                .EmailAddress();
            
            RuleFor(l => l.Code)
                .NotEmpty();
            RuleFor(l => l.NewPassword)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");
        }
    }

}
