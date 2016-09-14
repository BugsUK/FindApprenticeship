namespace SFA.Apprenticeships.Infrastructure.Raa.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;

    public interface IGetReleaseNotesStrategy
    {
        IList<ReleaseNote> GetReleaseNotes(DasApplication dasApplication);
    }
}