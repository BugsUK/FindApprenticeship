namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(FeedbackClientViewModelValidator))]
    public class FeedbackViewModel
    {
        [Display(Name = FeedbackViewModelMessages.FullNameMessages.LabelText)]
        public string Name { get; set; }

        [Display(Name = FeedbackViewModelMessages.EmailAddressMessages.LabelText)]
        public string Email { get; set; }

        [Display(Name = FeedbackViewModelMessages.DetailsMessages.LabelText)]
        public string Details { get; set; }
    }
}