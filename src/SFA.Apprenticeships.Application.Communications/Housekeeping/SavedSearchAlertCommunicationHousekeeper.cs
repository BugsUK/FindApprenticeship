namespace SFA.Apprenticeships.Application.Communications.Housekeeping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candidates.Configuration;
    using Domain.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;

    public class SavedSearchAlertCommunicationHousekeeper : ISavedSearchAlertCommunicationHousekeeper
    {
        private readonly IConfigurationService _configurationService;
        private readonly ISavedSearchAlertRepository _savedSearchAlertRepository;

        public SavedSearchAlertCommunicationHousekeeper(
            IConfigurationService configurationService,
            ISavedSearchAlertRepository savedSearchAlertRepository)
        {
            _configurationService = configurationService;
            _savedSearchAlertRepository = savedSearchAlertRepository;
        }

        public IEnumerable<CommunicationHousekeepingRequest> GetHousekeepingRequests()
        {
            var housekeepingDateTime = GetHousekeepingDateTime();

            return _savedSearchAlertRepository
                .GetAlertsCreatedOnOrBefore(housekeepingDateTime)
                .Select(each => new CommunicationHousekeepingRequest
                {
                    CommunicationId = each,
                    CommunicationType = CommunicationTypes.SavedSearchAlert
                });
        }

        public void Handle(CommunicationHousekeepingRequest request)
        {
            if (request.CommunicationType == CommunicationTypes.SavedSearchAlert)
            {
                var housekeepingDateTime = GetHousekeepingDateTime();
                var savedSearchAlert = _savedSearchAlertRepository.Get(request.CommunicationId);

                if (savedSearchAlert != null && savedSearchAlert.DateCreated <= housekeepingDateTime)
                {
                    _savedSearchAlertRepository.Delete(savedSearchAlert);
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
                housekeepingConfiguration.Communication.HardDeleteSavedSearchAlertAfterCycles);
        }

        #endregion
    }
}