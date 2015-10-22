namespace SFA.Apprenticeships.Web.Recruit.Constants.ViewModels
{
    using Common.Constants;

    public class ProviderViewModelMessages
    {
        public static class ProviderNameMessages
        {
            public const string LabelText = "Provider name";
            public const string RequiredErrorText = "Please enter your provider name";
            public const string TooLongErrorText = "Full name mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Provider name " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}