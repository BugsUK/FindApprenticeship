namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    public class ApplicationsLinkViewModel
    {
        public const string PartialView = "_ApplicationsLink";

        public ApplicationsLinkViewModel(int vacancyReferenceNumber, int applicationCount)
        {
            VacancyReferenceNumber = vacancyReferenceNumber;
            ApplicationCount = applicationCount;
        }

        public int VacancyReferenceNumber { get; private set; }
        public int ApplicationCount { get; private set; }
    }
}
