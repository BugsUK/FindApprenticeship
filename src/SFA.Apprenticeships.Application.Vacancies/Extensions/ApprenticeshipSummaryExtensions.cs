namespace SFA.Apprenticeships.Application.Vacancies.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Domain.Entities.Vacancies;

    public static class ApprenticeshipSummaryExtensions
    {
        public static string GetResultsHash(this IEnumerable<ApprenticeshipSummary> apprenticeshipSummaries)
        {
            using (var md5Hash = MD5.Create())
            {
                var resultIds = string.Join(",", apprenticeshipSummaries.Select(s => s.Id));

                var hashIds = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(resultIds));

                var sb = new StringBuilder();

                foreach (byte hashId in hashIds)
                {
                    sb.Append(hashId.ToString("X2"));
                }

                var hash = sb.ToString();

                return hash;
            }
        }
    }
}