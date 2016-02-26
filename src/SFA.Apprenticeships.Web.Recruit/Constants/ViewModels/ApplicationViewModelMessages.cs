namespace SFA.Apprenticeships.Web.Recruit.Constants.ViewModels
{
    using Common.Constants;

    public class ApplicationViewModelMessages
    {
        public const string UpdateNotesFailed = "An error occured when trying to save. Please try again later";
        public const string SuccessfulApplicationFormat = "{0}'s application has been successful";

        public class Notes
        {
            public const string LabelText = "Vacancy manager notes (optional)";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The vacancy manager notes " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}