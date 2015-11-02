namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System;
    using System.Runtime.Serialization;
    using Common;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class ApplicationData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public DateTime ClosingDate { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public DateTime InterviewStartDate { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public DateTime PossibleStartDate { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public ApplicationType Type { get; set; }

        [DataMember(IsRequired = false, Order = 5)]
        public string Instructions { get; set; }

    }
}
