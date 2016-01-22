namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.WebService
{
    using Domain.Entities.Reference;
    using Vacancy;

    public class WebServiceMappers : MapperEngine
    {
        public override void Initialise()
        {
            /*
            Mapper.CreateMap<County, Reference.Entities.County>()
                .ForMember(c => c.PostalAddresses, opt => opt.Ignore());
            Mapper.CreateMap<Reference.Entities.County, County>();
            */
        }
    }
}