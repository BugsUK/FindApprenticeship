namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;

    public interface ICandidateUserUpdater
    {
        void Create(Guid candidateGuid);
        void Update(Guid candidateGuid);
        void Delete(Guid candidateGuid);
    }
}