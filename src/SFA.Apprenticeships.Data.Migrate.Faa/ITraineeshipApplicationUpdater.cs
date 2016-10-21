namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;

    public interface ITraineeshipApplicationUpdater
    {
        void Update(Guid applicationGuid);
    }
}