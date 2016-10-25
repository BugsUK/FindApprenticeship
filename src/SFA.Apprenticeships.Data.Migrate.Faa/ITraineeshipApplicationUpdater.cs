namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;

    public interface ITraineeshipApplicationUpdater
    {
        void Create(Guid applicationGuid);
        void Update(Guid applicationGuid);
        void Delete(Guid applicationGuid);
    }
}