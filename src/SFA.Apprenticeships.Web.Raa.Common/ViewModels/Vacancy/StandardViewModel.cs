namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;

    public class StandardViewModel
    {
        public int Id { get; set; }

        public string Sector { get; set; }

        public string Name { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public FrameworkStatusType Status { get; set; }
    }
}