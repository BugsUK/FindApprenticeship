namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;

    public class EditStandardViewModel
    {
        public int Id { get; set; }

        public string Sector { get; set; }

        public string Name { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public string StandardSectorName { get; set; }
        public string StandardName { get; set; }

        public FrameworkStatusType Status { get; set; }
    }
}