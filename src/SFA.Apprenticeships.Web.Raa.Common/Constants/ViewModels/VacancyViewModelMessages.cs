namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class VacancyViewModelMessages
    {
        public const string NoApplications = "There have not been any applications for this vacancy";
        public const string NoClickThroughs = "There have not been any click-throughs to your application page for this vacancy";

        public static class Title
        {
            public const string LabelText = "Title";
            public const string RequiredErrorText = "Enter a title";
            public const string TooLongErrorText = "The title must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The title " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class TitleComment
        {
            public const string LabelText = "Title comment";
        }

        public static class ShortDescription
        {
            public const string LabelText = "Summary";
            public const string RequiredErrorText = "Enter a vacancy summary";
            public const string TooLongErrorText = "The vacancy summary must not be more than 350 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The vacancy summary " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ShortDescriptionComment
        {
            public const string LabelText = "Summary comment";
        }

        public static class VacancyType
        {
            public const string LabelText = "Vacancy type";
            public const string RequiredErrorText = "Select a vacancy type";
        }

        public static class WorkingWeek
        {
            public const string LabelText = "Working week";
            public const string RequiredErrorText = "Enter the working week";
            public const string TooLongErrorText = "The working week must not be more than 256 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The working week " + Whitelists.FreetextWhitelist.ErrorText;
            public const string TraineeshipLabelText = "Weekly hours";
            public const string TraineeshipRequiredErrorText = "Enter the weekly hours";
            public const string TraineeshipTooLongErrorText = "The weekly hours must not be more than 256 characters";
            public const string TraineeshipWhiteListErrorText = "The weekly hours " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WorkingWeekComment
        {
            public const string LabelText = "Working week/Hours per week comment";
            public const string TraineeshipLabelText = "Weekly hours comment";
        }

        public static class HoursPerWeek
        {
            public const string LabelText = "Hours per week";
            public const string RequiredErrorText = "Enter the hours per week";
            public const string HoursPerWeekShouldBeGreaterThan16 = "The hours per week must be more than 16";
        }

        public static class WageType
        {
            public const string LabelText = "Wage";
            public const string RequiredErrorText = "Select a wage";
        }

        public static class Wage
        {
            public const string LabelText = "Wage";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The wage " + Whitelists.FreetextWhitelist.ErrorText;
            public const string RequiredErrorText = "Enter an amount for wage";
            public const string WageLessThanMinimum = "The wage should not be less than the National Minimum Wage for apprentices";
        }

        public static class WageComment
        {
            public const string LabelText = "Wage comment";
        }

        public static class Duration
        {
            public const string LabelText = "Expected duration";
            public const string WhiteListRegularExpression = Whitelists.IntegerWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The expected duration " + Whitelists.IntegerWhitelist.ErrorText;
            public const string RequiredErrorText = "Enter the expected duration";
            public const string RangeErrorText = "The expected duration must be a whole number";
            public const string DurationCantBeLessThan12Months = "The expected duration must be at least 12 months (52 weeks)";
            public const string DurationMustBeBetweenSixWeeksAndSixMonths = "The expected duration must be between six weeks and six months";
            public const string DurationWarning16Hours =
                "The minimum expected duration is 23 months based on the hours per week selected";
            public const string DurationWarning18Hours =
                "The minimum expected duration is 20 months based on the hours per week selected";
            public const string DurationWarning20Hours =
                "The minimum expected duration is 18 months based on the hours per week selected";
            public const string DurationWarning22Hours =
                "The minimum expected duration is 17 months based on the hours per week selected";
            public const string DurationWarning25Hours =
                "The minimum expected duration is 15 months based on the hours per week selected";
            public const string DurationWarning28Hours =
                "The minimum expected duration is 13 months based on the hours per week selected";
            public const string DurationWarning30Hours =
                "The minimum expected duration is 12 months based on the hours per week selected";
        }

        public static class DurationComment
        {
            public const string LabelText = "Expected duration comment";
        }

        public static class ClosingDate
        {
            public const string LabelText = "Closing date";
            public const string RequiredErrorText = "Enter the closing date";
            public const string AfterTodayErrorText = "The closing date can't be today or earlier. We advise using a date more than two weeks from now";
            public const string TooSoonErrorText = "The closing date should be at least two weeks in the future";
        }

        public static class ClosingDateComment
        {
            public const string LabelText = "Closing date comment";
        }

        public static class PossibleStartDate
        {
            public const string LabelText = "Possible start date";
            public const string RequiredErrorText = "Enter the start date";
            public const string TooSoonErrorText = "The possible start date should be at least two weeks in the future";
            public const string AfterTodayErrorText = "The possible start date can't be today or earlier. We advise using a date more than two weeks from now";
            public const string BeforePublishDateErrorText = "The possible start date should be after the closing date";
        }

        public static class PossibleStartDateComment
        {
            public const string LabelText = "Possible start date comment";
        }

        public static class LongDescription
        {
            public const string LabelText = "Vacancy description";
            public const string RequiredErrorText = "Enter the long description";
            public const string TooLongErrorText = "The long description must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The long description " + Whitelists.FreetextWhitelist.ErrorText;
            public const string TraineeshipLabelText = "Work placement description";
            public const string TraineeshipRequiredErrorText = "Enter the work placement description";
            public const string TraineeshipTooLongErrorText = "The work placement description must not be more than 4000 characters";
            public const string TraineeshipWhiteListErrorText = "The work placement description " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class LongDescriptionComment
        {
            public const string LabelText = "Vacancy description comment";
            public const string TraineeshipLabelText = "Work placement description comment";
        }

        public static class DesiredSkills
        {
            public const string LabelText = "Desired skills";
            public const string RequiredErrorText = "Enter the desired skills";
            public const string TooLongErrorText = "Desired skills must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Desired skills " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class DesiredSkillsComment
        {
            public const string LabelText = "Desired skills comment";
        }

        public static class FutureProspects
        {
            public const string LabelText = "Future prospects";
            public const string RequiredErrorText = "Enter the future prospects";
            public const string TooLongErrorText = "Future prospects must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Future prospects " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FutureProspectsComment
        {
            public const string LabelText = "Future prospects comment";
        }

        public static class PersonalQualities
        {
            public const string LabelText = "Desired personal qualities";
            public const string RequiredErrorText = "Enter the personal qualities required";
            public const string TooLongErrorText = "Personal qualities must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Personal qualities " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class PersonalQualitiesComment
        {
            public const string LabelText = "Personal qualities comment";
        }

        public static class ThingsToConsider
        {
            public const string LabelText = "Things to consider (optional)";
            public const string TooLongErrorText = "Things to consider must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Things to consider " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ThingsToConsiderComment
        {
            public const string LabelText = "Things to consider comment";
        }

        public static class DesiredQualifications
        {
            public const string LabelText = "Desired qualifications";
            public const string RequiredErrorText = "Enter the desired qualifications";
            public const string TooLongErrorText = "Desired qualifications must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Desired qualifications " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class DesiredQualificationsComment
        {
            public const string LabelText = "Desired qualifications comment";
        }

        public static class FirstQuestion
        {
            public const string LabelText = "First question (optional)";
            public const string TooLongErrorText = "The first question must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The first question " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class SecondQuestion
        {
            public const string LabelText = "Second question (optional)";
            public const string TooLongErrorText = "The second question must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The second question " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FirstQuestionComment
        {
            public const string LabelText = "First question comment";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The first question " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class SecondQuestionComment
        {
            public const string LabelText = "Second question comment";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The second question " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class OfflineApplicationUrl
        {
            public const string LabelText = "Enter the web address candidates should use to apply for this vacancy";
            public const string RequiredErrorText = "Enter a valid website address";
            public const string TooLongErrorText = "The website address must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The website address " + Whitelists.FreetextWhitelist.ErrorText;
            public const string ErrorUriText = "Enter a valid website address";

            public const string ShouldBeEmpty =
                "Only enter a web address if the vacancy applications will be managed through an external website";
        }

        public static class OfflineApplicationUrlComment
        {
            public const string LabelText = "Website address comment";
        }

        public static class OfflineApplicationInstructions
        {
            public const string LabelText = "Explain the external website application process (optional)";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The candidate explanation " + Whitelists.FreetextWhitelist.ErrorText;

            public const string ShouldBeEmptyText =
                "Only answer if the vacancy applications will be managed through an external website";
        }

        public static class OfflineApplicationInstructionsComment
        {
            public const string LabelText = "Application process explanation comment";
        }

        public static class ApprenticeshipLevel
        {
            public const string RequiredErrorText = "Select the apprenticeship level";
        }

        public static class ApprenticeshipLevelComment
        {
            public const string LabelText = "Apprenticeship level comment";
        }

        public static class FrameworkCodeName
        {
            public const string RequiredErrorText = "Select the apprenticeship framework";
        }

        public static class FrameworkCodeNameComment
        {
            public const string LabelText = "Apprenticeship framework comment";
        }

        public static class StandardId
        {
            public const string RequiredErrorText = "Select the apprenticeship standard";
        }

        public static class StandardIdComment
        {
            public const string LabelText = "Apprenticeship standard comment";
        }

        public static class SectorCodeName
        {
            public const string RequiredErrorText = "Select the traineeship sector";
        }

        public static class SectorCodeNameComment
        {
            public const string LabelText = "Traineeship sector comment";
        }

        public static class TrainingProvidedMessages
        {
            public const string LabelText = "Training to be provided";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Training to be provided " + Whitelists.FreetextWhitelist.ErrorText;
            public const string RequiredErrorText = "Enter the training to be provided";
        }

        public static class TrainingProvidedComment
        {
            public const string LabelText = "Training to be provided comment";
        }

        public static class ContactNameMessages
        {
            public const string LabelText = "Contact name (optional)";
            public const string TooLongErrorText = "Contact name must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Contact name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ContactNumberMessages
        {
            public const string LabelText = "Contact number (optional)";
            public const string LengthErrorText = "Contact number must be between 8 and 16 digits or not specified";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Contact number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }

        public static class ContactEmailMessages
        {
            public const string LabelText = "Email (optional)";
            public const string TooLongErrorText = "Email address must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Email address " + Whitelists.EmailAddressWhitelist.ErrorText;
        }

        public static class ContactDetailsComment
        {
            public const string LabelText = "Contact details comment";
        }

        public static class Comment
        {
            public const string LabelText = "QA Comment";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Comment " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ResubmitOptin
        {
            public const string LabelText = "I've reviewed this vacancy and would like to resubmit it for approval";
            public const string RequiredErrorText = "Confirm you have reviewed this vacancy and would like to resubmit it for approval";
        }

        public class EmployerDescriptionComment
        {
            public const string LabelText = "Description comment";
        }

        public class EmployerWebsiteUrlComment
        {
            public const string LabelText = "Employer's Website Url comment";
		}
		
        public class OfflineVacancy
        {
            public const string RequiredErrorText = "Select whether the vacancy will be managed through the find an apprentice site or not";
        }
    }
}