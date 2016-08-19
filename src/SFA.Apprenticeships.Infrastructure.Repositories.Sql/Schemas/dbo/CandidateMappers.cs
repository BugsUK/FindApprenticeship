namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using Domain.Entities.Locations;
    using Infrastructure.Common.Mappers;
    using CandidateSummary = Domain.Entities.Candidates.CandidateSummary;
    using DbCandidateSummary = Entities.CandidateSummary;

    public class CandidateMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<DbCandidateSummary, CandidateSummary>()
                .ForMember(v => v.EntityId, opt => opt.MapFrom(src => src.CandidateGuid))
                .ForMember(v => v.LastName, opt => opt.MapFrom(src => src.Surname))
                .ForMember(v => v.Address, opt => opt.Ignore())
                .AfterMap((v, av) =>
                {
                    av.Address = new Address
                    {
                        AddressLine1 = v.AddressLine1,
                        AddressLine2 = v.AddressLine2,
                        AddressLine3 = v.AddressLine3,
                        AddressLine4 = v.AddressLine4,
                        Postcode = v.Postcode,
                        Town = v.Town,
                        County = v.County,
                        GeoPoint = new GeoPoint
                        {
                            Latitude = (double?) v.Latitude ?? 0d,
                            Longitude = (double?) v.Longitude ?? 0d
                        }
                    };
                });
        }
    }
}