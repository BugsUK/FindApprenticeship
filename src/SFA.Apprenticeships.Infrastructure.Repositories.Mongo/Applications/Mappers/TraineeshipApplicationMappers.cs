﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Applications.Mappers
{
    using Domain.Entities.Applications;
    using Infrastructure.Common.Mappers;
    using Entities;

    public class TraineeshipApplicationMappers : MapperEngine
    {
        public override void Initialise()
        {
            InitialiseApplicationDetailMappers();
            InitialiseApplicationSummaryMappers();
        }

        private void InitialiseApplicationDetailMappers()
        {
            Mapper.CreateMap<TraineeshipApplicationDetail, MongoTraineeshipApplicationDetail>();
            Mapper.CreateMap<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>();
        }

        private void InitialiseApplicationSummaryMappers()
        {
            Mapper.CreateMap<MongoTraineeshipApplicationDetail, TraineeshipApplicationSummary>()
                .ForMember(x => x.ApplicationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.LegacyApplicationId, opt => opt.MapFrom(src => src.LegacyApplicationId))
                .ForMember(x => x.LegacyVacancyId, opt => opt.MapFrom(src => src.Vacancy.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Vacancy.Title))
                .ForMember(x => x.EmployerName, opt => opt.MapFrom(src => src.Vacancy.EmployerName))
                .ForMember(x => x.IsPositiveAboutDisability, opt => opt.MapFrom(src => src.Vacancy.IsPositiveAboutDisability))
                .ForMember(x => x.VacancyStatus, opt => opt.MapFrom(src => src.VacancyStatus))
                .ForMember(x => x.ClosingDate, opt => opt.MapFrom(src => src.Vacancy.ClosingDate))
                .ForMember(x => x.IsArchived, opt => opt.MapFrom(src => src.IsArchived))
                .ForMember(x => x.DateUpdated, opt => opt.MapFrom(src => src.DateUpdated))
                .ForMember(x => x.DateApplied, opt => opt.MapFrom(src => src.DateApplied));
        }
    }
}
