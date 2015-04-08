namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    public enum UpdateEmailStatus
    {
        Ok,
        Updated,
        AccountAlreadyExixts,
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

        [Display(Name = VertifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.LabelText)]
        public string PendingUsernameCode { get; set; }

        [Display(Name = VertifyUpdatedEmailViewModelMessages.PasswordMessages.LabelText)]
        public string Password { get; set; }
    }
}