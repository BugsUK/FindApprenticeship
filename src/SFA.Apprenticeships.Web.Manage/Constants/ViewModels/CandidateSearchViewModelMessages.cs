namespace SFA.Apprenticeships.Web.Manage.Constants.ViewModels
{
    using Common.Constants;

    public class CandidateSearchViewModelMessages
    {
        public const string NoSearchCriteriaErrorText =
            "You must enter either a first name, last name, date of birth or postcode";

        public class FirstName
        {
            public const string LabelText = "First name";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public class LastName
        {
            public const string LabelText = "Last name";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public class DateOfBirth
        {
            public const string LabelText = "Date of birth";
            public const string HintText = "Use dd/mm/yyyy format";
            public const string WhiteListRegularExpression = Whitelists.DateWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Date of birth " + Whitelists.DateWhitelist.ErrorText;
        }

        public class Postcode
        {
            public const string LabelText = "Postcode";
            public const string HintText = "Eg CV1 2WT";
            public const string WhiteListRegularExpression = Whitelists.PostcodeWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Postcode" + Whitelists.PostcodeWhitelist.ErrorText;
        }
    }
}