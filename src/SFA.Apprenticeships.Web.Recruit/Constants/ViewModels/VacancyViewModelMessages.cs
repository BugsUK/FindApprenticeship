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
        }

        public static class WeeklyWage
        {
            public const string LabelText = "Weekly wage (£)";
        }

        public static class Duration
        {
            public const string LabelText = "Duration (years)";
        }

        public static class PublishDate
        {
            public const string LabelText = "Publish date";
        }

        public static class ClosingDate
        {
            public const string LabelText = "Closing date";
        }

        public static class PossibleStartDate
        {
            public const string LabelText = "Possible start date";
        }

        public static class LongDescription
        {
            public const string LabelText = "Long description";
        }

        public static class DesiredSkills
        {
            public const string LabelText = "Desired skills";
        }

        public static class FutureProspects
        {
            public const string LabelText = "Future prospects";
        }

        public static class PersonalQualities
        {
            public const string LabelText = "Personal qualities";
        }

        public static class ThingsToConsider
        {
            public const string LabelText = "Things to consider";
        }

        public static class DesiredQualifications
        {
            public const string LabelText = "Desired qualifications";
        }

        public static class FirstQuestion
        {
            public const string LabelText = "First question (optional)";
        }

        public static class SecondQuestion
        {
            public const string LabelText = "Second question (optional)";
        }
    }
}