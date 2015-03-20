namespace SFA.Apprenticeships.Domain.Entities.Configuration
{
    public class SearchFactorConfiguration
    {
        public SearchTermFactors JobTitleFactors { get; set; }

        public SearchTermFactors EmployerFactors { get; set; }

        public SearchTermFactors DescriptionFactors { get; set; }
    }

    public class SearchTermFactors
    {
        public bool Enabled { get; set; }

        public string Boost { get; set; }

        public int Fuzziness { get; set; }

        public int FuzzyPrefix { get; set; }

        public bool MatchAllKeywords { get; set; }

        public string MinimumMatch { get; set; }
    }
}
