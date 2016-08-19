namespace SFA.Apprenticeships.Infrastructure.Raa.Mappers
{
    using Application.Vacancies.Entities;
    using Common.Mappers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;

    public class VacancySummaryUpdateMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipSummary, ApprenticeshipSummaryUpdate>();
            Mapper.CreateMap<TraineeshipSummary, TraineeshipSummaryUpdate>();
        }
    }
}
