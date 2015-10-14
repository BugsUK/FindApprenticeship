namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version50
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfaces)]
    public class ApprenticeshipFrameworkAndOccupationData
    {
        [DataMember]
        public string ApprenticeshipFrameworkCodeName { get; set; }
        [DataMember]
        public string ApprenticeshipFrameworkShortName { get; set; }
        [DataMember]
        public string ApprenticeshipFrameworkFullName { get; set; }
        [DataMember]
        public string ApprenticeshipOccupationCodeName { get; set; }
        [DataMember]
        public string ApprenticeshipOccupationShortName { get; set; }
        [DataMember]
        public string ApprenticeshipOccupationFullName { get; set; }
    }
}
