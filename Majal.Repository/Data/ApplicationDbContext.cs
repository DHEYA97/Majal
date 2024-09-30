using Majal.Core.Entities;
using Majal.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Majal.Core.Extensions;
namespace Majal.Repository.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) :
        IdentityDbContext<ApplicationUser, ApplicationRole,string>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Client> Clients { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<OurSystem> OurSystems { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategorys { get; set; }
        public DbSet<SystemImage> SystemImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add Global Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Change OnDelete Behavior
            var cascadeFk = modelBuilder.Model
                                        .GetEntityTypes()
                                        .SelectMany(t => t.GetForeignKeys())
                                        .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFk)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var Entities = ChangeTracker.Entries<BaseEntity>();
            foreach (var entity in Entities)
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()!;
                if (entity.State == EntityState.Added)
                {
                    entity.Property(x => x.CreatedById).CurrentValue = currentUserId;
                }
                else if (entity.State == EntityState.Modified)
                {
                    entity.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                    entity.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        
    }
}
