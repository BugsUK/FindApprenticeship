namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;

    public class AsyncProcessorMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipApplicationDetail, ApplicationStatusAlert>()
                .ForMember(output => output.ApplicationId, map => map.MapFrom(input => input.EntityId))
                .ForMember(output => output.Title, map => map.MapFrom(input => input.Vacancy.Title))
                .ForMember(output => output.EmployerName, map => map.MapFrom(input => input.Vacancy.EmployerName));
        }
    }
}