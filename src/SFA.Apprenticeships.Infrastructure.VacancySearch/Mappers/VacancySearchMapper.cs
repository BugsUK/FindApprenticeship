namespace SFA.Apprenticeships.Infrastructure.VacancySearch.Mappers
{
    using Application.Interfaces.Vacancies;
    using AutoMapper;
    using Common.Mappers;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Elastic.Common.Entities;
    using ApprenticeshipSummary = Elastic.Common.Entities.ApprenticeshipSummary;

    public class VacancySearchMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipSummary, ApprenticeshipSearchResponse>()
                .ForMember(d => d.Distance, opt => opt.Ignore())
                .ForMember(d => d.Score, opt => opt.Ignore())
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<GeoPointElasticToDomainResolver>().FromMember(src => src.Location))
                .ForMember(d => d.Wage,
                    opt => opt.ResolveUsing<WageElasticToDomainResolver>().FromMember(src => src));

            Mapper.CreateMap<TraineeshipSummary, TraineeshipSearchResponse>()
                .ForMember(d => d.Distance, opt => opt.Ignore())
                .ForMember(d => d.Score, opt => opt.Ignore())
                .ForMember(d => d.SubCategory, opt => opt.Ignore())
                .ForMember(d => d.SubCategoryCode, opt => opt.Ignore())
                .ForMember(d => d.Score, opt => opt.Ignore())
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<GeoPointElasticToDomainResolver>().FromMember(src => src.Location));
        }
    }

    public class WageElasticToDomainResolver : ValueResolver<ApprenticeshipSummary, Wage>
    {
        protected override Wage ResolveCore(ApprenticeshipSummary source)
        {
            return new Wage((WageType)source.WageType, source.WageAmount, source.WageText, (WageUnit)source.WageUnit, source.HoursPerWeek);
        }
    }
}