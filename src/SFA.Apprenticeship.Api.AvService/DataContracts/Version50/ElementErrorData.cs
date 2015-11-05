namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version50
{
    using System.Runtime.Serialization;
    using Namespaces.Version50;

    [DataContract(Namespace = Namespace.Uri)]
    public class ElementErrorData
    {
        //[DataMember]
        //public string ElementName { get; set; }

        //[DataMember]
        //public string ErrorDescription { get; set; }

        [DataMember]
        public int ErrorCode { get; set; }
    }
}
