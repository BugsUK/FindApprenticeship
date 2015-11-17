namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Infrastructure.Common.Mappers;
    using Resolvers;
    using TypeConverters;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
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

            //Nullable datetime mapper requires an accompanying datetime mapper (below)
            //for cases when the nullable has a value.  Seems to be a peculiarity of automapper.
            Mapper.CreateMap<DateTime?, DateViewModel>()
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Value.Day))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Value.Month))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Value.Year));
            Mapper.CreateMap<DateTime, DateViewModel>()
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Day))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year));

            Mapper.CreateMap<ApprenticeshipVacancy, NewVacancyViewModel>();

            Mapper.CreateMap<ApprenticeshipVacancy, VacancySummaryViewModel>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.Value.ToNullableDouble()));

            Mapper.CreateMap<ApprenticeshipVacancy, VacancyRequirementsProspectsViewModel>();
            Mapper.CreateMap<ApprenticeshipVacancy, VacancyQuestionsViewModel>();
            Mapper.CreateMap<ApprenticeshipVacancy, VacancyViewModel>().ConvertUsing<ApprenticeshipVacancyToVacancyViewModelConverter>();
        }
    }
}