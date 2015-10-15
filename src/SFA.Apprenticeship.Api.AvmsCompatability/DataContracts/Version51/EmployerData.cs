namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version51
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class EmployerData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public int EdsUrn { get; set; }

        [DataMember(IsRequired = false, Order = 2)]
        public string Description { get; set; }

        [DataMember(IsRequired = false, Order = 3)]
        public string AnonymousName { get; set; }

        [DataMember(IsRequired = false, Order = 4)]
        public string ContactName { get; set; }

        [DataMember(IsRequired = false, Order = 5)]
        public string Website { get; set; }

        [DataMember(IsRequired = false, Order = 6)]
        public byte[] Image { get; set; }
    }
}
