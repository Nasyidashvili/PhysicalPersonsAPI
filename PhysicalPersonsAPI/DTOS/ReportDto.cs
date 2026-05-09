namespace PhysicalPersonsAPI.DTOS
{
    public class ReportDto
    {
        public int TotalPersons { get; set; }
        public Dictionary<string, int> ByGender { get; set; }
        public int RelatedPersons { get; set; }
    }
}
