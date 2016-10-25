namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;

    public interface IApprenticeshipApplicationUpdater
    {
        void Create(Guid applicationGuid);
        void Update(Guid applicationGuid);
        void Delete(Guid applicationGuid);
    }
}