namespace SFA.Infrastructure.Interfaces
{
    using System;

    public interface IDateTimeService
    {
        DateTime UtcNow { get; }

        DateTime MinValue { get; }
    }
}