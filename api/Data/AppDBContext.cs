
using api.models;
using api.models.authors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDBContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {

        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Author> authors { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            List<IdentityRole> roles =
        [
            new IdentityRole
                {
                    Name = "admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "customer",
                    NormalizedName = "CUSTOMER"
                }
        ];
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }


}