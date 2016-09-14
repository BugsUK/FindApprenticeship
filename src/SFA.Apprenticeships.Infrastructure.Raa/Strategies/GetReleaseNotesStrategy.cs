namespace SFA.Apprenticeships.Infrastructure.Raa.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.ReferenceData;
    using Domain.Raa.Interfaces.Repositories;

    public class GetReleaseNotesStrategy : IGetReleaseNotesStrategy
    {
        private readonly IReferenceRepository _referenceRepository;

        public GetReleaseNotesStrategy(IReferenceRepository referenceRepository)
        {
            _referenceRepository = referenceRepository;
        }

        public IList<ReleaseNote> GetReleaseNotes(DasApplication dasApplication)
        {
            var releaseNotes = _referenceRepository.GetReleaseNotes();
            return releaseNotes.Where(rn => rn.Application.HasFlag(dasApplication)).ToList();
        }
    }
}