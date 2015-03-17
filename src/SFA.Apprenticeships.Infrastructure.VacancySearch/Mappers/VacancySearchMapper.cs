﻿namespace SFA.Apprenticeships.Infrastructure.VacancySearch.Mappers
{
    using System.Collections.Generic;
    using Application.Interfaces.Vacancies;
    using Common.Mappers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Elastic.Common.Entities;
    using ApprenticeshipSummary = Elastic.Common.Entities.ApprenticeshipSummary;

    public class VacancySearchMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipSummary, ApprenticeshipSearchResponse>()
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<GeoPointElasticToDomainResolver>().FromMember(src => src.Location));

            Mapper.CreateMap<TraineeshipSummary, TraineeshipSearchResponse>()
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<GeoPointElasticToDomainResolver>().FromMember(src => src.Location));
        }
    }
}