using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PhysicalPersonsAPI.Data;
using PhysicalPersonsAPI.Models;
using PhysicalPersonsAPI.Repositories.Interfaces;

namespace PhysicalPersonsAPI.Repositories.Implementations
{
    public class RelatedPersonRepository : IRelatedPersonRepository
    {
        private readonly AppDbContext _context;

        public RelatedPersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RelatedPerson person)
        {
            _context.RelatedPersons.Add(person);
        }

        public async Task<RelatedPerson?> GetBothIdAsync(int personId, int relativeId)
        {
            return await _context.RelatedPersons.FirstOrDefaultAsync(p => p.PhysicalPersonId == personId && p.RelativePersonId == relativeId);
        }

        public async Task<IEnumerable<RelatedPerson>> GetByPersonIdAsync(int personId)
        {
            return await _context.RelatedPersons
                .Include(p => p.RelativePerson)
                .Where(p => p.PhysicalPersonId == personId)
                .ToListAsync();
        }

        public async void Remove(RelatedPerson persons)
        {
            _context.RelatedPersons.Remove(persons);
        }

        public async Task<int> CountAsync()
        {
            return await _context.RelatedPersons.CountAsync();
        }
    }
}
