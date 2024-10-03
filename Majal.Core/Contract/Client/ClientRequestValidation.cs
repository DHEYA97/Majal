using FluentValidation;
using Majal.Core.Interfaces.Service;
using Majal.Core.Specification.EntitySpecification;

namespace Majal.Core.Contract.Client
{
    public class ClientRequestValidation : AbstractValidator<ClientRequest>
    {
        private readonly IClientService _clientService;
        public ClientRequestValidation(IClientService clientService)
        {
            _clientService = clientService;

            RuleFor(l => l.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("يجب كتابة الاسم")
            .Must(BeUniqueName)
            .WithMessage("الاسم موجود مسبقًا.");

        }
       private bool BeUniqueName(ClientRequest request, string name)
        {
            var id = request.Id; // تأكد من أن كلاس ClientRequest يحتوي على خاصية Id

            if (string.IsNullOrWhiteSpace(name))
            {
                return true;
            }

            var clients =  _clientService.GetAllClientAsync(new ClientSpecification()).Result;

            return !clients.Value.Any(c => c.Name == name && c.Id != id);
        }
    }
}
