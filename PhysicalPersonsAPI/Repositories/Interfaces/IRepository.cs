namespace PhysicalPersonsAPI.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int Id);
        Task AddAsync(T Entity);
        Task DeleteAsync(int Id);
        Task UpdateAsync(T Entity);
        Task SaveAsync();
    }
}
