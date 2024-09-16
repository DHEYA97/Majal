﻿namespace Majal.Core.Contract.Auth
{
    public record AuthResponse(
         string Id,
         string Email,
         string FirstName,
         string LastName,
         string Token,
         int ExpireDate
        );
}