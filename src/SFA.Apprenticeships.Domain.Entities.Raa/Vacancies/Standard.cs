namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    public class Standard
    {
        public int Id { get; set; }

        public int ApprenticeshipSectorId { get; set; }

        public string Name { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
    }
}