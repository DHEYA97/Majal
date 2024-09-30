using Majal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Majal.Repository.Data.Configurations
{
    public class OurSystemConfiguration : IEntityTypeConfiguration<OurSystem>
    {
        public void Configure(EntityTypeBuilder<OurSystem> builder)
        {
            
            builder.HasMany(s => s.SystemImages)
                   .WithOne(si => si.OurSystem)
                   .HasForeignKey(si => si.OurSystemId);


            builder.HasOne(s=>s.Image)
                    .WithOne()
                    .HasForeignKey<OurSystem>(c=>c.MainImageId);
        }
    }
}
