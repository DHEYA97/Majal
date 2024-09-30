using Majal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Majal.Repository.Data.Configurations
{
    public class SystemImageConfiguration : IEntityTypeConfiguration<SystemImage>
    {
        public void Configure(EntityTypeBuilder<SystemImage> builder)
        {
            builder.HasKey(si => new { si.OurSystemId, si.ImageId });

            builder.HasOne(s => s.OurSystem)
                   .WithMany(si => si.SystemImages)
                   .HasForeignKey(si => si.OurSystemId);

            builder.HasOne(s => s.Image)
                   .WithMany()
                   .HasForeignKey(si => si.ImageId);
        }
    }
}
