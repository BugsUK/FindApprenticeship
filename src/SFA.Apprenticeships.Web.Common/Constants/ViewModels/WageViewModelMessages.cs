namespace SFA.Apprenticeships.Web.Common.Constants.ViewModels
{
    public static class WageViewModelMessages
    {
        public const string LabelText = "Wage";
        public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
        public const string WhiteListErrorText = "The wage " + Whitelists.FreetextWhitelist.ErrorText;
        public const string RequiredErrorText = "Enter an amount for wage";
        public const string WageLessThanMinimum = "The wage should not be less than the National Minimum Wage for apprentices";

        public static class HoursPerWeek
        {
            public const string LabelText = "Paid hours per week";
            public const string RequiredErrorText = "Enter the paid hours per week";
            public const string HoursPerWeekShouldBeGreaterThan16 = "The paid hours per week must be more than 16";
        }

        public static class WageTypeReason
        {
            public const string LabelText = "Explain why you need to use a text description";
            public const string TooLongErrorText = "The reason must not be more than 240 characters";

            public const string HelpText =
                "This explanation is for our helpdesk advisers and will not be shown on the live vacancy";
        }

        public static class AmountMessages
        {
            public const string HelpText = "The wage should not be less then the National Minimum Wage for apprentices";
        }

        public static class AmountRangeMessages
        {
            public const string HelpText = "The lowest figure in the wage range should not be less then the National Minimum Wage for apprentices";
        }
    }
}
