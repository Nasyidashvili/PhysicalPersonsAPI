using PhysicalPersonsAPI.Models;
using PhysicalPersonsAPI.DTOS;

namespace PhysicalPersonsAPI.Repositories.Interfaces
{
    public interface IPersonRepository : IRepository<PhysicalPerson>
    {
        Task<(IEnumerable<PhysicalPerson> Items, int TotalCount)> SearchAsync(SearchDto dto);
        Task<PhysicalPerson?> GetWithRelatedAsync(int id);
        Task<IEnumerable<PhysicalPerson>> GetAllCityAsync();
        Task<PhysicalPerson?> GetByIdDetailsAsync(int id);
        Task<PhysicalPerson?> FindIdAsync(int id);
        Task<Dictionary<string, int>> CountByGennderAsync();
    }
}
