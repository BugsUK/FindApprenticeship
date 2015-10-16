namespace SFA.Apprenticeships.Web.Recruit.Constants.ViewModels
{
    public class EmployerSearchViewModelMessages
    {
        public const string NameAndLocationSearchRequiredErrorText = "Please enter an employer name, a town or postcode or both to search";
        //TODO: Bluesheep link?
        public const string NoResultsText = "We could not find any employers that match your search criteria. In order to process this vacancy posting you’ll need to have an Employer Reference Number (ERN) created for your chosen employer by the <a href=\"http://edrs.lsc.gov.uk/search/lsc/\">Blue Sheep/EDRS service</a>";

        public static class Ern
        {
            public const string LabelText = "Search using Employer Reference Number (ERN)";
            public const string RequiredErrorText = "Please enter an Employer Reference Number (ERN)";
            public const string WhiteListRegularExpression = @"^\d{6}(\d{3})?$";
            public const string WhiteListErrorText = "Employer Reference Number (ERN) must be a number with between six and nine digits";
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