using PhysicalPersonsAPI.Models;
using PhysicalPersonsAPI.DTOS;

namespace PhysicalPersonsAPI.Repositories.Interfaces
{
    public interface IPersonRepository : IRepository<PhysicalPerson>
    {
        Task<(IEnumerable<PhysicalPerson> Items, int TotalCount)> SearchAsync(
            string? firstName,
            string? lastName,
            string? personalNumber,
            int pageNumber,
            int pageSize);
        Task<PhysicalPerson?> GetWithRelatedAsync(int id);
        Task<IEnumerable<PhysicalPerson>> GetAllWithCityAsync();
        Task<PhysicalPerson?> GetByIdDetailsAsync(int id);
    }
}
