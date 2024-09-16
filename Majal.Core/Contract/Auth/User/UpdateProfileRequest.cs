namespace Majal.Core.Contract.Auth.User;

public record UpdateProfileRequest(
    string FirstName,
    string LastName
);