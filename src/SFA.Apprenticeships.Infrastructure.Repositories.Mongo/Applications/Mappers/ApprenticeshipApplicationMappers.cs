﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Applications.Mappers
{
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Entities;
    using Infrastructure.Common.Mappers;
    using Sql.Schemas.Vacancy;

    public class ApprenticeshipApplicationMappers : MapperEngine
    {
        public override void Initialise()
        {
            InitialiseApplicationDetailMappers();
            InitialiseApplicationSummaryMappers();
        }

        private void InitialiseApplicationDetailMappers()
        {
            Mapper.CreateMap<MongoApprenticeshipSummary, ApprenticeshipSummary>();
            Mapper.CreateMap<ApprenticeshipSummary, MongoApprenticeshipSummary>();
            Mapper.CreateMap<ApprenticeshipApplicationDetail, MongoApprenticeshipApplicationDetail>();
            Mapper.CreateMap<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>();
        }

        private void InitialiseApplicationSummaryMappers()
        {
            Mapper.CreateMap<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationSummary>()
                .ForMember(x => x.ApplicationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.LegacyApplicationId, opt => opt.MapFrom(src => src.LegacyApplicationId))
                .ForMember(x => x.LegacyVacancyId, opt => opt.MapFrom(src => src.Vacancy.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Vacancy.Title))
                .ForMember(x => x.EmployerName, opt => opt.MapFrom(src => src.Vacancy.EmployerName))
                .ForMember(x => x.IsPositiveAboutDisability, opt => opt.MapFrom(src => src.Vacancy.IsPositiveAboutDisability))
                .ForMember(x => x.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(x => x.VacancyStatus, opt => opt.MapFrom(src => src.VacancyStatus))
                .ForMember(x => x.ClosingDate, opt => opt.MapFrom(src => src.Vacancy.ClosingDate))
                .ForMember(x => x.IsArchived, opt => opt.MapFrom(src => src.IsArchived))
                .ForMember(x => x.DateUpdated, opt => opt.MapFrom(src => src.DateUpdated))
                .ForMember(x => x.DateApplied, opt => opt.MapFrom(src => src.DateApplied));
        }
    }
}
