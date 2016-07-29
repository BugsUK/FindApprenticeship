namespace SFA.Apprenticeships.Infrastructure.Common.DateTime
{
    using System;
    using SFA.Infrastructure.Interfaces;

    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime MinValue => DateTime.MinValue;

        public DateTime TwoWeeksFromUtcNow
        {
            get
            {
                var twoWeeksInFuture = UtcNow.AddDays(15);
                return new DateTime(twoWeeksInFuture.Year, twoWeeksInFuture.Month, twoWeeksInFuture.Day, 0, 0, 0, DateTimeKind.Utc);
            }
        }
    }
}