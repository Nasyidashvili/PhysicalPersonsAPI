using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PhysicalPersonsAPI.Validation
{
    public class MinimumAge : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }
            if (value is DateTime birthDate) {
                var today = DateTime.Today;
                var age = today.Year - birthDate.Year;
                return age >= 18;
            }
            return false;
        }
    }
}
