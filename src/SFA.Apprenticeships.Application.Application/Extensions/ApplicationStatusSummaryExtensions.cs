namespace SFA.Apprenticeships.Application.Application.Extensions
{
    using Entities;
    using System;

    public static class ApplicationStatusSummaryExtensions
    {
        public static bool IsLegacySystemUpdate(this ApplicationStatusSummary applicationStatusSummary)
        {
            return applicationStatusSummary.ApplicationId == Guid.Empty;
        }
    }
}
