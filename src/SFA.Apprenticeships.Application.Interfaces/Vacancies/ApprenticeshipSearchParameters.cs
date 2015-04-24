namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Vacancies.Apprenticeships;
    using Search;

    public class ApprenticeshipSearchParameters : VacancySearchParametersBase
    {
        public string Keywords { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public string Category { get; set; }

        public string[] SubCategories { get; set; }

        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        public ApprenticeshipSearchField SearchField { get; set; }
        public override string ToString()
        {
            var joinedFrameworks = (SubCategories == null || SubCategories.Length == 0)
                ? string.Empty
                : string.Join(",", SubCategories);
            return string.Format("{0}, Keywords:{1}, ApprenticeshipLevel:{2}, Category:{3}, SubCategories:{4}, LocationType:{5}", base.ToString(), Keywords, ApprenticeshipLevel, Category, joinedFrameworks, VacancyLocationType);
        }
    }
}
