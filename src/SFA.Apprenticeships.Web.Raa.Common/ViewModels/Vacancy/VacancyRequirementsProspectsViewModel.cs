namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Attributes;
    using Validators.Vacancy;

    [Validator(typeof (VacancyRequirementsProspectsViewModelClientValidator))]
    public class VacancyRequirementsProspectsViewModel : IPartialVacancyViewModel
    {
        public const string PartialView = "Vacancy/RequirementsProspects";

        public int VacancyReferenceNumber { get; set; }

        [AllowHtml]
        [Display(Name = VacancyViewModelMessages.DesiredSkills.LabelText)]
        public string DesiredSkills { get; set; }

        [Display(Name = VacancyViewModelMessages.DesiredSkillsComment.LabelText)]
        public string DesiredSkillsComment { get; set; }

        [AllowHtml]
        [Display(Name = VacancyViewModelMessages.FutureProspects.LabelText)]
        public string FutureProspects { get; set; }

        [Display(Name = VacancyViewModelMessages.FutureProspectsComment.LabelText)]
        public string FutureProspectsComment { get; set; }

        [AllowHtml]
        [Display(Name = VacancyViewModelMessages.PersonalQualities.LabelText)]
        public string PersonalQualities { get; set; }

        [Display(Name = VacancyViewModelMessages.PersonalQualitiesComment.LabelText)]
        public string PersonalQualitiesComment { get; set; }

        [AllowHtml]
        [Display(Name = VacancyViewModelMessages.ThingsToConsider.LabelText)]
        public string ThingsToConsider { get; set; }

        [Display(Name = VacancyViewModelMessages.ThingsToConsiderComment.LabelText)]
        public string ThingsToConsiderComment { get; set; }

        [AllowHtml]
        [Display(Name = VacancyViewModelMessages.DesiredQualifications.LabelText)]
        public string DesiredQualifications { get; set; }

        [Display(Name = VacancyViewModelMessages.DesiredQualificationsComment.LabelText)]
        public string DesiredQualificationsComment { get; set; }

        [AllowHtml]
        [Display(Name = VacancyViewModelMessages.DesiredQualifications.LabelText)]
        public string OtherInformation { get; set; }

        [Display(Name = VacancyViewModelMessages.DesiredQualificationsComment.LabelText)]
        public string OtherInformationComment { get; set; }

        public VacancyStatus Status { get; set; }

        public bool ComeFromPreview { get; set; }

        public VacancyType VacancyType { get; set; }

        public int AutoSaveTimeoutInSeconds { get; set; }

        public VacancySource VacancySource { get; set; }
    }
}