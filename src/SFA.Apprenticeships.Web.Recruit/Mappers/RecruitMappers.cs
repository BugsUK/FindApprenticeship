namespace SFA.Apprenticeships.Web.Recruit.Mappers
{
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Users;
    using Infrastructure.Presentation;
    using Raa.Common.Mappers;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyManagement;
    using ViewModels.Home;

    public class RecruitMappers : RaaCommonWebMappers
    {
        public override void Initialise()
        {
            base.Initialise();

            Mapper.CreateMap<Vacancy, VacancyApplicationsViewModel>()
                .ForMember(v => v.VacancyApplicationsSearch, opt => opt.Ignore())
                .ForMember(v => v.EmployerName, opt => opt.Ignore())
                .ForMember(v => v.EmployerGeoPoint, opt => opt.Ignore())
                .ForMember(v => v.NewApplicationsCount, opt => opt.Ignore())
                .ForMember(v => v.InProgressApplicationsCount, opt => opt.Ignore())
                .ForMember(v => v.SuccessfulApplicationsCount, opt => opt.Ignore())
                .ForMember(v => v.UnsuccessfulApplicationsCount, opt => opt.Ignore())
                .ForMember(v => v.ApplicationSummaries, opt => opt.Ignore());

            Mapper.CreateMap<ApplicationSummary, ApplicationSummaryViewModel>()
                .ForMember(v => v.FirstName, opt => opt.MapFrom(src => src.CandidateDetails.FirstName))
                .ForMember(v => v.LastName, opt => opt.MapFrom(src => src.CandidateDetails.LastName))
                .ForMember(v => v.ApplicantName, opt => opt.MapFrom(src => new Name(src.CandidateDetails.FirstName, src.CandidateDetails.MiddleNames, src.CandidateDetails.LastName).GetDisplayText()))
                .ForMember(v => v.ApplicantID, opt => opt.MapFrom(src => src.CandidateId.GetApplicantId(0)))
                .ForMember(v => v.AnonymousLinkData, opt => opt.Ignore());

            Mapper.CreateMap<ContactMessageViewModel, ProviderContactMessage>()
                .ForMember(c => c.Type, opt => opt.UseValue(ContactMessageTypes.ContactUs))
                .ForMember(c => c.EntityId, opt => opt.Ignore())
                .ForMember(c => c.UserId, opt => opt.Ignore())
                .ForMember(c => c.DateCreated, opt => opt.Ignore())
                .ForMember(c => c.DateUpdated, opt => opt.Ignore());

            Mapper.CreateMap<VacancySummary, EditWageViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Wage.Type))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Wage.Amount))
                .ForMember(dest => dest.AmountLowerBound, opt => opt.MapFrom(src => src.Wage.AmountLowerBound))
                .ForMember(dest => dest.AmountUpperBound, opt => opt.MapFrom(src => src.Wage.AmountUpperBound))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Wage.Unit))
                .ForMember(dest => dest.ExistingWage, opt => opt.MapFrom(src => src.Wage))
                .ForMember(dest => dest.VacancyApplicationsState, opt => opt.MapFrom(src => src.ApplicantCount > 0 ? VacancyApplicationsState.HasApplications : VacancyApplicationsState.NoApplications));
        }
    }
}