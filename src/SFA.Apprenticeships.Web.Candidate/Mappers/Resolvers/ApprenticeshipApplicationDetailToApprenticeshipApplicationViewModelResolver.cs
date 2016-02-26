namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Common.ViewModels.Applications;
    using Common.ViewModels.Candidate;
    using Domain.Entities.Applications;
    using Helpers;

    public class ApprenticeshipApplicationDetailToApprenticeshipApplicationViewModelResolver :
        ITypeConverter<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModel Convert(ResolutionContext context)
        {
            var application = (ApprenticeshipApplicationDetail) context.SourceValue;

            var model = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel().Resolve(application),
                DateUpdated = application.DateUpdated,
                VacancyId = application.Vacancy.Id,
                Status = application.Status,
                DateApplied = application.DateApplied ?? DateTime.UtcNow
            };

            model.Candidate.AboutYou = ApplicationConverter.GetAboutYouViewModel(application.CandidateInformation.AboutYou);
            model.Candidate.Education = ApplicationConverter.GetEducationViewModel(application.CandidateInformation.EducationHistory);
            model.Candidate.MonitoringInformation = ApplicationConverter.GetMonitoringInformationViewModel(application.CandidateInformation.AboutYou, application.CandidateInformation.DisabilityStatus);

            return model;
        }
    }
}