namespace SFA.Apprenticeships.Web.Recruit.Constants.ViewModels
{
    using Common.Constants;

    public class VacancyViewModelMessages
    {
        public static class Title
        {
            public const string LabelText = "Title";
            public const string RequiredErrorText = "Please enter a Title";
            public const string TooLongErrorText = "Title mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Title " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ShortDescription
        {
            public const string LabelText = "Short description";
            public const string RequiredErrorText = "Please provide a Short description";
            public const string TooLongErrorText = "Short description mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Short description " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WorkingWeek
        {
            public const string LabelText = "Working week";
            public const string RequiredErrorText = "Please enter the Working week";
            public const string TooLongErrorText = "Working week mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Working week " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WeeklyWage
        {
            public const string LabelText = "Weekly wage (£)";
            public const string RequiredErrorText = "Please enter an amount for Weekly wage";
        }

        public static class Duration
        {
            public const string LabelText = "Duration (years)";
            public const string RequiredErrorText = "Please enter the Duration";
        }

        public static class PublishDate
        {
            public const string LabelText = "Publish date";
            public const string RequiredErrorText = "Please specify the Publish date";
            public const string TooSoonErrorText = "Publish date must be in the future";
        }

        public static class ClosingDate
        {
            public const string LabelText = "Closing date";
            public const string RequiredErrorText = "Please specify the Closing date";
            public const string TooSoonErrorText = "Closing date must be in the future";
            public const string BeforePublishDateErrorText = "Closing date must be after the Publish date";
        }

        public static class PossibleStartDate
        {
            public const string LabelText = "Possible start date";
            public const string RequiredErrorText = "Please specify the Possible start date";
            public const string TooSoonErrorText = "Possible start date must be in the future";
            public const string BeforePublishDateErrorText = "Possible start date must be after the Publish date";
        }

        public static class LongDescription
        {
            public const string LabelText = "Long description";
            public const string RequiredErrorText = "Please provide a Long description";
            public const string TooLongErrorText = "Long description mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Long description " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class DesiredSkills
        {
            public const string LabelText = "Desired skills";
            public const string RequiredErrorText = "Please provide the Desired skills";
            public const string TooLongErrorText = "Desired skills mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Desired skills " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FutureProspects
        {
            public const string LabelText = "Future prospects";
            public const string RequiredErrorText = "Please provide the Future prospects";
            public const string TooLongErrorText = "Future prospects mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Future prospects " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class PersonalQualities
        {
            public const string LabelText = "Personal qualities";
            public const string RequiredErrorText = "Please provide the Personal qualities required";
            public const string TooLongErrorText = "Personal qualities mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Personal qualities " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ThingsToConsider
        {
            public const string LabelText = "Things to consider";
            public const string RequiredErrorText = "Please provide a description of Things to consider";
            public const string TooLongErrorText = "Things to consider mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Things to consider " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class DesiredQualifications
        {
            public const string LabelText = "Desired qualifications";
            public const string RequiredErrorText = "Please provide the Desired qualifications";
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
    }
}