namespace SFA.Apprenticeships.Domain.Entities.Raa.Locations
{
    public class VacancyLocation
    {
        public int VacancyLocationId { get; set; }
        public int VacancyId { get; set; }
        public PostalAddress Address { get; set; }
        public int NumberOfPositions { get; set; }
        public string LocalAuthorityCode { get; set; }
    }
}