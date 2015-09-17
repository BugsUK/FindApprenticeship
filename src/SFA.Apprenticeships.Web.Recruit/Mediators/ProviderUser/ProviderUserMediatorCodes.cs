namespace SFA.Apprenticeships.Web.Recruit.Mediators.ProviderUser
{
    public static class ProviderUserMediatorCodes
    {
        public static class GetProviderUserViewModel
        {
            public const string Ok = "ProviderUser.GetProviderUserViewModel.Ok";
            public const string DoesntExist = "ProviderUser.GetProviderUserViewModel.DoesntExist";
        }

        public static class UpdateUser
        {
            public const string Ok = "ProviderUser.UpdateUser.Ok";
            public const string FailedValidation = "ProviderUser.UpdateUser.FailedValidation";
            public const string EmailUpdated = "ProviderUser.UpdateUser.EmailUpdated";
        }

        public static class VerifyEmailAddress
        {
            public const string Ok = "ProviderUser.VerifyEmailAddress.Ok";
            public const string FailedValidation = "ProviderUser.VerifyEmailAddress.FailedValidation";
            public const string InvalidCode = "ProviderUser.VerifyEmailAddress.InvalidCode";
        }
    }
}