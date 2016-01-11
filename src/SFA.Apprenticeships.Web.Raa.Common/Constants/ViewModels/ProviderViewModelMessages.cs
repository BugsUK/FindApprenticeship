using SFA.Apprenticeships.Web.Common.Constants;

namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    public class ProviderViewModelMessages
    {
        public static class ProviderNameMessages
        {
            public const string LabelText = "Provider name";
            public const string RequiredErrorText = "Enter your provider name";
            public const string TooLongErrorText = "Full name must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Provider name " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}