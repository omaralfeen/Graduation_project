using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Graduation_project.Models
{
    public class Graduation_projectContext : IdentityDbContext<ApplicationUser>
    {
        //public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Craftsman> Craftsmen { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Offer> Offers { get; set; }

        //--
        public Graduation_projectContext(DbContextOptions<Graduation_projectContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Craftsman>()
                .Property(c => c.Craft_Type)
                .HasConversion<string>();

            modelBuilder.Entity<Service>()
                .Property(s => s.Type)
                .HasConversion<string>();
        }



    }
}
