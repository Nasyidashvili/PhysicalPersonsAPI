namespace PhysicalPersonsAPI.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IPersonRepository Persons { get; }

        Task SaveAsync();
    }
}
