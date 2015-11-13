﻿namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using System;
    using Common.Constants;

    public static class QualificationViewModelMessages
    {
        public static class GradeMessages
        {
            public const string RequiredErrorText = "Please enter grade";
            public const string TooLongErrorText = "Grade must not be more than 15 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Grade " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class QualificationTypeMessages
        {
            public const string RequiredErrorText = "Please enter qualification type";
            public const string TooLongErrorText = "Qualification type must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Qualification type " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class SubjectMessages
        {
            public const string RequiredErrorText = "Please enter subject";
            public const string TooLongErrorText = "Subject must not be more than 50 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Subject " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class YearMessages
        {
            public const string RequiredErrorText = "Please enter year";
            public const string MustBeNumericErrorText = "Year must be 4 digits, eg 1990";
            public const string BeforeOrEqualErrorText = "Please select 'Predicted' if you're adding a grade in the future";
            public static string WhiteListRegularExpression = Whitelists.YearRangeWhiteList.RegularExpression();
            public static string WhiteListErrorText = "Qualification year " + Whitelists.YearRangeWhiteList.ErrorText();
            public const string MustBeNumericRegularExpression = Whitelists.YearWhitelist.RegularExpression;
            public static Func<string, string> MustBeGreaterThan = year => "Year must be 4 digits, and not before " + year;
        }
    }
}