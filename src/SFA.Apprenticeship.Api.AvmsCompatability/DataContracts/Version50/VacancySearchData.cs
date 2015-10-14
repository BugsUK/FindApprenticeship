namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version50
{
    using System;
    using System.Runtime.Serialization;
    using Common;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfaces)]
    public class VacancySearchData
    {
            [DataMember(IsRequired = false, Order = 1)]
            public int? VacancyReferenceId { get; set; }

            [DataMember(IsRequired = false, Order = 2)]
            public string OccupationCode { get; set; }

            [DataMember(IsRequired = false, Order = 3)]
            public string FrameworkCode { get; set; }

            [DataMember(IsRequired = true, Order = 4)]
            public VacancyDetailsSearchLocationType VacancyLocationType { get; set; }

            [DataMember(IsRequired = false, Order = 5)]
            public string CountyCode { get; set; }

            [DataMember(IsRequired = false, Order = 6)]
            public string Town { get; set; }

            [DataMember(IsRequired = false, Order = 7)]
            public string RegionCode { get; set; }

            [DataMember(IsRequired = false, Order = 8)]
            public DateTime? VacancyPublishedDate { get; set; }

            [DataMember(IsRequired = true, Order = 9)]
            public int PageIndex { get; set; }
    }
}
