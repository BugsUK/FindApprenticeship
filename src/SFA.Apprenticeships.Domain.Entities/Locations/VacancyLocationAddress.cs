namespace SFA.Apprenticeships.Domain.Entities.Locations
{
    public class VacancyLocationAddress : Address
    {
        public int NumberOfPositions { get; set; }
        public long VacancyReferenceNumber { get; set; }
    }
}