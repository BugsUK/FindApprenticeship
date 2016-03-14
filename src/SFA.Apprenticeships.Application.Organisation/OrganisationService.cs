namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;
    using Infrastructure.Interfaces;
    using Interfaces.Organisations;

    public class OrganisationService : IOrganisationService
    {
        private readonly ILogService _logService;
        private readonly IVerifiedOrganisationProvider _verifiedOrganisationProvider;

        public OrganisationService(
            ILogService logService,
            IVerifiedOrganisationProvider verifiedOrganisationProvider)
        {
            _logService = logService;
            _verifiedOrganisationProvider = verifiedOrganisationProvider;
        }

        public VerifiedOrganisationSummary GetVerifiedOrganisationSummary(string referenceNumber)
        {
            Condition.Requires(referenceNumber).IsNotNullOrEmpty();

            _logService.Debug("Calling VerifiedOrganisationProvider to get organisation with reference='{0}'.", referenceNumber);

            return _verifiedOrganisationProvider.GetByReferenceNumber(referenceNumber);
        }

        public IEnumerable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string edsUrn, string name, string location)
        {
            if (!string.IsNullOrEmpty(edsUrn))
            {
                var verifiedOrganisationSummary = _verifiedOrganisationProvider.GetByReferenceNumber(edsUrn);
                if (verifiedOrganisationSummary == null)
                {
                    return new List<VerifiedOrganisationSummary>();
                }
                return new List<VerifiedOrganisationSummary>
                {
                    verifiedOrganisationSummary
                };
            }

            int resultCount;
            return _verifiedOrganisationProvider.Find(name, location, out resultCount);
        }
    }
}
