namespace SFA.Apprenticeships.Infrastructure.VacancySearch.Configuration
{
    public class SearchFactorConfiguration
    {
        public const string ConfigurationName = "SearchTermFactors";

        public SearchTermFactors JobTitleFactors { get; set; }

        public SearchTermFactors EmployerFactors { get; set; }

        public SearchTermFactors ProviderFactors { get; set; }

        public SearchTermFactors DescriptionFactors { get; set; }
    }

    public class SearchTermFactors
    {
        public bool Enabled { get; set; }

        public double? Boost { get; set; }

        public int? Fuzziness { get; set; }

        public int? FuzzyPrefix { get; set; }

        public bool MatchAllKeywords { get; set; }

        public int? PhraseProximity { get; set; }

        public string MinimumMatch { get; set; }
    }
}
