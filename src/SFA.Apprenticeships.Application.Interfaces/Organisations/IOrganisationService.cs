namespace SFA.Apprenticeships.Application.Interfaces.Organisations
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;

    public interface IOrganisationService
    {
        VerifiedOrganisationSummary GetVerifiedOrganisationSummary(string referenceNumber);

        IEnumerable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string edsUrn, string name, string location);
    }
}
