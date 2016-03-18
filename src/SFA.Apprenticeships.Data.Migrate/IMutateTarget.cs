namespace SFA.Apprenticeships.Data.Migrate
{
    using System;

    public interface IMutateTarget : IDisposable
    {
        void Insert(dynamic record);
        void Update(dynamic record);
        void Delete(dynamic record);

        void NoChange(dynamic record);

        int NumberOfUpdates { get; }
    }
}
