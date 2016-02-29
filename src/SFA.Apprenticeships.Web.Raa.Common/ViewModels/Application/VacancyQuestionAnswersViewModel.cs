namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    public class VacancyQuestionAnswersViewModel
    {
        public string FirstQuestionAnswer { get; set; }

        public string SecondQuestionAnswer { get; set; }

        public string AnythingWeCanDoToSupportYourInterviewAnswer { get; set; }

        public bool HasAnythingWeCanDoToSupportYourInterviewAnswer => !string.IsNullOrEmpty(AnythingWeCanDoToSupportYourInterviewAnswer);
    }
}