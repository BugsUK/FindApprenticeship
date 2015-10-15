namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version51
{
    using System.Runtime.Serialization;

    [DataContract(Namespace=CommonNamespaces.ExternalInterfacesRel51)]
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
