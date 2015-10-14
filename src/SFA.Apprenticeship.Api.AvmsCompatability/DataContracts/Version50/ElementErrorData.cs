namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version50
{
    using System.Runtime.Serialization;

    [DataContract( Namespace = CommonNamespaces.ExternalInterfaces )]
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
