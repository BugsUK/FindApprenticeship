namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
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
