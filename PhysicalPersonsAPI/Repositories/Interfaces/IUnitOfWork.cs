namespace PhysicalPersonsAPI.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IPersonRepository Persons { get; }
        IPhoneNumberRepository PhoneNumbers { get; }
        IRelatedPersonRepository RelatedPersons { get; }
        Task SaveAsync();
    }
}
