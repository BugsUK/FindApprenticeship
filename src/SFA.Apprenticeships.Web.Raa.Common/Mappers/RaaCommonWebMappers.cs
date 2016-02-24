namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using System;
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

    public class RaaCommonWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();
            Mapper.CreateMap<PostalAddress, AddressViewModel>()
                .ForMember(dest => dest.Uprn, opt => opt.Ignore());
            //TODO: Bring PostalAddressViewModel over from SQL branch and fix mapping
            Mapper.CreateMap<AddressViewModel, PostalAddress>()
                .ForMember(dest => dest.PostalAddressId, opt => opt.Ignore())
                .ForMember(dest => dest.AddressLine5, opt => opt.Ignore())
                .ForMember(dest => dest.Town, opt => opt.Ignore())
                .ForMember(dest => dest.ValidationSourceCode, opt => opt.Ignore())
                .ForMember(dest => dest.ValidationSourceKeyValue, opt => opt.Ignore())
                .ForMember(dest => dest.DateValidated, opt => opt.Ignore())
                .ForMember(dest => dest.County, opt => opt.Ignore())
                .ForMember(dest => dest.GeoPoint, opt => opt.Ignore());
            Mapper.CreateMap<Employer, EmployerViewModel>();
            Mapper.CreateMap<VacancyParty, VacancyPartyViewModel>()
                .ForMember(dest => dest.IsEmployerLocationMainApprenticeshipLocation, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyGuid, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfPositions, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyReferenceNumber, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerDescriptionComment, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerWebsiteUrlComment, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfPositionsComment, opt => opt.Ignore())
                .ForMember(dest => dest.Employer, opt => opt.Ignore());
            Mapper.CreateMap<VacancyLocation, VacancyLocationAddressViewModel>();
            Mapper.CreateMap<VacancyLocationAddressViewModel, VacancyLocation>()
                .ForMember(dest => dest.VacancyId, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyLocationId, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyLocationGuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDateTime, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDateTime, opt => opt.Ignore());

            Mapper.CreateMap<DateTime?, DateViewModel>().ConvertUsing<DateTimeToDateViewModelConverter>();
            Mapper.CreateMap<DateTime, DateViewModel>().ConvertUsing<DateTimeToDateViewModelConverter>();

            Mapper.CreateMap<Vacancy, VacancyDatesViewModel>().ConvertUsing<VacancyToVacancyDatesViewModelConverter>();

            Mapper.CreateMap<Vacancy, NewVacancyViewModel>()
                .ForMember(dest => dest.VacancyGuid, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore())
                .ForMember(dest => dest.Ukprn, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerParty, opt => opt.Ignore())
                .ForMember(dest => dest.LocationAddresses, opt => opt.Ignore());

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