using PhysicalPersonsAPI.Models;

namespace PhysicalPersonsAPI.DTOS
{
    public class AddRelatedPersonDto
    {
        public int RelatedId { get; set; }
        public RelationType Type { get; set; }
    }
}
