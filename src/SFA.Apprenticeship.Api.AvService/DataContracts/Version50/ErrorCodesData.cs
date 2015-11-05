namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version50
{
    using System.Runtime.Serialization;
    using Namespaces.Version50;

    [DataContract(Namespace = Namespace.Uri)]
    public class ErrorCodesData
    {
        [DataMember]
        public int InterfaceErrorCode { get; set; }
        [DataMember]
        public string InterfaceErrorDescription { get; set; }
    }
}
