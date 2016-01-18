namespace SFA.Apprenticeships.Infrastructure.Repositories.Reference.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Reference;

    public class ReferenceMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<County, NewDB.Domain.Entities.Reference.County>()
                .ForMember(c => c.PostalAddresses, opt => opt.Ignore());
            Mapper.CreateMap<NewDB.Domain.Entities.Reference.County, County>();
        }
    }
}