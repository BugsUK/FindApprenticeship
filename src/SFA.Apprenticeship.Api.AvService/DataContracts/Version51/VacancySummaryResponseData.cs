namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class VacancySummaryResponseData
    {
        [DataMember(IsRequired = true, Order = 1)]
        // ReSharper disable once InconsistentNaming
        public WebInterfaceGenericDetailsData AVMSHeader { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public List<VacancySummaryData> SearchResults { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public int TotalPages { get; set; }
    }
}
