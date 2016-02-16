namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System;

    public static class DatePresenter
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

        public static string ToFriendlyDaysAgo(this DateTime? date)
        {
            return date.HasValue ? date.Value.ToFriendlyDaysAgo() : string.Empty;
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

        public static string ToFriendlyClosingToday(this DateTime? date)
        {
            return date.HasValue ? date.Value.ToFriendlyClosingToday() : string.Empty;
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

        public static string ToGmtDate(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToGmtDate() : "";
        }

        public static string ToGmtDate(this DateTime dateTime)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var dataTimeByZoneId = TimeZoneInfo.ConvertTime(dateTime.ToUniversalTime(), timeZoneInfo);

            return dataTimeByZoneId.ToString("dd MMM yyyy");
        }

        public static int GetDaysTillClose(this DateTime date)
        {
            var daysTillClose = (date - DateTime.UtcNow.Date).Days;
            return daysTillClose;
        }

        public static bool ShouldEmphasiseClosingDate(this DateTime date)
        {
            return date.GetDaysTillClose() == 0;
        }

        public static bool CloseToClosingDate(this DateTime? date)
        {
            return date?.CloseToClosingDate() ?? false;
        }

        public static bool CloseToClosingDate(this DateTime date)
        {
            return date.GetDaysTillClose() <= 5;
        }

        public static string GetClosingDate(this DateTime date)
        {
            var daysTillClose = GetDaysTillClose(date);
            if (daysTillClose == 0)
            {
                return "closing today";
            }
            if (daysTillClose == 1)
            {
                return "closing in 1 day";
            }
            if (daysTillClose > 1 && daysTillClose <= 5)
            {
                return $"closing in {daysTillClose} days";
            }

            return "";
        }

        public static string ToDateOfBirth(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }
    }
}