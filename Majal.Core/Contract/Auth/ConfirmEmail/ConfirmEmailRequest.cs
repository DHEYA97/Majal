namespace Majal.Core.Contract.Auth
{
    public record ConfirmEmailRequest(
        string UserId,
        string Code
        );
}
