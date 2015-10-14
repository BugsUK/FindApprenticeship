namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version50
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfaces)]
    public class CountyData
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string CodeName { get; set; }        
    }
}
