namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using Application.Interfaces;

    public class ApprenticeshipApplicationUpdater : ApplicationUpdater, IApprenticeshipApplicationUpdater
    {
        public ApprenticeshipApplicationUpdater(IConfigurationService configurationService, ILogService logService) : base("apprenticeships", configurationService, logService)
        {
        }
    }
}