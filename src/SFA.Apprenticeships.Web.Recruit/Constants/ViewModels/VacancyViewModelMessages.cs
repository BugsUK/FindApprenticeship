namespace SFA.Apprenticeships.Web.Recruit.Constants.ViewModels
{
    using Common.Constants;

    public class VacancyViewModelMessages
    {
        public static class Title
        {
            public const string LabelText = "Title";
            public const string RequiredErrorText = "Please enter a title";
            public const string TooLongErrorText = "Title mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Title " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ShortDescription
        {
            public const string LabelText = "Summary";
            public const string RequiredErrorText = "Please enter a vacancy summary";
            public const string TooLongErrorText = "Short description mustn’t exceed 512 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Short description " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WorkingWeek
        {
            public const string LabelText = "Working week";
            public const string RequiredErrorText = "Please enter the working week";
            public const string TooLongErrorText = "Working week mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Working week " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class HoursPerWeek
        {
            public const string LabelText = "hours per week";
            public const string RequiredErrorText = "Please enter the hours per week";
            public const string HoursPerWeekShouldBeBetween16And40 = "TODO: hours per week must be greater >= 16 and <= 40";
        }

        public static class WageType
        {
            public const string LabelText = "Wage";
            public const string RequiredErrorText = "Please select a wage";
        }

        public static class Wage
        {
            public const string LabelText = "Wage";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Working week " + Whitelists.FreetextWhitelist.ErrorText;
            public const string RequiredErrorText = "Please enter an amount for wage";
            public const string WageLessThanMinimum =
                "TODO: wage should not be less than the Apprenticeship Minimum Wage.";
        }

        public static class Duration
        {
            public const string LabelText = "Duration";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Working week " + Whitelists.FreetextWhitelist.ErrorText;
            public const string RequiredErrorText = "Please enter the duration";
            public const string DurationCantBeLessThan12Months = "TODO: duration can't be less than 12 months";
        }

        public static class ClosingDate
        {
            public const string LabelText = "Closing date";
            public const string RequiredErrorText = "Please specify the closing date";
            public const string TooSoonErrorText = "Closing date must be in the future";
            public const string BeforePublishDateErrorText = "Closing date must be after the Publish date";
        }

        public static class PossibleStartDate
        {
            public const string LabelText = "Possible start date";
            public const string RequiredErrorText = "Please specify the possible start date";
            public const string TooSoonErrorText = "Possible start date must be in the future";
            public const string BeforePublishDateErrorText = "Possible start date must be after the Publish date";
        }

        public static class LongDescription
        {
            public const string LabelText = "Vacancy description";
            public const string RequiredErrorText = "Please provide a vacancy description";
            public const string TooLongErrorText = "Long description mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Long description " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class DesiredSkills
        {
            public const string LabelText = "Desired skills";
            public const string RequiredErrorText = "Please provide the desired skills";
            public const string TooLongErrorText = "Desired skills mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Desired skills " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FutureProspects
        {
            public const string LabelText = "Future prospects";
            public const string RequiredErrorText = "Please provide the future prospects";
            public const string TooLongErrorText = "Future prospects mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Future prospects " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class PersonalQualities
        {
            public const string LabelText = "Personal qualities";
            public const string RequiredErrorText = "Please provide the personal qualities required";
            public const string TooLongErrorText = "Personal qualities mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Personal qualities " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ThingsToConsider
        {
            public const string LabelText = "Things to consider (optional)";
            public const string TooLongErrorText = "Things to consider mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Things to consider " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class DesiredQualifications
        {
            public const string LabelText = "Desired qualifications";
            public const string RequiredErrorText = "Please provide the desired qualifications";
            public const string TooLongErrorText = "Desired qualifications mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Desired qualifications " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FirstQuestion
        {
            public const string LabelText = "First question (optional)";
            public const string TooLongErrorText = "First question mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First question " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class SecondQuestion
        {
            public const string LabelText = "Second question (optional)";
            public const string TooLongErrorText = "Second question mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Second question " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class OfflineApplicationUrl
        {
            public const string LabelText = "What's the website address that candidate should apply through?";
            public const string RequiredErrorText = "TODO: pleae provide an application URL";
            public const string TooLongErrorText = "Website address mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Website address " + Whitelists.FreetextWhitelist.ErrorText;
            public const string ErrorUriText = "Please enter a valid website url";
        }

        public static class OfflineApplicationInstructions
        {
            public const string LabelText = "Please explain to the candidate what will happen when they apply (optional)";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Candidate explanation " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ApprenticeshipLevel
        {
            public const string RequiredErrorText = "Please select the apprenticeship level";
        }

        public static class FrameworkCodeName
        {
            public const string RequiredErrorText = "Please select the apprenticeship framework";
        }
    }
}