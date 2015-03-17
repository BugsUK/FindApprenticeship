namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Security.Policy;
    using System.Text;
    using Extensions;
    using Vacancies.Apprenticeships;

    public static class SavedSearchHelper
    {
        public static string Name(this SavedSearch savedSearch)
        {
            var sb = new StringBuilder();

            if (savedSearch.SearchMode == ApprenticeshipSearchMode.Keyword)
            {
                if (!string.IsNullOrEmpty(savedSearch.Keywords))
                {
                    sb.Append(savedSearch.Keywords);
                }
            }
            else if (savedSearch.SearchMode == ApprenticeshipSearchMode.Category)
            {
                sb.Append(savedSearch.CategoryFullName);
            }

            sb.Append(sb.Length == 0 ? "Within " : " within ");

            if (savedSearch.WithinDistance == 0)
            {
                sb.Append("England");
            }
            else
            {
                sb.AppendFormat("{0} miles of {1}", savedSearch.WithinDistance, savedSearch.Location);
            }

            return sb.ToString();
        }

        public static Url SearchUrl(this SavedSearch savedSearch)
        {
            var propertyDictionary = savedSearch.GetType().GetProperties(BindingFlags.DeclaredOnly|BindingFlags.Public|BindingFlags.Instance).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(savedSearch, null)
            );

            propertyDictionary["SearchAction"] = "Search";
            propertyDictionary["LocationType"] = "NonNational";

            var urlSb = new StringBuilder("/apprenticeships");
            urlSb.Append("?");

            var excludedKeys = new[] { "CandidateId", "AlertsEnabled", "SubCategories", "LastResultsHash", "DateProcessed" };

            foreach (var kvp in propertyDictionary.Where(kvp => !excludedKeys.Contains(kvp.Key)))
            {
                if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.ToString()))
                {
                    urlSb.AppendFormat("{0}={1}&", kvp.Key, WebUtility.UrlEncode(kvp.Value.ToString()));
                }
            }

            urlSb.Remove(urlSb.Length - 1, 1);

            urlSb.Append(savedSearch.SubCategories.ToQueryString("SubCategories"));
            
            return new Url(urlSb.ToString());
        }

        public static bool HasGeoPoint(this SavedSearch savedSearch)
        {
            return savedSearch.Latitude.HasValue && savedSearch.Longitude.HasValue;
        }

        public static int GetLatLonLocHash(this SavedSearch savedSearch)
        {
            return string.Format("{0}{1}{2}", savedSearch.Longitude, savedSearch.Latitude, savedSearch.Location).GetHashCode();
        }
    }
}