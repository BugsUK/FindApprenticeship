namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version51
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class LocalAuthorityData
    {
        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public string FullName { get; set; }
        
        [DataMember]
        public string County { get; set; }
    }
}
