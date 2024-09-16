namespace Majal.Core.Contract.Auth.User
{
    public record AddUserRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        IList<string>Roles
        );
}
