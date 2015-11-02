namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class CountyData
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string CodeName { get; set; }        
    }
}
