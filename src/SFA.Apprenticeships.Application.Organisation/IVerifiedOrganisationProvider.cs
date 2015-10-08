namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using Domain.Entities.Organisations;

    public interface IVerifiedOrganisationProvider
    {
        VerifiedOrganisationSummary GetByReferenceNumber(string referenceNumber);

        IEnumerable<VerifiedOrganisationSummary> Find(string employerName, string postcodeOrTown);
    }
}
