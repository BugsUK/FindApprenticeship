namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.WebService
{
    using Entities;
    using Vacancy;

    public class WebServiceMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<WebServiceConsumer, Domain.Entities.WebServices.WebServiceConsumer>()
                .ForMember(dest => dest.WebServiceConsumerType, opt =>
                    opt.ResolveUsing<WebServiceConsumerTypeResolver>()
                        .FromMember(src => src.WebServiceConsumerTypeCode));
        }
    }
}
