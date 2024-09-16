using Majal.Core.Contract.Auth;

namespace Majal.Api.Controllers
{
    public class AuthController(IAuthService authService,ILogger<AuthController> logger) : BaseApiController
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody] LoginRequest Request,CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Logg Info");
            var authResult = await _authService.GetTokenAsync(Request.Email, Request.Password,cancellationToken);
            return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest Request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.RegisterAsync(Request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest Request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.ConfirmEmailAsync(Request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("resend-confirm-email")]
        public async Task<IActionResult> ReSendConfirmEmail([FromBody] ReSendConfirmEmailRequest Request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.ResendConfirmEmailAsync(Request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ReSendConfirmEmailRequest Request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.ForgetPasswordAsync(Request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest Request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.ConfirmResetPasswordAsync(Request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

    }
}
