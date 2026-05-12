using PhysicalPersonsAPI.Models;
using PhysicalPersonsAPI.Repositories.Interfaces;
using PhysicalPersonsAPI.Data;
using Microsoft.EntityFrameworkCore;
using PhysicalPersonsAPI.DTOS;

namespace PhysicalPersonsAPI.Repositories.Implementations
{
    public class PersonRepository : Repository<PhysicalPerson>, IPersonRepository
    {
        public PersonRepository(AppDbContext context) : base(context) { }

        public async Task<(IEnumerable<PhysicalPerson> Items, int TotalCount)> SearchAsync(SearchDto dto)
        {
            var query  = _dbSet
                .Include(p => p.City)
                .Include(p => p.PhoneNumbers)
                .AsQueryable();

            if(!string.IsNullOrEmpty(dto.FirstName))
            {
                query = query.Where(p => p.FirstName.ToLower().Contains(dto.FirstName.ToLower()));
            }
            if (!string.IsNullOrEmpty(dto.LastName))
            {
                query = query.Where(p => p.LastName.ToLower().Contains(dto.LastName.ToLower()));
            }
            if (!string.IsNullOrEmpty(dto.PersonalNumber))
            {
                query = query.Where(p => p.PersonalNumber.Contains(dto.PersonalNumber));
            }
            if (dto.Gender != null)
            {
                query = query.Where(p => p.Gender == dto.Gender);
            }
            if (dto.CityId != null)
            {
                query = query.Where(p => p.CityId == dto.CityId);
            }



            int TotalCount = await query.CountAsync();

            var items = await query
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();

            return (items, TotalCount);
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

        public async Task<PhysicalPerson?> FindIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Dictionary<string, int>> CountByGennderAsync()
        {
            var result = await _dbSet
                .GroupBy(p => p.Gender)
                .Select(p => new { Key = p.Key, Count = p.Count() })
                .ToListAsync();
            
            return result.ToDictionary(p => p.Key.ToString() ?? "Unknown", p=> p.Count);
        }
    }
}
