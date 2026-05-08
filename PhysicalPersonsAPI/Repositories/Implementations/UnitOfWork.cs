using PhysicalPersonsAPI.Data;
using PhysicalPersonsAPI.Repositories.Interfaces;

namespace PhysicalPersonsAPI.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IPersonRepository _persons;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IPersonRepository Persons { get { return _persons ??= new PersonRepository(_context); } }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
