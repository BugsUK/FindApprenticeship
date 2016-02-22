namespace SFA.Apprenticeships.Web.Manage.Mappers
{
    using Common.ViewModels.Locations;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Infrastructure.Common.Mappers;
    using Infrastructure.Presentation;
    using ViewModels;

    public class CandidateMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();

            Mapper.CreateMap<CandidateSummary, CandidateSummaryViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(s => s.EntityId))
                .ForMember(m => m.Name, opt => opt.MapFrom(s => new Name(s.FirstName, s.MiddleNames, s.LastName).GetDisplayText()));
        }
    }
}