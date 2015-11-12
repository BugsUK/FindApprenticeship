namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class CountyData
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string CodeName { get; set; }
    }
}
