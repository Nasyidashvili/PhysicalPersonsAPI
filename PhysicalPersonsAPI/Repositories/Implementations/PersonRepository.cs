using PhysicalPersonsAPI.Models;
using PhysicalPersonsAPI.Repositories.Interfaces;
using PhysicalPersonsAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace PhysicalPersonsAPI.Repositories.Implementations
{
    public class PersonRepository : Repository<PhysicalPerson>, IPersonRepository
    {
        public PersonRepository(AppDbContext context) : base(context) { }

        public async Task<(IEnumerable<PhysicalPerson> Items, int TotalCount)> SearchAsync(
            string? firstName,
            string? lastName,
            string? personalNumber,
            int pageNumber,
            int pageSize)
        {
            var query  = _dbSet.AsQueryable();
            
            if(!firstName.Equals(null))
            {
                query = query.Where(p => p.FirstName.ToLower().Contains(firstName.ToLower()));
            }
            if (!lastName.Equals(null))
            {
                query = query.Where(p => p.LastName.ToLower().Contains(lastName.ToLower()));
            }
            if (!personalNumber.Equals(null))
            {
                query = query.Where(p => p.PersonalNumber.ToLower().Contains(personalNumber.ToLower()));
            }
            
            int TotalCount = await query.CountAsync();

            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var Items = await query.ToListAsync();

            return (Items, TotalCount);
        }
        public async Task<PhysicalPerson?> GetWithRelatedAsync(int id)
        { 
            return await _dbSet
                .Include(p => p.RelatedPersons)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public async Task<IEnumerable<PhysicalPerson>> GetAllWithCityAsync()
        {
            return await _dbSet
                .Include(p => p.City)
                .Include(p => p.PhoneNumbers)
                .ToListAsync(); 
        }

        public async Task<PhysicalPerson?> GetByIdDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.City)
                .Include(p => p.PhoneNumbers)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
