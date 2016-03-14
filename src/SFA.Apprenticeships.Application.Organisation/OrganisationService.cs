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

        public IEnumerable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string edsUrn, string name, string location, out int resultCount)
        {
            if (!string.IsNullOrEmpty(edsUrn))
            {
                var verifiedOrganisationSummary = _verifiedOrganisationProvider.GetByReferenceNumber(edsUrn);
                if (verifiedOrganisationSummary == null)
                {
                    resultCount = 0;
                    return new List<VerifiedOrganisationSummary>();
                }
                resultCount = 1;
                return new List<VerifiedOrganisationSummary>
                {
                    verifiedOrganisationSummary
                };
            }

            return _verifiedOrganisationProvider.Find(name, location, out resultCount);
        }
    }
}
