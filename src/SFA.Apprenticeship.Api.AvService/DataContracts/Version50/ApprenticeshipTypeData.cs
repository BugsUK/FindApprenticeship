namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version50
{
    using System.Runtime.Serialization;

    [DataContract(Namespace=CommonNamespaces.ExternalInterfaces)]
    public class ApprenticeshipTypeData 
    {
        [DataMember]
        public string CodeName { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string FullName { get; set; }
    }
}
