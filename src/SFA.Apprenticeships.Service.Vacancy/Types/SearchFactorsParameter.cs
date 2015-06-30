namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using Infrastructure.VacancySearch.Configuration;

    public class SearchFactorsParameter
    {
        public string Name { get; set; }
 
        public SearchTermFactors Value { get; set; }
    }
}