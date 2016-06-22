namespace SFA.Apprenticeships.Infrastructure.Common.DateTime
{
    using System;

    using SFA.Apprenticeships.Application.Interfaces;

    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime MinValue => DateTime.MinValue;
    }
}