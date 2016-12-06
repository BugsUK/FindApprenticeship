namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System.Runtime.Serialization;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Vacancies;
    using Infrastructure.VacancySearch.Configuration;

    [DataContract(Namespace = "http://candidates.gov.uk")]
    public class SearchResponse
    {
        [DataMember(Order = 1, Name = "Request")]
        public SearchRequest Request { get; set; }

        [DataMember(Order = 2, Name = "SearchResults")]
        public ApprenticeshipSearchResponse[] SearchResults { get; set; }

        [DataMember(Order = 3, Name = "Total")]
        public long Total { get; set; }

        [DataMember(Order = 4, Name = "SearchFactorConfiguration")]
        public SearchFactorConfiguration SearchFactorConfiguration { get; set; }

        [DataMember(Order = 4, Name = "Parameters")]
        public ApprenticeshipSearchParameters Parameters { get; set; }
    }
}