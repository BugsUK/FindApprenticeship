namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Users;
    using Infrastructure.Presentation;
    using ViewModels.Application;
    using ViewModels.Candidate;
    using Web.Common.ViewModels.Locations;

    public class CandidateMappers : RaaCommonWebMappers
    {
        public override void Initialise()
        {
            base.Initialise();

            Mapper.CreateMap<CandidateSummary, CandidateSummaryViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(s => s.EntityId))
                .ForMember(m => m.Name, opt => opt.MapFrom(s => new Name(s.FirstName, s.MiddleNames, s.LastName).GetDisplayText()))
                .ForMember(m => m.ApplicantId, opt => opt.MapFrom(s => s.EntityId.GetApplicantId(s.LegacyCandidateId)));

            Mapper.CreateMap<Candidate, ApplicantDetailsViewModel>()
                .ForMember(m => m.CandidateId, opt => opt.MapFrom(s => s.EntityId))
                .ForMember(v => v.Name, opt => opt.MapFrom(src => new Name(src.RegistrationDetails.FirstName, src.RegistrationDetails.MiddleNames, src.RegistrationDetails.LastName).GetDisplayText()))
                .ForMember(v => v.Address, opt => opt.MapFrom(src => Map<Address, AddressViewModel>(src.RegistrationDetails.Address)))
                .ForMember(v => v.DateOfBirth, opt => opt.MapFrom(src => src.RegistrationDetails.DateOfBirth))
                .ForMember(v => v.PhoneNumber, opt => opt.MapFrom(src => src.RegistrationDetails.PhoneNumber))
                .ForMember(v => v.EmailAddress, opt => opt.MapFrom(src => src.RegistrationDetails.EmailAddress))
                .ForMember(v => v.DisabilityStatus, opt => opt.MapFrom(src => src.MonitoringInformation.DisabilityStatus))
                .ForMember(m => m.ApplicantId, opt => opt.MapFrom(s => s.EntityId.GetApplicantId(s.LegacyCandidateId)));

            Mapper.CreateMap<ApprenticeshipApplicationSummary, CandidateApplicationSummaryViewModel>()
                .ForMember(m => m.VacancyId, opt => opt.MapFrom(src => src.LegacyVacancyId))
                .ForMember(m => m.VacancyReferenceNumber, opt => opt.Ignore())
                .ForMember(m => m.VacancyTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(m => m.EmployerName, opt => opt.MapFrom(src => src.EmployerName))
                .ForMember(m => m.EmployerLocation, opt => opt.Ignore())
                .ForMember(m => m.VacancyType, opt => opt.UseValue(VacancyType.Apprenticeship))
                .ForMember(m => m.AnonymousLinkData, opt => opt.Ignore());

            Mapper.CreateMap<TraineeshipApplicationSummary, CandidateApplicationSummaryViewModel>()
                .ForMember(m => m.VacancyId, opt => opt.MapFrom(src => src.LegacyVacancyId))
                .ForMember(m => m.VacancyReferenceNumber, opt => opt.Ignore())
                .ForMember(m => m.VacancyTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(m => m.EmployerName, opt => opt.MapFrom(src => src.EmployerName))
                .ForMember(m => m.EmployerLocation, opt => opt.Ignore())
                .ForMember(m => m.VacancyType, opt => opt.UseValue(VacancyType.Traineeship))
                .ForMember(m => m.AnonymousLinkData, opt => opt.Ignore());
        }
    }
}