using PhysicalPersonsAPI.Models;
using System.ComponentModel.DataAnnotations;
using PhysicalPersonsAPI.Validation;



namespace PhysicalPersonsAPI.DTOS
{
    public class SearchDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalNumber { get; set; }
        public GenderType? Gender { get; set; }
        public int? CityId { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
