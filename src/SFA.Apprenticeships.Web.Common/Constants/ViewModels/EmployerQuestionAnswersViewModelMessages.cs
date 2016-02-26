namespace SFA.Apprenticeships.Web.Common.Constants.ViewModels
{
    using Constants;

    public static class EmployerQuestionAnswersViewModelMessages
    {
        public static class CandidateAnswer1Messages
        {
            public const string RequiredErrorText = "Please provide an answer to the additional question(s)";
            public const string TooLongErrorText = "Your answer must not be more than 4000 characters";
            public const string WhitelistRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhitelistErrorText = "Your answer " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class CandidateAnswer2Messages
        {
            public const string RequiredErrorText = "Please provide an answer to the additional question(s)";
            public const string TooLongErrorText = "Your answer must not be more than 4000 characters";
            public const string WhitelistRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhitelistErrorText = "Your answer " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}