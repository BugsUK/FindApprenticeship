namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;

    public interface IApprenticeshipApplicationUpdater
    {
        void Update(Guid applicationGuid);
    }
}