namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;

    public interface IVerifiedOrganisationProvider
    {
        VerifiedOrganisationSummary GetByReferenceNumber(string referenceNumber);

        IEnumerable<VerifiedOrganisationSummary> Find(string employerName, string postcodeOrTown, out int resultCount);
    }
}
