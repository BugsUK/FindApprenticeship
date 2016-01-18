namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Reference;
    using Apprenticeships.Infrastructure.Common.Mappers;
    using DataContracts.Version51;

    public class ReferenceDataMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<County, CountyData>();
        }
    }
}