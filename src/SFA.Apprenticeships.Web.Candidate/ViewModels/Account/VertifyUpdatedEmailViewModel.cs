namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Applications;
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

    //[Validator(typeof(VerifyMobileViewModelClientValidator))]
    public class VertifyUpdatedEmailViewModel : ViewModelBase
    {
        public VertifyUpdatedEmailViewModel()
        {
        }

        public VertifyUpdatedEmailViewModel(string message)
            : base(message)
        {
        }

        public UpdateEmailStatus UpdateStatus { get; set; }

        //[Display(Name = VerifyMobileViewModelMessages.MobileNumberCodeMessages.LabelText)]
        public string PendingUsernameCode { get; set; }

        //[Display(Name = VerifyMobileViewModelMessages.VerifyMobileCodeMessages.LabelText)]
        public string Password { get; set; }
    }
}