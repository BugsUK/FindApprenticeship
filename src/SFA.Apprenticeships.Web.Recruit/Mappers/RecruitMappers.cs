namespace SFA.Apprenticeships.Web.Recruit.Mappers
{
    using Common.ViewModels.Locations;
    using Domain.Entities.Applications;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Infrastructure.Common.Mappers;
    using Infrastructure.Presentation;
    using ViewModels.Application;

    public class RecruitMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();

            Mapper.CreateMap<ApprenticeshipVacancy, VacancyApplicationsViewModel>()
                .ForMember(v => v.EmployerName, opt => opt.MapFrom(src => src.ProviderSiteEmployerLink.Employer.Name))
                .ForMember(v => v.EmployerGeoPoint, opt => opt.MapFrom(src => Map<GeoPoint, GeoPointViewModel>(src.ProviderSiteEmployerLink.Employer.Address.GeoPoint)))
                .ForMember(v => v.ApplicationSummaries, opt => opt.Ignore());
            
            Mapper.CreateMap<ApprenticeshipApplicationSummary, ApplicationSummaryViewModel>()
                .ForMember(v => v.ApplicantName, opt => opt.MapFrom(src => new Name(src.CandidateDetails.FirstName, src.CandidateDetails.MiddleNames, src.CandidateDetails.LastName).GetDisplayText()))
                .ForMember(v => v.ApplicantLocation, opt => opt.MapFrom(src => src.CandidateDetails.Address.GetCityOrTownDisplayText()))
                .ForMember(v => v.ApplicantGeoPoint, opt => opt.MapFrom(src => Map<GeoPoint, GeoPointViewModel>(src.CandidateDetails.Address.GeoPoint)))
                .ForMember(v => v.Distance, opt => opt.Ignore());
        }
    }
}