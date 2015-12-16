namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System;

    public static class QualificationPresenter
    {
        public static string GetMonthYearLabel(this DateTime? dateTime)
        {
            if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
            {
                return GetMonthYearLabel(dateTime.Value);
            }
            return GetMonthYearLabel(0, null);
        }

        public static string GetMonthYearLabel(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return GetMonthYearLabel(0, null);
            }
            return GetMonthYearLabel(dateTime.Month, dateTime.Year.ToString());
        }

        public static string GetMonthYearLabel(int month, string year)
        {
            if (string.IsNullOrEmpty(year))
            {
                return "Current";
            }

            int intYear;
            int.TryParse(year, out intYear);

            if (intYear == DateTime.MinValue.Year)
            {
                return "Current";
            }

            var date = new DateTime(intYear, month, 1);

            return date.ToString("MMM yyyy");
        }

        public static string GetDisplayGrade(string grade, bool isPredicted)
        {
            return isPredicted ? $"{grade}(Predicted)" : grade;
        }
    }
}