using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Contract.OurSystem
{
    public class OurSystemRequestValidation : AbstractValidator<OurSystemRequest>
    {
        public OurSystemRequestValidation()
        {
            RuleFor(x => x.Name)
                   .NotEmpty()
                   .NotNull()
                   .WithMessage("يجب ادخال الاسم");

            RuleFor(x => x.MainImage)
                   .NotEmpty()
                   .NotNull()
                   .WithMessage("يجب ادخال الصورة الرئيسية");

            RuleFor(x => x.MainContentMedia)
                   .NotEmpty()
                   .NotNull()
                   .WithMessage("يجب ادخال الصورة الرئيسية");

            RuleFor(x => x.Content)
                   .NotEmpty()
                   .NotNull()
                   .WithMessage("يجب ادخال الصورة الرئيسية");

            RuleFor(q => q.SystemImages)
               .Must(q => q.Distinct().Count() == q.Count())
               .WithMessage(p => "هناك تكرار في الصور")
               .When(q => q.SystemImages != null);

            RuleFor(q => q.Features)
               .Must(q => q.Distinct().Count() == q.Count())
               .WithMessage(p => "هناك تكرار في المزايا")
               .When(q => q.Features != null);
        }
    }
}
