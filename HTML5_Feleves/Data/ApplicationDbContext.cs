using HTML5_Feleves.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HTML5_Feleves.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
              new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
              new { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" }
            );

            PasswordHasher<AppUser> ph = new PasswordHasher<AppUser>();
            AppUser test = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "tesztelek@test.hu",
                EmailConfirmed = true,
                UserName = "tesztelek@test.hu",
                FirstName = "Dorina",
                LastName = "Ottó",
                NormalizedUserName = "TESZTELEK@TEST.HU"
            };
            test.PasswordHash = ph.HashPassword(test, "asd123456");
            builder.Entity<AppUser>().HasData(test);

            base.OnModelCreating(builder);
        }
    }
}