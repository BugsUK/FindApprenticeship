namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class ElementErrorData
    {
        [DataMember]
        public int ErrorCode { get; set; }
    }
}
