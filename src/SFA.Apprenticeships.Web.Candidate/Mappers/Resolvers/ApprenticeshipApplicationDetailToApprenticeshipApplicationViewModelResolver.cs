namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Helpers;
    using ViewModels.Applications;
    using ViewModels.Candidate;

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
                DateApplied = application.DateApplied ?? DateTime.Now
            };

            model.Candidate.AboutYou = ApplicationConverter.GetAboutYouViewModel(application.CandidateInformation.AboutYou);
            model.Candidate.Education = ApplicationConverter.GetEducationViewModel(application.CandidateInformation.EducationHistory);
            model.Candidate.MonitoringInformation = ApplicationConverter.GetMonitoringInformationViewModel(application.CandidateInformation.AboutYou, application.CandidateInformation.MonitoringInformation);

            return model;
        }
    }
}