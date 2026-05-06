using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PhysicalPersonsAPI.Models;

namespace PhysicalPersonsAPI.DTOS
{
    public class PhoneNumberResponseDto
    {
        public string? Number { get; set; } 
        public PhoneType PhoneType { get; set; }
    }
}
