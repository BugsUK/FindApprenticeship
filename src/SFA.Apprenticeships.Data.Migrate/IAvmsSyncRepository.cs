namespace SFA.Apprenticeships.Data.Migrate
{
    using System;

    public interface IAvmsSyncRespository
    {
        bool IsVacancyOwnedByTargetDatabase(int vacancyId);

        bool IsVacancyOwnerRelationshipOwnedByTargetDatabase(int vacancyOwnerRelationshipId);
    }
}
