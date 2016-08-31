namespace SFA.Apprenticeships.Application.Communications.Housekeeping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candidates.Configuration;
    using Domain.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;

    public class ExpiringDraftApplicationAlertCommunicationHousekeeper : IExpiringDraftApplicationAlertCommunicationHousekeeper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExpiringApprenticeshipApplicationDraftRepository _expiringApprenticeshipApplicationDraftRepository;

        public ExpiringDraftApplicationAlertCommunicationHousekeeper(
            IConfigurationService configurationService,
            IExpiringApprenticeshipApplicationDraftRepository expiringApprenticeshipApplicationDraftRepository)
        {
            _configurationService = configurationService;
            _expiringApprenticeshipApplicationDraftRepository = expiringApprenticeshipApplicationDraftRepository;
        }

        public IEnumerable<CommunicationHousekeepingRequest> GetHousekeepingRequests()
        {
            var housekeepingDateTime = GetHousekeepingDateTime();

            return _expiringApprenticeshipApplicationDraftRepository
                .GetAlertsCreatedOnOrBefore(housekeepingDateTime)
                .Select(each => new CommunicationHousekeepingRequest
                {
                    CommunicationId = each,
                    CommunicationType = CommunicationTypes.ExpiringDraftApplicationAlert
                });
        }

        public void Handle(CommunicationHousekeepingRequest request)
        {
            if (request.CommunicationType == CommunicationTypes.ExpiringDraftApplicationAlert)
            {
                var housekeepingDateTime = GetHousekeepingDateTime();
                var expiringApprenticeshipApplicationDraft = _expiringApprenticeshipApplicationDraftRepository.Get(request.CommunicationId);

                if (expiringApprenticeshipApplicationDraft != null && expiringApprenticeshipApplicationDraft.DateCreated <= housekeepingDateTime)
                {
                    _expiringApprenticeshipApplicationDraftRepository.Delete(expiringApprenticeshipApplicationDraft);
                }
            }
            else if (Successor != null)
            {
                Successor.Handle(request);
            }
        }

        public ICommunicationHousekeeper Successor { get; set; }

        #region Helpers

        private DateTime GetHousekeepingDateTime()
        {
            var housekeepingConfiguration = _configurationService.Get<HousekeepingConfiguration>();

            return DateTime.UtcNow.AddHours(
                -housekeepingConfiguration.HousekeepingCycleInHours *
                housekeepingConfiguration.Communication.HardDeleteExpiringDraftApplicationAlertAfterCycles);
        }

        #endregion
    }
}