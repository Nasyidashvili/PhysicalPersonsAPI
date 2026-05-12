using Microsoft.EntityFrameworkCore;
using PhysicalPersonsAPI.Data;
using PhysicalPersonsAPI.Models;
using PhysicalPersonsAPI.Repositories.Interfaces;

namespace PhysicalPersonsAPI.Repositories.Implementations
{
    public class PhoneNumberRepository : IPhoneNumberRepository
    {
        private readonly AppDbContext _context;

        public PhoneNumberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PhoneNumber number)
        {
            _context.PhoneNumbers.Add(number);
        }

        public async Task<IEnumerable<PhoneNumber>> GetIdAsync(int personId)
        {
            return await _context.PhoneNumbers.Where(p => p.PhysicalPersonId == personId).ToListAsync();
        }

        public void RemoveRange(IEnumerable<PhoneNumber> numbers)
        {
            _context.PhoneNumbers.RemoveRange(numbers);
        }

    }
}
