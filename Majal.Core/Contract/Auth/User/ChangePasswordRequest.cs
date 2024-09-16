namespace Majal.Core.Contract.Auth.User;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);