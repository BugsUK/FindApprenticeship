namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Linq;
    using Common.ViewModels.Candidate;
    using Domain.Entities.Applications;
    using Helpers;

    public static class CandidateViewModelResolver
    {
        public static T Resolve<T>(this T candidateViewModel, ApplicationDetail applicationDetail) 
            where T : CandidateViewModelBase
        {
            candidateViewModel.Id = applicationDetail.CandidateId;
            candidateViewModel.EmailAddress = applicationDetail.CandidateDetails.EmailAddress;
            candidateViewModel.FirstName = applicationDetail.CandidateDetails.FirstName;
            candidateViewModel.LastName = applicationDetail.CandidateDetails.LastName;
            candidateViewModel.DateOfBirth = applicationDetail.CandidateDetails.DateOfBirth;
            candidateViewModel.PhoneNumber = applicationDetail.CandidateDetails.PhoneNumber;
            candidateViewModel.Address = ApplicationConverter.GetAddressViewModel(applicationDetail.CandidateDetails.Address);

            candidateViewModel.Qualifications = ApplicationConverter.GetQualificationsViewModels(applicationDetail.CandidateInformation.Qualifications);
            candidateViewModel.HasQualifications = candidateViewModel.Qualifications.Any();
            
            candidateViewModel.WorkExperience = ApplicationConverter.GetWorkExperiencesViewModels(applicationDetail.CandidateInformation.WorkExperience);
            candidateViewModel.HasWorkExperience = candidateViewModel.WorkExperience.Any();
            
            candidateViewModel.TrainingCourses = ApplicationConverter.GetTrainingCourseViewModels(applicationDetail.CandidateInformation.TrainingCourses);
            candidateViewModel.HasTrainingCourses = candidateViewModel.TrainingCourses.Any();

            candidateViewModel.MonitoringInformation = ApplicationConverter.GetMonitoringInformationViewModel(applicationDetail.CandidateInformation.AboutYou, applicationDetail.CandidateInformation.DisabilityStatus);
            candidateViewModel.EmployerQuestionAnswers = new EmployerQuestionAnswersViewModel
            {
                CandidateAnswer1 = applicationDetail.AdditionalQuestion1Answer,
                CandidateAnswer2 = applicationDetail.AdditionalQuestion2Answer
            };

            return candidateViewModel;
        }
    }
}