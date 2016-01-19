namespace SFA.Apprenticeships.Infrastructure.Common.DateTime
{
    using System;
    using SFA.Infrastructure.Interfaces;

    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}