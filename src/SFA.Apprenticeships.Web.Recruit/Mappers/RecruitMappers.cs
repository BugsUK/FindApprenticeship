namespace SFA.Apprenticeships.Web.Recruit.Mappers
{
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Infrastructure.Common.Mappers;
    using ViewModels.Application;

    public class RecruitMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipVacancy, VacancyApplicationsViewModel>()
                .ForMember(v => v.EmployerName, opt => opt.MapFrom(src => src.ProviderSiteEmployerLink.Employer.Name))
                .ForMember(v => v.ApplicationSummaries, opt => opt.Ignore());
        }
    }
}