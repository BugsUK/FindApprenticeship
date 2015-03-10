namespace SFA.Apprenticeships.Application.Applications.Extensions
{
    using System;
    using Entities;

    internal static class ApplicationStatusSummaryExtensions
    {
        public static bool IsLegacySystemUpdate(this ApplicationStatusSummary applicationStatusSummary)
        {
            return applicationStatusSummary.ApplicationId == Guid.Empty;
        }
    }
}
