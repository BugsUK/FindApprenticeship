namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    public class NewVacancyViewModelMessages
    {
        public const string MultiOfflineUrlsButtonText = "enter a different web address for each vacancy location";
        public const string SingleOfflineUrlButtonText = "use the same web address for all vacancy locations";

        public static class ApprenticeshipLevel
        {
            public const string RequiredErrorText = "Select an apprenticeship level";
        }

        public static class TrainingType
        {
            public const string RequiredErrorText = "Select an apprenticeship type";
        }

        public class FrameworkCodeName
        {
            public const string RequiredErrorText = "Select a framework";
        }

        public static class OptionalQuestions
        {
            public const string WontBeVisible = "Optional questions will not appear on offline vacancies";
        }
    }
}
