namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System.Text.RegularExpressions;

    public static class VacancyTextPresenter
    {
        private static readonly Regex HtmlRegex = new Regex("<br\\s?/?>");

        public static string GetPreserveFormattingCssClass(this string vacancyText)
        {
            return string.IsNullOrEmpty(vacancyText) || HtmlRegex.IsMatch(vacancyText) ? "" : "preserve-formatting";
        }
    }
}