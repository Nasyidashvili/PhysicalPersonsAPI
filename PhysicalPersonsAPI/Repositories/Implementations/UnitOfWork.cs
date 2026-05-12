using PhysicalPersonsAPI.Data;
using PhysicalPersonsAPI.Repositories.Interfaces;

namespace PhysicalPersonsAPI.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IPersonRepository _persons;
        private IRelatedPersonRepository _relatedPersons;
        private IPhoneNumberRepository _numbers;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IPersonRepository Persons { get { return _persons ??= new PersonRepository(_context); } }

        public IPhoneNumberRepository PhoneNumbers { get { return _numbers ??= new PhoneNumberRepository(_context); } }
        public IRelatedPersonRepository RelatedPersons { get { return _relatedPersons ??= new RelatedPersonRepository(_context); } }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
