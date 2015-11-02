namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;

    [DataContract( Namespace = CommonNamespaces.ExternalInterfacesRel51 )]
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
