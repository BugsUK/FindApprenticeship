namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;

    [DataContract(Namespace = "http://candidates.gov.uk")]
    public class SearchResponse
    {
        [DataMember(Order = 1, Name = "Request")]
        public SearchRequest Request { get; set; }

        [DataMember(Order = 2, Name = "SearchResults")]
        public IEnumerable<ApprenticeshipSearchResponse> SearchResults { get; set; }
    }
}
