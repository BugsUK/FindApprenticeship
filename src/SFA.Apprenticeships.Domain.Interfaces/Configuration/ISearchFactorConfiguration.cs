namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    public interface ISearchFactorConfiguration
    {
        ISearchTermFactors JobTitleFactors { get; }

        ISearchTermFactors EmployerFactors { get; }

        ISearchTermFactors DescriptionFactors { get; }
    }

    public interface ISearchTermFactors
    {
        bool Enabled { get; }

        string Boost { get; }

        int Fuzziness { get; }

        int FuzzyPrefix { get; }

        bool MatchAllKeywords { get; }

        string MinimumMatch { get; }
    }
}
