namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class RegionData
    {
        [DataMember]
        public string CodeName { get; set; }
        [DataMember]
        public string FullName { get; set; }
    }
}
