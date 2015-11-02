namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class ErrorCodesData
    {
        [DataMember]
        public int InterfaceErrorCode { get; set; }
        [DataMember]
        public string InterfaceErrorDescription { get; set; }
    }
}
