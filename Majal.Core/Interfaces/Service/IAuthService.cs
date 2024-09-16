using Majal.Core.Abstractions;
using Majal.Core.Contract.Auth;

namespace Majal.Core.Interfaces.Service
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<Result> ResendConfirmEmailAsync(ReSendConfirmEmailRequest request, CancellationToken cancellationToken = default);
        Task<Result> ForgetPasswordAsync(ReSendConfirmEmailRequest request, CancellationToken cancellationToken = default);
        Task<Result> ConfirmResetPasswordAsync(ResetPasswordRequest request);
    }
}
