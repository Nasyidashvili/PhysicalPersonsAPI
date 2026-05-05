using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PhysicalPersonsAPI.Validation
{
    public class GeorgianOrLatin : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
           if (value == null)
            {
                return false;
            }

            bool isGeorgian = Regex.IsMatch(value.ToString()!, @"^^[\u10D0-\u10FF]+$");
            bool isLatin = Regex.IsMatch(value.ToString()!, @"^[a-zA-Z]+$");
            return isGeorgian || isLatin;
        }
    }
}
