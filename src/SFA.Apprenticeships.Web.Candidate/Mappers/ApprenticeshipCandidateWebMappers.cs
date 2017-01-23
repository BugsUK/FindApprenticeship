namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Common.Mappers.Resolvers;
    using Common.ViewModels;
    using Common.ViewModels.Locations;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using Infrastructure.Common.Mappers;
    using Resolvers;
    using ViewModels.Account;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using ViewModels.Home;
    using ViewModels.Register;
    using ViewModels.VacancySearch;

    public class ApprenticeshipCandidateWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Wage, WageViewModel>().ConvertUsing<WageToWageViewModelConverter>();
            Mapper.CreateMap<WageViewModel, Wage>().ConvertUsing<WageViewModelToWageConverter>();

            Mapper.CreateMap<SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>, ApprenticeshipSearchResponseViewModel>()
                .ConvertUsing<ApprenticeshipSearchResultsResolver>();

            Mapper.CreateMap<ApprenticeshipSearchViewModel, Location>()
                .ConvertUsing<LocationResolver>();

            Mapper.CreateMap<Location, LocationViewModel>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.GeoPoint.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.GeoPoint.Longitude));

            Mapper.CreateMap<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>()
                .ForMember(d => d.VacancyStatus,
                    opt => opt.MapFrom(src => src.VacancyStatus))
                .ForMember(d => d.EmployerName,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.EmployerNameResolver>())
                .ForMember(d => d.Wage,
                    opt => opt.MapFrom(src => src.Wage))
                .ForMember(d => d.RealityCheck,
                    opt => opt.MapFrom(src => src.RealityCheck))
                .ForMember(d => d.OtherInformation,
                    opt => opt.MapFrom(src => src.OtherInformation))
                .ForMember(d => d.ApplyViaEmployerWebsite,
                    opt => opt.MapFrom(src => src.ApplyViaEmployerWebsite))
                .ForMember(d => d.VacancyUrl,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.UrlResolver>()
                .FromMember(src => src.VacancyUrl))
                .ForMember(d => d.IsWellFormedVacancyUrl,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.IsWellFormedUrlResolver>()
                .FromMember(src => src.VacancyUrl))
                .ForMember(d => d.ApplicationInstructions,
                    opt => opt.MapFrom(src => src.ApplicationInstructions))
                .ForMember(d => d.IsRecruitmentAgencyAnonymous,
                    opt => opt.MapFrom(src => src.IsRecruitmentAgencyAnonymous))
                .ForMember(d => d.RecruitmentAgency,
                    opt => opt.MapFrom(src => src.RecruitmentAgency))
                .ForMember(d => d.IsEmployerAnonymous,
                    opt => opt.MapFrom(src => src.IsEmployerAnonymous))
                .ForMember(d => d.IsNasProvider,
                    opt => opt.MapFrom(src => src.IsNasProvider))
                .ForMember(d => d.EmployerWebsite,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.UrlResolver>()
                .FromMember(src => src.EmployerWebsite))
                .ForMember(d => d.IsWellFormedEmployerWebsiteUrl,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.IsWellFormedUrlResolver>()
                .FromMember(src => src.EmployerWebsite))
                .ForMember(d => d.VacancyType,
                    opt => opt.Ignore())
                .ForMember(d => d.VacancyLocationType,
                    opt => opt.MapFrom(src => src.VacancyLocationType))
                .ForMember(d => d.CandidateApplicationStatus,
                    opt => opt.Ignore())
                .ForMember(d => d.DateApplied,
                    opt => opt.Ignore())
                .ForMember(d => d.ViewModelMessage,
                    opt => opt.Ignore())
                .ForMember(d => d.Distance,
                    opt => opt.Ignore())
                .ForMember(d => d.IsMultiLocation,
                    opt => opt.Ignore());

            Mapper.CreateMap<ApprenticeshipSearchResponse, ApprenticeshipVacancySummaryViewModel>()
                .ForMember(d => d.CandidateApplicationStatus,
                    opt => opt.Ignore())
                .ForMember(d => d.VacancyLocationType,
                    opt => opt.MapFrom(src => src.VacancyLocationType))
                .ForMember(d => d.Wage, opt => opt.MapFrom(src => src.Wage))
                .ForMember(d => d.GoogleStaticMapsUrl, opt => opt.Ignore());

            Mapper.CreateMap<Address, AddressViewModel>()
                .ForMember(a => a.AddressLine1, opt => opt.Ignore())
                .ForMember(a => a.AddressLine2, opt => opt.Ignore())
                .ForMember(a => a.AddressLine3, opt => opt.Ignore())
                .ForMember(a => a.AddressLine4, opt => opt.Ignore())
                .ForMember(a => a.AddressLine5, opt => opt.Ignore())
                .AfterMap((source, dest) =>
                {
                    dest.AddressLine1 = source.AddressLine1;
                    dest.AddressLine2 = source.AddressLine2;
                    dest.AddressLine3 = source.AddressLine3;
                    dest.AddressLine4 = source.AddressLine4;
                    dest.Town = source.Town;
                    dest.County = source.County;
                });
            Mapper.CreateMap<AddressViewModel, Address>();

            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();
            Mapper.CreateMap<GeoPointViewModel, GeoPoint>();

            Mapper.CreateMap<RegisterViewModel, Candidate>()
                .ConvertUsing<CandidateResolver>();

            Mapper.CreateMap<MonitoringInformationViewModel, MonitoringInformation>()
                .ForMember(d => d.Gender, opt => opt.MapFrom(s => (Gender?)s.Gender))
                .ForMember(d => d.Ethnicity, opt => opt.MapFrom(s => s.Ethnicity))
                .ForMember(d => d.DisabilityStatus, opt => opt.MapFrom(s => (DisabilityStatus?)s.DisabilityStatus));

            Mapper.CreateMap<ApprenticeshipApplicationViewModel, ApprenticeshipApplicationDetail>()
                .ConvertUsing<ApprenticeshipApplicationViewModelToApprenticeshipApplicationDetailResolver>();

            Mapper.CreateMap<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>()
                .ConvertUsing<ApprenticeshipApplicationDetailToApprenticeshipApplicationViewModelResolver>();

            Mapper.CreateMap<RegistrationDetails, SettingsViewModel>()
                .ConvertUsing<SettingsViewModelResolvers.RegistrationDetailsToSettingsViewModelResolver>();

            Mapper.CreateMap<ContactMessageViewModel, ContactMessage>()
                .ForMember(c => c.Type, opt => opt.UseValue(ContactMessageTypes.ContactUs))
                .ForMember(c => c.EntityId, opt => opt.Ignore())
                .ForMember(c => c.UserId, opt => opt.Ignore())
                .ForMember(c => c.DateCreated, opt => opt.Ignore())
                .ForMember(c => c.DateUpdated, opt => opt.Ignore());

            Mapper.CreateMap<FeedbackViewModel, ContactMessage>()
                .ForMember(c => c.Type, opt => opt.UseValue(ContactMessageTypes.Feedback))
                .ForMember(c => c.EntityId, opt => opt.Ignore())
                .ForMember(c => c.UserId, opt => opt.Ignore())
                .ForMember(c => c.Enquiry, opt => opt.Ignore())
                .ForMember(c => c.DateCreated, opt => opt.Ignore())
                .ForMember(c => c.DateUpdated, opt => opt.Ignore());
        }

        private static string AddAddressLine(string addressLine, string addressLineToAdd)
        {
            if (!string.IsNullOrWhiteSpace(addressLineToAdd))
            {
                if (!string.IsNullOrWhiteSpace(addressLine))
                {
                    addressLine += ", " + addressLineToAdd;
                }
                else
                {
                    addressLine += addressLineToAdd;
                }
            }

            return addressLine;
        }
    }
}