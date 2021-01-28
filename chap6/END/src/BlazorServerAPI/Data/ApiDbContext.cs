using BlazorAppShared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerAPI.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public ApiDbContext(
            DbContextOptions<ApiDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(c =>
            {
                c.HasKey(n => n.Id);
                c.Property(n => n.Name).IsRequired();

                c.HasData(new[]
                {
                    new Country{ Id = 1, Name = "France" },
                    new Country{ Id = 2, Name = "Belgique" },
                    new Country{ Id = 3, Name = "Suisse" },
                    new Country{ Id = 4, Name = "Canada" },
                    new Country{ Id = 5, Name = "Autre" }
                });
            });

            modelBuilder.Entity<Job>(j =>
            {
                j.HasKey(n => n.Id);
                j.Property(n => n.Title).IsRequired();

                j.HasData(new[]
                {
                     new Job { Id = 1, Title = "Manager" },
                     new Job { Id = 2, Title = "Journaliste" },
                     new Job { Id = 3, Title = "Développeur" },
                     new Job { Id = 4, Title = "Directeur" },
                     new Job { Id = 5, Title = "Secrétaire" }
                });
            });

            modelBuilder.Entity<Employee>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Name).IsRequired();
                e.Property(e => e.Lastname).IsRequired();
                e.HasOne(e => e.Job).WithMany();
                e.OwnsOne(e => e.Address).HasOne(a => a.Country).WithMany();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
