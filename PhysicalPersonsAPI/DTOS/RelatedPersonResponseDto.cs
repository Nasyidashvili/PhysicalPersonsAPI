using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PhysicalPersonsAPI.Models;

namespace PhysicalPersonsAPI.DTOS
{
    public class RelatedPersonResponseDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public RelationType Type { get; set; }
    }
}
