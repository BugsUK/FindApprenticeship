namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version50
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfaces)]
    public class ErrorCodesData
    {
        [DataMember]
        public int InterfaceErrorCode { get; set; }
        [DataMember]
        public string InterfaceErrorDescription { get; set; }
    }
}
