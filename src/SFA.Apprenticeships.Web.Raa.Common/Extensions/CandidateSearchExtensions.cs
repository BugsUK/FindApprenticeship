namespace SFA.Apprenticeships.Web.Raa.Common.Extensions
{
    using System.Text.RegularExpressions;

    public class CandidateSearchExtensions
    {
        private static readonly Regex CandidateGuidPrefixRegex = new Regex(@"[0-9,a-f,A-F,\s]+");
        private static readonly Regex CandidateIdRegex = new Regex(@"\d\d\d-\d\d\d-\d\d\d");

        public static string GetCandidateGuidPrefix(string applicantId)
        {
            if (string.IsNullOrEmpty(applicantId)) return null;

            var match = CandidateGuidPrefixRegex.Match(applicantId);
            if (match.Success)
            {
                var candidateGuidPrefix = match.Value.Replace(" ", "");
                if (candidateGuidPrefix.Length == 7)
                {
                    return candidateGuidPrefix;
                }
            }

            return null;
        }

        public static int? GetCandidateId(string applicantId)
        {
            if (string.IsNullOrEmpty(applicantId)) return null;

            var match = CandidateIdRegex.Match(applicantId);
            if (match.Success)
            {
                var candidateIdString = match.Value.Replace("-", "");
                int candidateId;
                if (int.TryParse(candidateIdString, out candidateId))
                {
                    return candidateId;
                }
            }

            return null;
        }
    }
}