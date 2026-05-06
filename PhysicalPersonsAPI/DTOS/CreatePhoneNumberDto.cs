using PhysicalPersonsAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace PhysicalPersonsAPI.DTOS
{
    public class CreatePhoneNumberDto
    {
        [StringLength(50, MinimumLength =4)]
        public string? Number { get; set; }
        public PhoneType PhoneType { get; set; }
    }
}
