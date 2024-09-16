using Majal.Core.Abstractions.Const;
using Majal.Core.Abstractions;
using Majal.Core.Contract.Auth;
using Majal.Core.Entities.Identity;
using Majal.Core.Interfaces.Authentication;
using Majal.Core.Interfaces.Service;
using Majal.Repository.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Majal.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Majal.Core.Abstractions.Const.Errors;
using Mapster;
using Hangfire;
namespace Majal.Service
{
    public class AuthService
        (
       UserManager<ApplicationUser> userManager,
       SignInManager<ApplicationUser> signInManager,
       IJwtProvider jwtProvider,
       IEmailSender emailSender,
       IHttpContextAccessor httpContextAccessor,
       ILogger<AuthService> logger, ApplicationDbContext context) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

            var result = await _signInManager.PasswordSignInAsync(user, password, false, true);
            if (result.Succeeded)
            {
                var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellationToken);
                var (token, expiration) = _jwtProvider.GenerateToken(user, userRoles);

                await _userManager.UpdateAsync(user);
                var authResponse = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expiration);
                return Result.Success(authResponse);
            }
            var error = result.IsNotAllowed
            ? UserErrors.EmailNotConfirmed
            : result.IsLockedOut
            ? UserErrors.LockedOut
            : UserErrors.InvalidCredentials;

            return Result.Failure<AuthResponse>(error);

        }

        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var isEmailExist = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
            if (isEmailExist)
                return Result.Failure(UserErrors.DuplicatedEmail);

            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
                Dictionary<string, string> templateModel = new Dictionary<string, string>
                                {
                                    {"{{name}}",user.FirstName},
                                    {"{{action_url}}",$"{origin}/auth/confirm-email?UserId={user.Id}&Code={code}"}
                                };
                await SendEmailAsync(user, code, "EmailConfirmation", "✅ Survey Basket: Email Confirmation", templateModel);
                return Result.Success();
            }
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.Failure(UserErrors.InvalidCode);

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);
            var code = request.Code;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return Result.Failure(UserErrors.InvalidCode);
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
                return Result.Success();
            }
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ResendConfirmEmailAsync(ReSendConfirmEmailRequest request, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();
            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            Dictionary<string, string> templateModel = new Dictionary<string, string>
                                {
                                    {"{{name}}",user.FirstName},
                                    {"{{action_url}}",$"{origin}/auth/confirm-email?UserId={user.Id}&Code={code}"}
                                };
            await SendEmailAsync(user, code, "EmailConfirmation", "✅ Survey Basket: Email Confirmation", templateModel);
            return Result.Success();
        }
        public async Task<Result> ForgetPasswordAsync(ReSendConfirmEmailRequest request, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();
            if (!user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            Dictionary<string, string> templateModel = new Dictionary<string, string>
                                {
                                    {"{{name}}",user.FirstName},
                                    {"{{action_url}}",$"{origin}/auth/reset-password?email={user.Email}&Code={code}"}
                                };
            await SendEmailAsync(user, code, "ForgetPassword", "✅ Survey Basket: Forget Password Confirmation", templateModel);
            return Result.Success();
        }
        public async Task<Result> ConfirmResetPasswordAsync(ResetPasswordRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();

            if (!user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = request.Code;
            IdentityResult result;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
                return Result.Failure(UserErrors.InvalidCode);
            }
            if (result.Succeeded)
            {
                return Result.Success();
            }
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized));
        }
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        private async Task SendEmailAsync(ApplicationUser user, string code, string templateName, string title, Dictionary<string, string> templateModel)
        {
            _logger.LogInformation("Code : {code}", code);
            var emailBody = EmailBodyBuilder.GenerateEmailBody(templateName,
                templateModel
                );
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, title, emailBody));
            await Task.CompletedTask;
        }
        private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissions(ApplicationUser user, CancellationToken cancellationToken)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var userPermissions = await (from r in _context.Roles
                                         join p in _context.RoleClaims
                                         on r.Id equals p.RoleId
                                         where userRoles.Contains(r.Name!)
                                         select p.ClaimValue!
                                         )
                                         .Distinct()
                                         .ToListAsync(cancellationToken);
            _logger.LogInformation("userRoles : {userRoles} , userPermissions : {userPermissions}", JsonSerializer.Serialize(userRoles), JsonSerializer.Serialize(userPermissions));
            return (userRoles, userPermissions);
        }
    }
}
