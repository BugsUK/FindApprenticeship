namespace SFA.Apprenticeships.Web.Candidate.Mediators.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Providers;
    using SFA.Infrastructure.Interfaces;
    using Search;
    using ViewModels.Candidate;

    public abstract class ApplicationMediatorBase : SearchMediatorBase
    {
        protected ApplicationMediatorBase(IConfigurationService configService, IUserDataProvider userDataProvider)
            : base(configService, userDataProvider)
        {
        }

        protected static IEnumerable<WorkExperienceViewModel> RemoveEmptyRowsFromWorkExperience(
            IEnumerable<WorkExperienceViewModel> workExperience)
        {
            if (workExperience == null)
            {
                return new List<WorkExperienceViewModel>();
            }

            return workExperience.Where(vm =>
                IsTrimmedNullOrWhitespace(vm.Employer) ||
                IsTrimmedNullOrWhitespace(vm.JobTitle) ||
                IsTrimmedNullOrWhitespace(vm.Description) ||
                IsTrimmedNullOrWhitespace(vm.FromYear) ||
                IsTrimmedNullOrWhitespace(vm.ToYear));
        }

        protected static IEnumerable<TrainingCourseViewModel> RemoveEmptyRowsFromTrainingCourses(
            IEnumerable<TrainingCourseViewModel> trainingCourseViewModels)
        {
            if (trainingCourseViewModels == null)
            {
                return new List<TrainingCourseViewModel>();
            }

            return trainingCourseViewModels.Where(vm =>
                IsTrimmedNullOrWhitespace(vm.Provider) ||
                IsTrimmedNullOrWhitespace(vm.Title) ||
                IsTrimmedNullOrWhitespace(vm.FromYear) ||
                IsTrimmedNullOrWhitespace(vm.ToYear));
        }

        protected static IEnumerable<QualificationsViewModel> RemoveEmptyRowsFromQualifications(
            IEnumerable<QualificationsViewModel> qualifications)
        {
            if (qualifications == null)
            {
                return new List<QualificationsViewModel>();
            }

            return qualifications.Where(vm =>
                IsTrimmedNullOrWhitespace(vm.Subject) ||
                IsTrimmedNullOrWhitespace(vm.QualificationType) ||
                IsTrimmedNullOrWhitespace(vm.Grade) ||
                IsTrimmedNullOrWhitespace(vm.Year));
        }

        #region Helpers

        private static bool IsTrimmedNullOrWhitespace(string s)
        {
            return s != null && !string.IsNullOrWhiteSpace(s.Trim());
        }

        #endregion
    }
}