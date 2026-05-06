using PhysicalPersonsAPI.Models;
using System.ComponentModel.DataAnnotations;
using PhysicalPersonsAPI.Validation;

namespace PhysicalPersonsAPI.DTOS
{
    public class CreatePhysicalPersonDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        [GeorgianOrLatin(ErrorMessage = "First name must contain only Georgian or Latin characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        [GeorgianOrLatin(ErrorMessage = "Last name must contain only Georgian or Latin characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Personal number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Personal number must be exactly 11 digits.")]
        public string? PersonalNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public GenderType Gender { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [MinimumAge(ErrorMessage = "Physical person must be at least 18 years old.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "City ID is required.")]
        public int CityId { get; set; }

        public List<CreatePhoneNumberDto> PhoneNumbers { get; set; } = new();

    }
}
