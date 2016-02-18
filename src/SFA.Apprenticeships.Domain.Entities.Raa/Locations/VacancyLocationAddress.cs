namespace SFA.Apprenticeships.Domain.Entities.Raa.Locations
{
    public class VacancyLocationAddress
    {
        public int VacancyId { get; set; }

        public PostalAddress Address { get; set; }

        public int NumberOfPositions { get; set; }
    }
}