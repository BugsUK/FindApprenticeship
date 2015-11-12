namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Vacancy;

    [Validator(typeof(VacancyQuestionsViewModelClientValidator))]
    public class VacancyQuestionsViewModel
    {
        public long VacancyReferenceNumber { get; set; }
        [Display(Name = VacancyViewModelMessages.FirstQuestion.LabelText)]
        public string FirstQuestion { get; set; }
        [Display(Name = VacancyViewModelMessages.SecondQuestion.LabelText)]
        public string SecondQuestion { get; set; }
        [Display(Name = VacancyViewModelMessages.Comment.LabelText)]
        public string FirstQuestionComment { get; set; }
        [Display(Name = VacancyViewModelMessages.Comment.LabelText)]
        public string SecondQuestionComment { get; set; }
    }
}