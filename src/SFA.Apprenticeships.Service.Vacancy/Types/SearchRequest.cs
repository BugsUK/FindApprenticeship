namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "http://candidates.gov.uk")]
    public class SearchRequest
    {
        [DataMember(Order = 1)]
        public string TestRunId { get; set; }

        [DataMember(Order = 2)]
        public string SearchTerms { get; set; }

        [DataMember(Order = 3)]
        public SearchFactorsParameter[] SearchFactorsParameters { get; set; }

        [DataMember(Order = 4)]
        public string SearchField { get; set; }
    }
}
