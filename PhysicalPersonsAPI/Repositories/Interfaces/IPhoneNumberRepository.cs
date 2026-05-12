using PhysicalPersonsAPI.Models;

namespace PhysicalPersonsAPI.Repositories.Interfaces
{
    public interface IPhoneNumberRepository
    {
        Task AddAsync(PhoneNumber number);
        Task<IEnumerable<PhoneNumber>> GetIdAsync(int personId);
        void RemoveRange(IEnumerable<PhoneNumber> numbers);
    }
}
