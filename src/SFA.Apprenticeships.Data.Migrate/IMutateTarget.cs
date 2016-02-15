namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;

    public interface IMutateTarget : IDisposable
    {
        void Insert(dynamic record);
        void Update(dynamic record);
    }
}
