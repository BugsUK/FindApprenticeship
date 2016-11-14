namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using Application.Interfaces;

    public class ApprenticeshipApplicationUpdater : ApplicationUpdater, IApprenticeshipApplicationUpdater
    {
        public ApprenticeshipApplicationUpdater(IConfigurationService configurationService, ILogService logService) : base("apprenticeships", configurationService, logService)
        {
        }
    }
}