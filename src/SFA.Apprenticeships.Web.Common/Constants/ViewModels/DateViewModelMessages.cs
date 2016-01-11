namespace SFA.Apprenticeships.Web.Common.Constants.ViewModels
{
    public static class DateViewModelMessages
    {
        public const string MustBeValidDate = "Enter a valid date";

        public static class DayMessages
        {
            public const string LabelText = "Day";
            public const string RequiredErrorText = "Enter the day";
            public const string RangeErrorText = "Enter a number between 1 and 31";
        }

        public static class MonthMessages
        {
            public const string LabelText = "Month";
            public const string RequiredErrorText = "Enter the month";
            public const string RangeErrorText = "Enter a number between 1 and 12";
        }

        public static class YearMessages
        {
            public const string LabelText = "Year";
            public const string RequiredErrorText = "Enter the year";
            public const string RangeErrorText = "Year must be 4 digits, between {0} and {1}";
        }
    }
}