namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference
{
    using Domain.Entities.Reference;
    using Vacancy;

    public class ReferenceMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<County, Entities.County>()
                .ForMember(c => c.PostalAddresses, opt => opt.Ignore());
            Mapper.CreateMap<Entities.County, County>();
        }
    }
}