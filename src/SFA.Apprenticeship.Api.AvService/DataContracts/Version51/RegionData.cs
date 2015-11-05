namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class RegionData
    {
        [DataMember]
        public string CodeName { get; set; }
        [DataMember]
        public string FullName { get; set; }
    }
}
