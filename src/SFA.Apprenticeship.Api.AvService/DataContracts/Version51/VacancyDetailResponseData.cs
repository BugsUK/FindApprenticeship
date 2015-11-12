namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class VacancyDetailResponseData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public WebInterfaceGenericDetailsData AVMSHeader { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public List<VacancyFullData> SearchResults { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public int TotalPages { get; set; }
    }
}
