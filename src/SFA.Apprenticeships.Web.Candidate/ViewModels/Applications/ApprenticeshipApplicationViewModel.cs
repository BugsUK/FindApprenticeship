namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
    using System.Configuration;
    using Domain.Entities.Vacancies;
    using FluentValidation.Attributes;
    using Domain.Entities.Applications;
    using Candidate;
    using Common.Models.Application;
    using VacancySearch;
    using Validators;

    [Validator(typeof(ApprenticeshipApplicationViewModelClientValidator))]
    [Serializable]
    public class ApprenticeshipApplicationViewModel : ApplicationViewModelBase<ApprenticeshipCandidateViewModel>
    {
        public readonly string AutoSaveTimeInMinutes = ConfigurationManager.AppSettings["AutoSaveTimeInMinutes"];

        public ApprenticeshipApplicationViewModel(string message, ApplicationViewModelStatus viewModelStatus)
            : base(message, viewModelStatus)
        {
        }

        public ApprenticeshipApplicationViewModel(string message)
            : base(message)
        {
        }

        public ApprenticeshipApplicationViewModel(ApplicationViewModelStatus viewModelStatus)
            : base(viewModelStatus)
        {
        }

        public ApprenticeshipApplicationViewModel()
        {
        }

        public ApplicationStatuses Status { get; set; }

        public ApprenticeshipVacancyDetailViewModel VacancyDetail { get; set; }

        public string ProviderName { get; set; }
        public string Contact { get; set; }

        //TODO: Think of a better name
        public bool IsExpiredOrWithdrawn()
        {
            return Status == ApplicationStatuses.ExpiredOrWithdrawn || VacancyDetail.VacancyStatus != VacancyStatuses.Live;
        }

        public bool IsNotFound()
        {
            return ViewModelStatus == ApplicationViewModelStatus.ApplicationNotFound;
        }
    }
}