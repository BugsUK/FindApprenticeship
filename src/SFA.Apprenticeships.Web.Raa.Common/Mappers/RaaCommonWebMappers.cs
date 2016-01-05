namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
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
            Mapper.CreateMap<ProviderSiteEmployerLink, ProviderSiteEmployerLinkViewModel>()
                .ForMember(dest => dest.IsEmployerLocationMainApprenticeshipLocation, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyGuid, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfPositions, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyReferenceNumber, opt => opt.Ignore())
                .ForMember(dest => dest.DescriptionComment, opt => opt.Ignore())
                .ForMember(dest => dest.WebsiteUrlComment, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfPositionsComment, opt => opt.Ignore())
                ;
            Mapper.CreateMap<VacancyLocationAddress, VacancyLocationAddressViewModel>();

            Mapper.CreateMap<DateTime?, DateViewModel>().ConvertUsing<DateTimeToDateViewModelConverter>();
            Mapper.CreateMap<DateTime, DateViewModel>().ConvertUsing<DateTimeToDateViewModelConverter>();

            Mapper.CreateMap<ApprenticeshipVacancy, NewVacancyViewModel>()
                .ForMember(dest => dest.VacancyGuid, opt => opt.Ignore())
                .ForMember(dest => dest.SectorsAndFrameworks, opt => opt.Ignore())
                .ForMember(dest => dest.Standards, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());

            Mapper.CreateMap<ApprenticeshipVacancy, VacancySummaryViewModel>()
                .ForMember(dest => dest.Duration, opt => opt.ResolveUsing<NullableIntToNullableDecimalResolver>().FromMember(src => src.Duration))
                .ForMember(dest => dest.WageUnits, opt => opt.Ignore())
                .ForMember(dest => dest.DurationTypes, opt => opt.Ignore())
                .ForMember(dest => dest.WarningsHash, opt => opt.Ignore())
                .ForMember(dest => dest.AcceptWarnings, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());

            Mapper.CreateMap<ApprenticeshipVacancy, VacancyRequirementsProspectsViewModel>()
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());
            Mapper.CreateMap<ApprenticeshipVacancy, VacancyQuestionsViewModel>()
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());
            Mapper.CreateMap<ApprenticeshipVacancy, VacancyViewModel>().ConvertUsing<ApprenticeshipVacancyToVacancyViewModelConverter>();
        }
    }
}