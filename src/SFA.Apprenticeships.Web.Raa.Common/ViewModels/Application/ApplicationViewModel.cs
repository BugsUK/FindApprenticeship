namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Domain.Entities.Applications;

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
    }
}
