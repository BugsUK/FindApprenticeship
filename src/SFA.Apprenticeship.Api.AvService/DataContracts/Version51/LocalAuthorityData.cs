namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
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
