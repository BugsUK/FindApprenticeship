namespace SFA.Apprenticeships.Application.Interfaces
{
    using System;

    public interface IDateTimeService
    {
        DateTime UtcNow { get; }

        DateTime MinValue { get; }

        DateTime TwoWeeksFromUtcNow { get; }
    }
}