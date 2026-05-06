namespace PhysicalPersonsAPI.Models
{
    public enum RelationType
    {
        Colleague,
        Acquaintance,
        Relative,
        Other
    }
    public class RelatedPerson
    {
        public int Id { get; set; }
        public int PhysicalPersonId { get; set; }
        public RelationType Related { get; set; }
        public int RelativePersonId { get; set; }

        public PhysicalPerson? RelativePerson { get; set; }
    }
}
