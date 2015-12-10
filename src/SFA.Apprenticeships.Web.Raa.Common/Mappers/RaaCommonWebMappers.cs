﻿namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using System;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Infrastructure.Common.Mappers;
    using Resolvers;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels;
    using Web.Common.ViewModels.Locations;

    public class RaaCommonWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();
            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<Employer, EmployerViewModel>();
            Mapper.CreateMap<ProviderSiteEmployerLink, ProviderSiteEmployerLinkViewModel>();
            Mapper.CreateMap<VacancyLocationAddress, VacancyLocationAddressViewModel>();

            Mapper.CreateMap<DateTime?, DateViewModel>().ConvertUsing<DateTimeToDateViewModelConverter>();
            Mapper.CreateMap<DateTime, DateViewModel>().ConvertUsing<DateTimeToDateViewModelConverter>();

            Mapper.CreateMap<ApprenticeshipVacancy, NewVacancyViewModel>();

            Mapper.CreateMap<ApprenticeshipVacancy, VacancySummaryViewModel>()
                .ForMember(dest => dest.Duration, opt => opt.ResolveUsing<NullableIntToNullableDecimalResolver>().FromMember(src => src.Duration));

            Mapper.CreateMap<ApprenticeshipVacancy, VacancyRequirementsProspectsViewModel>();
            Mapper.CreateMap<ApprenticeshipVacancy, VacancyQuestionsViewModel>();
            Mapper.CreateMap<ApprenticeshipVacancy, VacancyViewModel>().ConvertUsing<ApprenticeshipVacancyToVacancyViewModelConverter>();
        }
    }
}