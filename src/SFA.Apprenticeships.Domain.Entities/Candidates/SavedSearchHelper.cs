namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Security.Policy;
    using System.Text;
    using Extensions;
    using Vacancies;

    public static class SavedSearchHelper
    {
        public static string Name(this SavedSearch savedSearch)
        {
            var sb = new StringBuilder();

            if (savedSearch.SearchMode == ApprenticeshipSearchMode.Keyword)
            {
                if (!string.IsNullOrEmpty(savedSearch.Keywords))
                {
                    if (savedSearch.SearchField == "JobTitle")
                    {
                        sb.Append("Job title: ");
                    }
                    else if (savedSearch.SearchField != "All" && savedSearch.SearchField != "ReferenceNumber")
                    {
                        sb.AppendFormat("{0}: ", savedSearch.SearchField);
                    }

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

            var excludedKeys = new[]
            {
                "CandidateId", "AlertsEnabled", "CategoryFullName", "SubCategories", "SubCategoriesFullName",
                "LastResultsHash", "DateProcessed"
            };

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

        public static string GetSearchHash(this SavedSearch savedSearch)
        {
            var sb = new StringBuilder();

            sb.Append(savedSearch.SearchMode);
            sb.Append(savedSearch.Location);
            sb.Append(savedSearch.Longitude);
            sb.Append(savedSearch.Latitude);
            sb.Append(savedSearch.Keywords);
            sb.Append(savedSearch.WithinDistance);
            sb.Append(savedSearch.ApprenticeshipLevel);
            sb.Append(savedSearch.Category);
            if (savedSearch.SubCategories != null)
            {
                sb.Append(string.Join("", savedSearch.SubCategories));
            }
            sb.Append(savedSearch.SearchField);

            return sb.ToString().ToLower();
        }

        public static string TruncatedSubCategoriesFullNames(this SavedSearch savedSearch, int subCategoriesFullNamesLimit)
        {
            if (string.IsNullOrEmpty(savedSearch.SubCategoriesFullName))
            {
                return savedSearch.SubCategoriesFullName;
            }

            //For backwards compatability with existing saved searches
            var separator = savedSearch.SubCategoriesFullName.Contains("|") ? new[] {"|"} : new[] {", "};
            var split = savedSearch.SubCategoriesFullName.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var truncatedSubCategoriesFullName = string.Join(", ", split.Take(subCategoriesFullNamesLimit).Select(s => s.Trim()));

            if (split.Length <= subCategoriesFullNamesLimit)
            {
                return truncatedSubCategoriesFullName;
            }

            return string.Format("{0} and {1} more", truncatedSubCategoriesFullName, split.Length - subCategoriesFullNamesLimit);
        }
    }
}