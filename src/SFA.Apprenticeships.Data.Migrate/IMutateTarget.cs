namespace SFA.Apprenticeships.Data.Migrate
{
    using System;

    public interface IMutateTarget
    {
        void Insert(dynamic record);
        void Update(dynamic record);
        void Delete(dynamic record);

        void NoChange(dynamic record);

        int NumberOfUpdates { get; }

        void FlushInsertsAndUpdates();
        void FlushDeletes();
    }
}
