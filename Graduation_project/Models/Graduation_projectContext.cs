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


    }
}
