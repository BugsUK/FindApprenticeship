namespace SFA.Apprenticeships.Web.Common.Constants.ViewModels
{
    public static class WageViewModelMessages
    {
        public const string LabelText = "Wage";
        public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
        public const string WhiteListErrorText = "The wage " + Whitelists.FreetextWhitelist.ErrorText;
        public const string RequiredErrorText = "Enter an amount for wage";
        public const string WageLessThanMinimum = "The wage should not be less than the National Minimum Wage for apprentices";
    }
}
