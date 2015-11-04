namespace SFA.Apprenticeships.Infrastructure.Common.DateTime
{
    using System;
    using Application.Interfaces.DateTime;

    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}