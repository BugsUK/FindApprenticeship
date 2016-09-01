namespace SFA.Apprenticeships.Application.UnitTests.Builders
{
    using Apprenticeships.Application.Application.Entities;
    using Domain.Entities.Applications;
    using System;

    public class ApplicationStatusSummaryBuilder
    {
        private readonly ApplicationStatuses _applicationStatus;

        private Guid _applicationId = Guid.Empty;
        private int _legacyApplicationId;
        private string _unsuccessfulReason;

        public ApplicationStatusSummaryBuilder(ApplicationStatuses applicationStatus)
        {
            _applicationStatus = applicationStatus;
        }

        public ApplicationStatusSummaryBuilder WithLegacyApplicationId(int legacyApplicationId)
        {
            _legacyApplicationId = legacyApplicationId;
            return this;
        }

        public ApplicationStatusSummaryBuilder IsLegacySystemUpdate(bool isLegacySystemUpdate)
        {
            _applicationId = isLegacySystemUpdate ? Guid.Empty : Guid.NewGuid();
            return this;
        }

        public ApplicationStatusSummaryBuilder WithUnsuccessfulReason(string unsuccessfulReason)
        {
            _unsuccessfulReason = unsuccessfulReason;
            return this;
        }

        public ApplicationStatusSummary Build()
        {
            var summary = new ApplicationStatusSummary
            {
                ApplicationId = _applicationId,
                LegacyApplicationId = _legacyApplicationId,
                ApplicationStatus = _applicationStatus,
                UnsuccessfulReason = _unsuccessfulReason
            };

            return summary;
        }
    }
}