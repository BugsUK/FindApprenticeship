namespace SFA.Apprenticeships.Web.Recruit.Constants.ViewModels
{
    public class EmployerSearchViewModelMessages
    {
        public const string NameAndLocationSearchRequiredErrorText = "Please enter an employer name, a town or postcode or both to search";

        public static class Ern
        {
            public const string LabelText = "Search using Employer Reference Number (ERN)";
            public const string RequiredErrorText = "Please enter an Employer Reference Number (ERN)";
        }

        public static class Name
        {
            public const string LabelText = "Employer name";
        }

        public static class Location
        {
            public const string LabelText = "Employer location";
            public const string HintText = "Enter town or postcode";
        }
    }
}