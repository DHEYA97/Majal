using FluentValidation;

namespace Majal.Core.Contract
{
    public class EmployeeRequestValidation : AbstractValidator<EmployeeRequest>
    {
        public EmployeeRequestValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
