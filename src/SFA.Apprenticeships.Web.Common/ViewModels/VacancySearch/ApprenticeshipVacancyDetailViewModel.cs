namespace SFA.Apprenticeships.Web.Common.ViewModels.VacancySearch
{
    using System;

    [Serializable]
    public class ApprenticeshipVacancyDetailViewModel : VacancyDetailViewModel
    {
        public ApprenticeshipVacancyDetailViewModel()
        {
        }

        public ApprenticeshipVacancyDetailViewModel(string message) : base(message)
        {
        }

        public bool IsMultiLocation { get; set; }
    }
}