namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Common.ViewModels.Applications;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(ApprenticeshipApplicationPreviewViewModelValidator))]
    [Serializable]
    public class ApprenticeshipApplicationPreviewViewModel : ApprenticeshipApplicationViewModel
    {
        public ApprenticeshipApplicationPreviewViewModel()
        {
        }

        public ApprenticeshipApplicationPreviewViewModel(string message)
            : base(message)
        {
        }

        [Display(Name = ApprenticeshipApplicationPreviewViewModelMessages.AcceptSubmit.LabelText)]
        public bool AcceptSubmit { get; set; }
    }
}