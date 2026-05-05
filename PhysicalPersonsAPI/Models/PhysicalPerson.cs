using System.Runtime.CompilerServices;

namespace PhysicalPersonsAPI.Models
{
    public enum GenderType
    {
        Male,
        Female
    }
    public class PhysicalPerson
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalNumber { get; set; }
        public GenderType Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string? ImagePath { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }
    }
}
