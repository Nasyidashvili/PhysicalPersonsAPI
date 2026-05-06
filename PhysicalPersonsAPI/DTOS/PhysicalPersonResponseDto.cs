using PhysicalPersonsAPI.Models;

namespace PhysicalPersonsAPI.DTOS
{
    public class PhysicalPersonResponseDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalNumber { get; set; }
        public GenderType Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string? CityName { get; set; }
        public List<PhoneNumberResponseDto> PhoneNumbers { get; set; } = new();

    }
}
