using Majal.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Majal.Core.Abstractions.Const;

namespace Majal.Repository.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            
            builder.Property(u => u.FirstName).HasMaxLength(100);
            builder.Property(u => u.LastName).HasMaxLength(100);

            //Seeding data
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(
                [
                    new ApplicationUser
                    {
                        Id = DefaultUsers.AdminId,
                        FirstName = "Admin",
                        LastName = "Majal",
                        UserName = DefaultUsers.AdminEmail,
                        NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
                        Email = DefaultUsers.AdminEmail,
                        NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                        SecurityStamp = DefaultUsers.AdminSecurityStamp,
                        ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                        PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword),
                        EmailConfirmed = true
                    },
                    new ApplicationUser
                    {
                        Id = DefaultUsers.MemberId,
                        FirstName = "Member",
                        LastName = "Majal",
                        UserName = DefaultUsers.MemberEmail,
                        NormalizedUserName = DefaultUsers.MemberEmail.ToUpper(),
                        Email = DefaultUsers.MemberEmail,
                        NormalizedEmail = DefaultUsers.MemberEmail.ToUpper(),
                        SecurityStamp = DefaultUsers.MemberSecurityStamp,
                        ConcurrencyStamp = DefaultUsers.MemberConcurrencyStamp,
                        PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.MemberPassword),
                        EmailConfirmed = true
                    },
                    new ApplicationUser
                    {
                        Id = DefaultUsers.ContentWriterId,
                        FirstName = "ContentWriter",
                        LastName = "Majal",
                        UserName = DefaultUsers.ContentWriterEmail,
                        NormalizedUserName = DefaultUsers.ContentWriterEmail.ToUpper(),
                        Email = DefaultUsers.ContentWriterEmail,
                        NormalizedEmail = DefaultUsers.ContentWriterEmail.ToUpper(),
                        SecurityStamp = DefaultUsers.ContentWriterSecurityStamp,
                        ConcurrencyStamp = DefaultUsers.ContentWriterConcurrencyStamp,
                        PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.ContentWriterPassword),
                        EmailConfirmed = true
                    }
                ]
                );
        }
    }
}
