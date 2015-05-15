namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using AutoMapper;
    using Helpers;
    using Domain.Entities.Applications;
    using ViewModels.Applications;
    using ViewModels.Candidate;

    public class TraineeshipApplicationDetailToTraineeshipApplicationViewModelResolver :
        ITypeConverter<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModel Convert(ResolutionContext context)
        {
            var application = (TraineeshipApplicationDetail) context.SourceValue;

            var model = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel().Resolve(application),
                DateUpdated = application.DateUpdated,
                VacancyId = application.Vacancy.Id,
            };

            if (application.DateApplied.HasValue)
            {
                model.DateApplied = application.DateApplied.Value;
            }

            model.Candidate.AboutYou = ApplicationConverter.GetAboutYouViewModel(application.CandidateInformation.AboutYou);
            model.Candidate.MonitoringInformation = ApplicationConverter.GetMonitoringInformationViewModel(application.CandidateInformation.AboutYou, application.CandidateInformation.DisabilityStatus);

            return model;
        }
    }
}