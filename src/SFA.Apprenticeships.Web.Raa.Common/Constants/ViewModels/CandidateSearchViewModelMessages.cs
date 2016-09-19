namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class CandidateSearchViewModelMessages
    {
        public const string NoSearchCriteriaErrorText =
            "You must enter at least one search criteria";

        public class ApplicantId
        {
            public const string LabelText = "Applicant ID (optional)";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Applicant ID " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public class FirstName
        {
            public const string LabelText = "First name (optional)";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First name " + Whitelists.NameWhitelist.ErrorText;
        }

        public class LastName
        {
            public const string LabelText = "Last name (optional)";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First name " + Whitelists.NameWhitelist.ErrorText;
        }

        public class DateOfBirth
        {
            public const string LabelText = "Date of birth (optional)";
            public const string HintText = "Use dd/mm/yyyy format";
            public const string WhiteListRegularExpression = Whitelists.DateWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Use the dd/mm/yyy format for date of birth";
        }

        public class Postcode
        {
            public const string LabelText = "Postcode (optional)";
            public const string HintText = "Eg CV1 2WT";
            public const string WhiteListRegularExpression = Whitelists.PostcodeWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Enter a valid postcode eg CV1 2WT";
        }
    }
}