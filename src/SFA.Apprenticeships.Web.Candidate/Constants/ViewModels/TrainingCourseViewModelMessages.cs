﻿namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using System;
    using Common.Constants;

    public static class TrainingCourseViewModelMessages
    {
        public static class ProviderMessages
        {
            public const string RequiredErrorText = "Please enter name of provider";
            public const string TooLongErrorText = "Provider name mustn’t exceed 50 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Provider name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FromYearMessages
        {
            public const string BeforeOrEqualErrorText = "Year started can’t be after year finished";
            public const string RequiredErrorText = "Please enter year started";
            public const string MustBeNumericText = "Please enter year started";
            public const string CanNotBeInTheFutureErrorText = "Year started can’t be in the future";
            public const string WhiteListRegularExpression = Whitelists.YearWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Year started " + Whitelists.YearWhitelist.ErrorText; 
            public static Func<string, string> MustBeGreaterThan = year => "Year must be 4 digits, and not before " + year;
        }

        public static class TitleMessages
        {
            public const string RequiredErrorText = "Please enter course title";
            public const string TooLongErrorText = "Course title mustn’t exceed 50 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Course title " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ToYearMessages
        {
            public const string MustBeNumericText = "Year finished must be a number";
            public const string WhiteListRegularExpression = Whitelists.YearWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Year finished " + Whitelists.YearWhitelist.ErrorText;
            public static Func<string, string> MustBeGreaterThan = year => "Year must be 4 digits, and not before " + year;
            public static string CanNotBeInTheFutureErrorText = "Year finished can’t be in the future";
        }
    }
}