namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.Mappers
{
    using Application.Vacancies.Entities;
    using Common.Mappers;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Traineeships;

    public class VacancyEtlMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipSummary, ApprenticeshipSummaryUpdate>();
            Mapper.CreateMap<TraineeshipSummary, TraineeshipSummaryUpdate>();
            
            Mapper.CreateMap<ApprenticeshipApplicationSummary, ExpiringApprenticeshipApplicationDraft>()
                .ForMember(output => output.EntityId, map => map.MapFrom(input => input.ApplicationId))
                .ForMember(output => output.VacancyId, map => map.MapFrom(input => input.LegacyVacancyId));
        }
    }
}
