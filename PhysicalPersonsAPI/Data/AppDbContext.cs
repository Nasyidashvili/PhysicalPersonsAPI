using Microsoft.EntityFrameworkCore;
using PhysicalPersonsAPI.Models;

namespace PhysicalPersonsAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set;  }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<PhysicalPerson> PhysicalPersons { get; set; }
        public DbSet<RelatedPerson> RelatedPersons { get; set; }
    }
}
