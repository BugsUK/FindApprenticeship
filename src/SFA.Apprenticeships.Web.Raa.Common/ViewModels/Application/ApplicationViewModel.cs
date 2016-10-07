namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using Constants.ViewModels;
    using Domain.Entities.Applications;
    using FluentValidation.Attributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validators.Application;

    [Validator(typeof(ApprenticeshipApplicationViewModelClientValidator))]
    public class ApplicationViewModel
    {
        public ApplicationStatuses Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateApplied { get; set; }

        public DateTime? SuccessfulDateTime { get; set; }

        public DateTime? UnsuccessfulDateTime { get; set; }

        public ApplicationSelectionViewModel ApplicationSelection { get; set; }

        public ApplicationVacancyViewModel Vacancy { get; set; }

        public ApplicantDetailsViewModel ApplicantDetails { get; set; }

        public AnonymisedApplicantDetailsViewModel AnonymousApplicantDetails { get; set; }

        public AboutYouViewModel AboutYou { get; set; }

        public EducationViewModel Education { get; set; }

        public IList<QualificationViewModel> Qualifications { get; set; }

        public IList<WorkExperienceViewModel> WorkExperience { get; set; }

        public IList<TrainingCourseViewModel> TrainingCourses { get; set; }

        public VacancyQuestionAnswersViewModel VacancyQuestionAnswers { get; set; }

        [Display(Name = ApplicationViewModelMessages.Notes.LabelText)]
        public string Notes { get; set; }

        [Display(Name = ApplicationViewModelMessages.CandidateApplicationFeedback.LabelText)]
        public string CandidateApplicationFeedback { get; set; }
    }
}
