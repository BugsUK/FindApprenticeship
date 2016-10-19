namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using Application.Interfaces;

    public class TraineeshipApplicationUpdater : ApplicationUpdater, ITraineeshipApplicationUpdater
    {
        public TraineeshipApplicationUpdater(IConfigurationService configurationService, ILogService logService) : base("traineeships", configurationService, logService)
        {
        }
    }
}