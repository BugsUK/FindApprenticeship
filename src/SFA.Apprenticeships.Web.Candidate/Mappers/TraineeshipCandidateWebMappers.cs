namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Common.Mappers.Resolvers;
    using Common.ViewModels;
    using Common.ViewModels.Locations;
    using Domain.Entities.Applications;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Traineeships;
    using Infrastructure.Common.Mappers;
    using Resolvers;
    using ViewModels.Applications;
    using ViewModels.VacancySearch;

    public class TraineeshipCandidateWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Wage, WageViewModel>().ConvertUsing<WageToWageViewModelConverter>();
            Mapper.CreateMap<WageViewModel, Wage>().ConvertUsing<WageViewModelToWageConverter>();

            Mapper.CreateMap<SearchResults<TraineeshipSearchResponse, TraineeshipSearchParameters>, TraineeshipSearchResponseViewModel>()
                .ConvertUsing<TraineeshipSearchResultsResolver>();

            Mapper.CreateMap<TraineeshipSearchResponse, TraineeshipVacancySummaryViewModel>()
                .ForMember(d => d.GoogleStaticMapsUrl, opt => opt.Ignore());

            Mapper.CreateMap<TraineeshipSearchViewModel, Location>()
                .ConvertUsing<LocationResolver>();

            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();
            Mapper.CreateMap<GeoPointViewModel, GeoPoint>();

            Mapper.CreateMap<TraineeshipVacancyDetail, TraineeshipVacancyDetailViewModel>()
                .ForMember(d => d.VacancyStatus,
                    opt => opt.MapFrom(src => src.VacancyStatus))
                .ForMember(d => d.EmployerName,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.EmployerNameResolver>())
                .ForMember(d => d.Wage, opt => opt.MapFrom(src => src.Wage))
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
                .ForMember(d => d.VacancyLocationType,
                    opt => opt.MapFrom(src => src.VacancyLocationType))
                .ForMember(d => d.VacancyType,
                    opt => opt.Ignore())
                .ForMember(d => d.CandidateApplicationStatus,
                    opt => opt.Ignore())
                .ForMember(d => d.DateApplied,
                    opt => opt.Ignore())
                .ForMember(d => d.ViewModelMessage,
                    opt => opt.Ignore())
                .ForMember(d => d.ApprenticeshipLevel,
                    opt => opt.Ignore())
                .ForMember(d => d.Distance,
                    opt => opt.Ignore());

            Mapper.CreateMap<Address, AddressViewModel>()
                .ForMember(a => a.AddressLine1, opt => opt.Ignore())
                 .ForMember(a => a.AddressLine2, opt => opt.Ignore())
                 .ForMember(a => a.AddressLine3, opt => opt.Ignore())
                 .ForMember(a => a.AddressLine4, opt => opt.Ignore())
                 .ForMember(a => a.AddressLine5, opt => opt.Ignore())
                 .AfterMap((source, dest) =>
                 {
                     var addressLine2 =
                         AddAddressLine(
                             AddAddressLine(
                                 AddAddressLine(null, source.AddressLine2), source.AddressLine3), source.AddressLine4);

                     dest.AddressLine1 = source.AddressLine1;
                     dest.AddressLine2 = addressLine2;
                     dest.AddressLine3 = source.Town;
                     dest.AddressLine4 = source.County;
                 });

            Mapper.CreateMap<AddressViewModel, Address>();

            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();
            Mapper.CreateMap<GeoPointViewModel, GeoPoint>();

            Mapper.CreateMap<TraineeshipApplicationViewModel, TraineeshipApplicationDetail>()
                .ConvertUsing<TraineeeshipApplicationViewModelToTraineeeshipApplicationDetailResolver>();

            Mapper.CreateMap<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>()
                .ConvertUsing<TraineeshipApplicationDetailToTraineeshipApplicationViewModelResolver>();
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