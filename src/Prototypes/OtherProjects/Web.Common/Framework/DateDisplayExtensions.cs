namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;

    public static class DateDisplayExtensions
    {
        public static string ToFriendlyClosingWeek(this DateTime closingDate)
        {
            var utcDateTime = DateTime.SpecifyKind(closingDate.Date, DateTimeKind.Utc);
            var daysLeft = (int)(utcDateTime - DateTime.UtcNow.Date).TotalDays;

            if (daysLeft > 7 || daysLeft < 0)
            {
                return utcDateTime.ToString("dd MMM yyyy");
            }

            switch (daysLeft)
            {
                case 0:
                    return "today";
                case 1:
                    return "tomorrow";
                default:
                    return "in " + daysLeft + " days";
            }
        }

        public static string ToFriendlyDaysAgo(this DateTime date)
        {
            var utcDateTime = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
            var daysAgo = (int)(DateTime.UtcNow.Date - utcDateTime).TotalDays;

            if (daysAgo > 7 || daysAgo < 0)
            {
                return utcDateTime.ToString("dd MMM yyyy");
            }

            switch (daysAgo)
            {
                case 0:
                    return "today";
                case 1:
                    return "yesterday";
                default:
                    return daysAgo + " days ago";
            }
        }

        public static string ToFriendlyClosingToday(this DateTime closingDate)
        {
            var utcDateTime = DateTime.SpecifyKind(closingDate.Date, DateTimeKind.Utc);
            var daysLeft = (int)(utcDateTime - DateTime.UtcNow.Date).TotalDays;

            switch (daysLeft)
            {
                case 0:
                    return "today";
                default:
                    return utcDateTime.ToString("dd MMM yyyy");
            }
        }
    }
}
