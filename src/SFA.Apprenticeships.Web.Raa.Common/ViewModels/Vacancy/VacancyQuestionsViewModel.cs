namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Attributes;
    using Validators.Vacancy;

    [Validator(typeof (VacancyQuestionsViewModelClientValidator))]
    public class VacancyQuestionsViewModel : IPartialVacancyViewModel
    {
        public const string PartialView = "Vacancy/EmployerQuestions";

        public int VacancyReferenceNumber { get; set; }

        [AllowHtml]
        [Display(Name = VacancyViewModelMessages.FirstQuestion.LabelText)]
        public string FirstQuestion { get; set; }

        [AllowHtml]
        [Display(Name = VacancyViewModelMessages.SecondQuestion.LabelText)]
        public string SecondQuestion { get; set; }

        [Display(Name = VacancyViewModelMessages.FirstQuestionComment.LabelText)]
        public string FirstQuestionComment { get; set; }

        [Display(Name = VacancyViewModelMessages.SecondQuestionComment.LabelText)]
        public string SecondQuestionComment { get; set; }

        public bool ComeFromPreview { get; set; }

        public VacancyStatus Status { get; set; }

        public VacancyType VacancyType { get; set; }

        public int AutoSaveTimeoutInSeconds { get; set; }
    }
}