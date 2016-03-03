namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    public class ApplicationVacancyViewModel
    {
        public int VacancyId { get; set; }
        public int VacancyReferenceNumber { get; set; }
        public string Title { get; set; }
        public string EmployerName { get; set; }
        public string ShortDescription { get; set; }
        public string FirstQuestion { get; set; }
        public string SecondQuestion { get; set; }

        public bool HasQuestions => HasFirstQuestion && HasSecondQuestion;
        public bool HasFirstQuestion => !string.IsNullOrEmpty(FirstQuestion);
        public bool HasSecondQuestion => !string.IsNullOrEmpty(SecondQuestion);
    }
}