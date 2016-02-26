namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Common.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(EmailViewModelClientValidator))]
    public class EmailViewModel : ViewModelBase
    {
        public EmailViewModel()
        {
        }

        public EmailViewModel(string message) : base(message)
        {
        }

        [Display(Name = "Enter new email address")]
        public string EmailAddress { get; set; }

        public UpdateEmailStatus UpdateStatus { get; set; }
    }
}