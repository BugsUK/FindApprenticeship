namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Linq;
    using Domain.Entities.Applications;
    using Helpers;
    using ViewModels.Candidate;

    public static class CandidateViewModelResolver
    {
        public static T Resolve<T>(this T candidateViewModel, ApplicationDetail apprenticeshipApplicationDetail) 
            where T : CandidateViewModelBase
        {
            candidateViewModel.Id = apprenticeshipApplicationDetail.CandidateId;
            candidateViewModel.EmailAddress = apprenticeshipApplicationDetail.CandidateDetails.EmailAddress;
            candidateViewModel.FirstName = apprenticeshipApplicationDetail.CandidateDetails.FirstName;
            candidateViewModel.LastName = apprenticeshipApplicationDetail.CandidateDetails.LastName;
            candidateViewModel.DateOfBirth = apprenticeshipApplicationDetail.CandidateDetails.DateOfBirth;
            candidateViewModel.PhoneNumber = apprenticeshipApplicationDetail.CandidateDetails.PhoneNumber;
            candidateViewModel.Address = ApplicationConverter.GetAddressViewModel(apprenticeshipApplicationDetail.CandidateDetails.Address);
            candidateViewModel.Qualifications = ApplicationConverter.GetQualificationsViewModels(apprenticeshipApplicationDetail.CandidateInformation.Qualifications);
            candidateViewModel.HasQualifications = ApplicationConverter.GetQualificationsViewModels(apprenticeshipApplicationDetail.CandidateInformation.Qualifications).Any();
            candidateViewModel.WorkExperience = ApplicationConverter.GetWorkExperiencesViewModels(apprenticeshipApplicationDetail.CandidateInformation.WorkExperience);
            candidateViewModel.HasWorkExperience = ApplicationConverter.GetWorkExperiencesViewModels(apprenticeshipApplicationDetail.CandidateInformation.WorkExperience).Any();
            candidateViewModel.MonitoringInformation = ApplicationConverter.GetMonitoringInformationViewModel(apprenticeshipApplicationDetail.CandidateInformation.AboutYou, apprenticeshipApplicationDetail.CandidateInformation.DisabilityStatus);
            candidateViewModel.EmployerQuestionAnswers = new EmployerQuestionAnswersViewModel
            {
                CandidateAnswer1 = apprenticeshipApplicationDetail.AdditionalQuestion1Answer,
                CandidateAnswer2 = apprenticeshipApplicationDetail.AdditionalQuestion2Answer
            };

            return candidateViewModel;
        }
    }
}