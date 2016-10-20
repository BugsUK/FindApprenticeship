namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class ApplicationViewModelMessages
    {
        public const string UpdateNotesFailed = "An error occured when trying to save. Please try again later";
        public const string SuccessfulDecisionFormat = "'Successful candidate' message has been sent to ";
        public const string UnsuccessfulDecisionFormat = "An 'unsuccessful application' message has been sent to ";
        public const string BulkUnsuccessfulDecisionFormat = "'Unsuccessful candidate' message has been sent to ";
        public const string RevertToInProgressFormat = "{0}'s application has been reverted to the in progress state";

        public class Notes
        {
            public const string LabelText = "This is for reference and not shown to candidates.";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The vacancy manager notes " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public class UnSuccessfulReason
        {
            public const string RequiredErrorText = "You must provide feedback explaining why the candidate\'s application was not successful";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The candidate application feedback " + Whitelists.FreetextWhitelist.ErrorText;
            public const string LabelText = " ";
            public const string BulkRequiredErrorText =
                "Please provide feedback to the candidates for why they were unsuccessful";
        }
    }
}