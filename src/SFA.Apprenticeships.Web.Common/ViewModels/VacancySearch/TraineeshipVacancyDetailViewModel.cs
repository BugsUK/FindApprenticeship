namespace SFA.Apprenticeships.Web.Common.ViewModels.VacancySearch
{
    using System;

    [Serializable]
    public class TraineeshipVacancyDetailViewModel : VacancyDetailViewModel
    {
        public TraineeshipVacancyDetailViewModel()
        {
        }

        public TraineeshipVacancyDetailViewModel(string message) : base(message)
        {
        }
    }
}