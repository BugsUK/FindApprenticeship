namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Common.ViewModels;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    public enum UpdateEmailStatus
    {
        Ok,
        Updated,
        AccountAlreadyExists,
        UserPasswordError,
        InvalidUpdateUsernameCode,
        Error
    }

    [Validator(typeof(VerifyUpdatedEmailViewModelClientValidator))]
    public class VerifyUpdatedEmailViewModel : ViewModelBase
    {
        public VerifyUpdatedEmailViewModel()
        {
        }

        public VerifyUpdatedEmailViewModel(string message) : base(message)
        {
        }

        public UpdateEmailStatus UpdateStatus { get; set; }

        [Display(Name = VerifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.LabelText)]
        public string PendingUsernameCode { get; set; }

        [Display(Name = VerifyUpdatedEmailViewModelMessages.PasswordMessages.LabelText, Description = VerifyUpdatedEmailViewModelMessages.PasswordMessages.HintText)]
        public string VerifyPassword { get; set; }
    }
}