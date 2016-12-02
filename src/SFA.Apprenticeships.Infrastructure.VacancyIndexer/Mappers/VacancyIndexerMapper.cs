namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Mappers
{
    using Application.Vacancies.Entities;
    using Common.Mappers;
    using Elastic.Common.Entities;

    public class VacancyIndexerMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>()
                .ForMember(d => d.Location, opt => opt.ResolveUsing<GeoPointDomainToElasticResolver>().FromMember(src => src.Location))
                .ForMember(d => d.WageType, opt => opt.MapFrom(src => src.Wage.Type))
                .ForMember(d => d.WageAmount, opt => opt.MapFrom(src => src.Wage.Amount))
                .ForMember(d => d.WageAmountLowerBound, opt => opt.MapFrom(src => src.Wage.AmountLowerBound))
                .ForMember(d => d.WageAmountUpperBound, opt => opt.MapFrom(src => src.Wage.AmountUpperBound))
                .ForMember(d => d.WageText, opt => opt.MapFrom(src => src.Wage.Text))
                .ForMember(d => d.WageUnit, opt => opt.MapFrom(src => src.Wage.Unit))
                .ForMember(d => d.HoursPerWeek, opt => opt.MapFrom(src => src.Wage.HoursPerWeek))
                .ForMember(d => d.AnonymousEmployerName, opt => opt.MapFrom(src => src.AnonymousEmployerName))
                .ForMember(d => d.VacancyLocationType, opt => opt.MapFrom(src => src.ApprenticeshipLocationType));

            Mapper.CreateMap<TraineeshipSummaryUpdate, TraineeshipSummary>()
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<GeoPointDomainToElasticResolver>().FromMember(src => src.Location));
        }
    }
}