namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System.Text;
    using Vacancies.Apprenticeships;

    public static class SavedSearchHelper
    {
        public static string Name(this SavedSearch savedSearch)
        {
            var sb = new StringBuilder();

            if (savedSearch.SearchMode == ApprenticeshipSearchMode.Keyword)
            {
                sb.Append(string.IsNullOrEmpty(savedSearch.Keywords) ? "All" : savedSearch.Keywords);
            }
            else if (savedSearch.SearchMode == ApprenticeshipSearchMode.Category)
            {
                sb.Append(savedSearch.Category);
            }

            sb.Append(" within ");

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
    }
}