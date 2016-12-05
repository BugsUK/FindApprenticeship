namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Vacancies;
    using Search;

    public class ApprenticeshipSearchParameters : VacancySearchParametersBase
    {
        public string Keywords { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public string CategoryCode { get; set; }

        public string[] SubCategoryCodes { get; set; }

        public VacancyLocationType VacancyLocationType { get; set; }

        public ApprenticeshipSearchField SearchField { get; set; }
        public override string ToString()
        {
            var joinedFrameworks = (SubCategoryCodes == null || SubCategoryCodes.Length == 0)
                ? string.Empty
                : string.Join(",", SubCategoryCodes);
            return string.Format("{0}, Keywords:{1}, ApprenticeshipLevel:{2}, CategoryCode:{3}, SubCategoryCodes:{4}, LocationType:{5}", base.ToString(), Keywords, ApprenticeshipLevel, CategoryCode, joinedFrameworks, VacancyLocationType);
        }
    }
}
