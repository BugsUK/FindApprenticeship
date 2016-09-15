namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Infrastructure.Presentation;
    using ViewModels.Candidate;

    public class CandidateMappers : RaaCommonWebMappers
    {
        public override void Initialise()
        {
            base.Initialise();

            Mapper.CreateMap<CandidateSummary, CandidateSummaryViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(s => s.EntityId))
                .ForMember(m => m.Name, opt => opt.MapFrom(s => new Name(s.FirstName, s.MiddleNames, s.LastName).GetDisplayText()))
                .ForMember(m => m.ApplicantId, opt => opt.MapFrom(s => s.EntityId.GetApplicantId(s.LegacyCandidateId)));
        }
    }
}