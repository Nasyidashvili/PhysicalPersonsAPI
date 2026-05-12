using PhysicalPersonsAPI.Models;

namespace PhysicalPersonsAPI.Repositories.Interfaces
{
    public interface IRelatedPersonRepository
    {
        Task AddAsync(RelatedPerson person);
        Task<RelatedPerson?> GetBothIdAsync(int personId, int relativeId);
        Task<IEnumerable<RelatedPerson>> GetByPersonIdAsync(int personId);
        void Remove(RelatedPerson persons);
        Task<int> CountAsync();
    }
}
