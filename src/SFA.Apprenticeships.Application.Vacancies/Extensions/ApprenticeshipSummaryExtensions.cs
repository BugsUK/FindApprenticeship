namespace SFA.Apprenticeships.Application.Vacancies.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Domain.Entities.Vacancies.Apprenticeships;

    public static class ApprenticeshipSummaryExtensions
    {
        public static string GetResultsHash(this IEnumerable<ApprenticeshipSummary> apprenticeshipSummaries)
        {
            using (var md5Hash = MD5.Create())
            {
                var resultIds = string.Join(",", apprenticeshipSummaries.Select(s => s.Id));

                var hashData = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(resultIds));

                var sb = new StringBuilder();

                for (var i = 0; i < hashData.Length; i++)
                {
                    sb.Append(hashData[i].ToString("X2"));
                }

                var hash = sb.ToString();

                return hash;
            }
        }
    }
}