using Microsoft.EntityFrameworkCore;
using PhysicalPersonsAPI.Models;

namespace PhysicalPersonsAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RelatedPerson>(entity =>
            {
                entity.HasOne<PhysicalPerson>()
                    .WithMany(x => x.RelatedPersons)
                    .HasForeignKey(x => x.PhysicalPersonId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.RelativePerson)
                    .WithMany()
                    .HasForeignKey(x => x.RelativePersonId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, CityName = "Tbilisi" },
                new City { Id = 2, CityName = "Batumi" },
                new City { Id = 3, CityName = "Kutaisi" }
                );
        }

        public DbSet<City> Cities { get; set;  }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<PhysicalPerson> PhysicalPersons { get; set; }
        public DbSet<RelatedPerson> RelatedPersons { get; set; }
    }
}
