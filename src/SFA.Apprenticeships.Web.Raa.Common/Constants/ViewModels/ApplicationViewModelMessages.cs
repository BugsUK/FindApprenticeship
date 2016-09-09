namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class ApplicationViewModelMessages
    {
        public const string UpdateNotesFailed = "An error occured when trying to save. Please try again later";
        public const string SuccessfulDecisionFormat = "{0}'s application has been successful";
        public const string UnsuccessfulDecisionFormat = "{0}'s application has been unsuccessful";
        public const string RevertToViewedFormat = "{0}'s application has been reverted to the viewed state";

        public class Notes
        {
            public const string LabelText = "Vacancy manager notes (optional)";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The vacancy manager notes " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}