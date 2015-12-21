namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Application
{
    public class VacancyQuestionAnswersViewModel
    {
        public string FirstQuestionAnswer { get; set; }

        public string SecondQuestionAnswer { get; set; }

        public string AnythingWeCanDoToSupportYourInterviewAnswer { get; set; }

        public bool HasAnythingWeCanDoToSupportYourInterviewAnswer => !string.IsNullOrEmpty(AnythingWeCanDoToSupportYourInterviewAnswer);
    }
}