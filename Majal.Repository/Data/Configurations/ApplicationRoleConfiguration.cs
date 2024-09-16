using Majal.Core.Abstractions.Const;
using Majal.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Repository.Data.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {

            //Seeding data

            builder.HasData(
                   [
                       new ApplicationRole{
                           Id = DefaultRoles.AdminRoleId,
                           ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp,
                           Name = DefaultRoles.Admin,
                           NormalizedName = DefaultRoles.Admin.ToUpper(),
                       },
                       new ApplicationRole{
                           Id = DefaultRoles.MemberRoleId,
                           ConcurrencyStamp = DefaultRoles.MemberRoleConcurrencyStamp,
                           Name = DefaultRoles.Member,
                           NormalizedName = DefaultRoles.Member.ToUpper(),
                           IsDefault = true,
                       },
                       new ApplicationRole{
                           Id = DefaultRoles.ContentWriterRoleId,
                           ConcurrencyStamp = DefaultRoles.ContentWriterRoleConcurrencyStamp,
                           Name = DefaultRoles.ContentWriter,
                           NormalizedName = DefaultRoles.ContentWriter.ToUpper(),
                           IsDefault = true,
                       },
                   ]
                );
        }
    }
}
