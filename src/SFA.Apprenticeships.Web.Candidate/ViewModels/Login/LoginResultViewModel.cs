namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Login
{
    using Common.ViewModels;
    using Domain.Entities.Users;

    public class LoginResultViewModel : ViewModelBase
    {
        public LoginResultViewModel()
        {
        }

        public LoginResultViewModel(string message)
            : base(message)
        {
        }

        public string EmailAddress { get; set; }

        public string FullName { get; set; }

        public UserStatuses? UserStatus { get; set; }

        public bool IsAuthenticated { get; set; }

        public string AcceptedTermsAndConditionsVersion { get; set; }

        public string PhoneNumber { get; set; }

        public bool MobileVerificationRequired { get; set; }

        public bool PendingUsernameVerificationRequired { get; set; }

        public string ReturnUrl { get; set; }
    }
}