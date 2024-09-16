using Majal.Core.Abstractions.Const;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Repository.Data.Configurations
{
    public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {

            //Seeding data

            builder.HasData(
                [
                     new IdentityUserRole<string>
                     {
                         UserId = DefaultUsers.AdminId,
                         RoleId = DefaultRoles.AdminRoleId
                     },
                     new IdentityUserRole<string>
                     {
                         UserId = DefaultUsers.MemberId,
                         RoleId = DefaultRoles.MemberRoleId
                     },
                     new IdentityUserRole<string>
                     {
                         UserId = DefaultUsers.ContentWriterId,
                         RoleId = DefaultRoles.ContentWriterRoleId
                     }
                ]
            );
    }
}
}