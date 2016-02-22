namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using System;
    using Domain.Entities.Locations;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Infrastructure.Common.Mappers;
    using Resolvers;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels;
    using Web.Common.ViewModels.Locations;
    using GeoPoint = Domain.Entities.Raa.Locations.GeoPoint;

    public class RaaCommonWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();
            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<Employer, EmployerViewModel>();
            Mapper.CreateMap<VacancyParty, VacancyPartyViewModel>()
                .ForMember(dest => dest.IsEmployerLocationMainApprenticeshipLocation, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyGuid, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfPositions, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyReferenceNumber, opt => opt.Ignore())
                .ForMember(dest => dest.DescriptionComment, opt => opt.Ignore())
                .ForMember(dest => dest.WebsiteUrlComment, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfPositionsComment, opt => opt.Ignore());
            Mapper.CreateMap<VacancyLocationAddress, VacancyLocationAddressViewModel>();

            Mapper.CreateMap<DateTime?, DateViewModel>().ConvertUsing<DateTimeToDateViewModelConverter>();
            Mapper.CreateMap<DateTime, DateViewModel>().ConvertUsing<DateTimeToDateViewModelConverter>();

            Mapper.CreateMap<Vacancy, VacancyDatesViewModel>().ConvertUsing<VacancyToVacancyDatesViewModelConverter>();

            Mapper.CreateMap<Vacancy, NewVacancyViewModel>()
                .ForMember(dest => dest.VacancyGuid, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());

            Mapper.CreateMap<Vacancy, TrainingDetailsViewModel>()
                .ForMember(dest => dest.SectorsAndFrameworks, opt => opt.Ignore())
                .ForMember(dest => dest.Standards, opt => opt.Ignore())
                .ForMember(dest => dest.Sectors, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());

            Mapper.CreateMap<Vacancy, FurtherVacancyDetailsViewModel>().ConvertUsing<VacancyToFurtherVacancyDetailsViewModelConverter>();

            Mapper.CreateMap<Vacancy, VacancyRequirementsProspectsViewModel>()
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());
            Mapper.CreateMap<Vacancy, VacancyQuestionsViewModel>()
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());
            Mapper.CreateMap<Vacancy, VacancyViewModel>().ConvertUsing<VacancyToVacancyViewModelConverter>();
            Mapper.CreateMap<Vacancy, VacancySummaryViewModel>().ConvertUsing<VacancyToVacancySummaryViewModelConverter>();
        }
    }
}