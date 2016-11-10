using SFA.Apprenticeships.Web.Common.Constants;

namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    public class SectorViewModelMessages
    {
        public const string SectorSavedSuccessfully = "The changes to the provider were saved successfully";
        public const string SectorSaveError = "An error occured when saving the standard. Please check your entries and try again";

        public static class Name
        {
            public const string LabelText = "Name";
            public const string RequiredErrorText = "Enter the sector name";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ApprenticeshipOccupationId
        {
            public const string LabelText = "SSAT1";
        }
    }
}