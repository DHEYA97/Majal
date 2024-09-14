using Majal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Majal.Repository.Data.Configurations
{
    public class EmployeeDepartmentConfiguration : IEntityTypeConfiguration<EmployeeDepartment>
    {
        public void Configure(EntityTypeBuilder<EmployeeDepartment> builder)
        {
            builder.HasKey(ed => new { ed.EmployeeId, ed.DepartmentId });

            builder.HasOne(ed => ed.Employee)
                   .WithMany(e => e.Departments)
                   .HasForeignKey(ed => ed.EmployeeId);

            builder.HasOne(ed => ed.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(ed => ed.DepartmentId);
        }
    }
}
