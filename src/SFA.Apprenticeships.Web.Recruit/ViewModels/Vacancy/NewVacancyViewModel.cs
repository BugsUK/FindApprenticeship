namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;

    public class NewVacancyViewModel
    {
        public string SiteUrn { get; set; }

        public Category[] Categories { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
    }
}
