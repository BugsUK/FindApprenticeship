namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using Reference;

    public class Standard
    {
        public int Id { get; set; }

        public int ApprenticeshipSectorId { get; set; }

        public string Name { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public FrameworkStatusType Status { get; set; }
    }
}