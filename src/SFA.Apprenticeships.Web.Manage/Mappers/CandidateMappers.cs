namespace SFA.Apprenticeships.Web.Manage.Mappers
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Infrastructure.Presentation;
    using Raa.Common.Mappers;
    using ViewModels;

    public class CandidateMappers : RaaCommonWebMappers
    {
        public override void Initialise()
        {
            base.Initialise();

            Mapper.CreateMap<CandidateSummary, CandidateSummaryViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(s => s.EntityId))
                .ForMember(m => m.Name, opt => opt.MapFrom(s => new Name(s.FirstName, s.MiddleNames, s.LastName).GetDisplayText()));
        }
    }
}