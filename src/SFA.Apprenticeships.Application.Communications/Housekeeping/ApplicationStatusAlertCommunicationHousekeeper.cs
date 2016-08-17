namespace SFA.Apprenticeships.Application.Communications.Housekeeping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candidates.Configuration;
    using Domain.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;

    public class ApplicationStatusAlertCommunicationHousekeeper : IApplicationStatusAlertCommunicationHousekeeper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IApplicationStatusAlertRepository _applicationStatusAlertRepository;

        public ApplicationStatusAlertCommunicationHousekeeper(
            IConfigurationService configurationService,
            IApplicationStatusAlertRepository applicationStatusAlertRepository)
        {
            _configurationService = configurationService;
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
        }

        public IEnumerable<CommunicationHousekeepingRequest> GetHousekeepingRequests()
        {
            var housekeepingDateTime = GetHousekeepingDateTime();

            return _applicationStatusAlertRepository
                .GetAlertsCreatedOnOrBefore(housekeepingDateTime)
                .Select(each => new CommunicationHousekeepingRequest
                {
                    CommunicationId = each,
                    CommunicationType = CommunicationTypes.ApplicationStatusAlert
                });
        }

        public void Handle(CommunicationHousekeepingRequest request)
        {
            if (request.CommunicationType == CommunicationTypes.ApplicationStatusAlert)
            {
                var housekeepingDateTime = GetHousekeepingDateTime();
                var applicationStatusAlert = _applicationStatusAlertRepository.Get(request.CommunicationId);

                if (applicationStatusAlert != null && applicationStatusAlert.DateCreated <= housekeepingDateTime)
                {
                    _applicationStatusAlertRepository.Delete(applicationStatusAlert);
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
                housekeepingConfiguration.Communication.HardDeleteApplicationStatusAlertAfterCycles);
        }

        #endregion
    }
}