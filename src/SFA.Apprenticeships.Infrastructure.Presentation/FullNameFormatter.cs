namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System.Text.RegularExpressions;

    public class FullNameFormatter
    {
        private static readonly Regex FullNameRegex = new Regex(@"(^.+?)(\(.+?\))$");

        public static string Format(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return fullName;
            var match = FullNameRegex.Match(fullName);
            if (match.Success)
            {
                fullName = match.Groups[1].Value;
            }
            return fullName.Trim();
        }
    }
}