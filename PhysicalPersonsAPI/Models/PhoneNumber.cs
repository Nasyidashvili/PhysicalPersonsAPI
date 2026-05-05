using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace PhysicalPersonsAPI.Models
{
    public enum PhoneType{
        PersonalNumber,
        WorkNumber,
        HomeNumber
    }
    public class PhoneNumber
    {

        public int Id {  get; set; }
        public string? Number { get; set; }
        public PhoneType NumberType { get; set; }
        public int PhysicalPersonId { get; set; }
    }
}
