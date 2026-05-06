using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace PhysicalPersonsAPI.Models
{
    public enum PhoneType{
        Mobile,
        Work,
        Home
    }
    public class PhoneNumber
    {

        public int Id {  get; set; }
        public string? Number { get; set; }
        public PhoneType NumberType { get; set; }
        public int PhysicalPersonId { get; set; }
    }
}
