using SFA.Apprenticeships.Web.Common.Constants;

namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    public class StandardViewModelMessages
    {
        public const string StandardSavedSuccessfully = "The changes to the provider were saved successfully";
        public const string StandardSaveError = "An error occured when saving the standard. Please check your entries and try again";

        public static class Name
        {
            public const string LabelText = "Name";
            public const string RequiredErrorText = "Enter the standard name";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ApprenticeshipSectorId
        {
            public const string LabelText = "Sector";
        }

        public static class ApprenticeshipLevel
        {
            public const string LabelText = "Apprenticeship Level";
        }
    }
}