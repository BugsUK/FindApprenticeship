namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Application
{
    using System.Collections.Generic;

    public class ApplicationViewModel
    {
        public ApplicationVacancyViewModel Vacancy { get; set; }

        public ApplicantDetailsViewModel ApplicantDetails { get; set; }

        public AboutYouViewModel AboutYou { get; set; }

        public EducationViewModel Education { get; set; }

        public IList<QualificationViewModel> Qualifications { get; set; }

        public IList<WorkExperienceViewModel> WorkExperience { get; set; }

        public IList<TrainingCourseViewModel> TrainingCourses { get; set; }

        public VacancyQuestionAnswersViewModel VacancyQuestionAnswers { get; set; }
    }
}