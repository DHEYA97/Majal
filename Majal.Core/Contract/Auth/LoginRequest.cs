namespace Majal.Core.Contract.Auth
{
    public record LoginRequest(
        string Email,
        string Password
       );
}
