namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    public class ReportParametersMessages
    {
        public static class FromDateMessages
        {
            public const string LabelText = "Date from";
            public const string RequiredErrorText = "Date from is required";
            public const string InvalidDateErrorText = "Date from must be a valid date";
        }

        public static class ToDateMessages
        {
            public const string LabelText = "Date to";
            public const string RequiredErrorText = "Date to is required";
            public const string InvalidDateErrorText = "Date to must be a valid date";
        }

        public static class AgeRange
        {
            public const string LabelText = "Age range";
        }

        public static class LocalAuthority
        {
            public const string LabelText = "Local authority";
        }

        public static class MarketMessagesOnly
        {
            public const string LabelText = "Only Opted-in Candidates for Marketing Messages";
        }

        public static class IncludeCandidateIds
        {
            public const string LabelText = "Include Candidate IDs in report";
        }
    }
}