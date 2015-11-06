namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;

    public class StandardViewModel
    {
        public int Id { get; set; }

        public string Sector { get; set; }

        public string Name { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
    }
}