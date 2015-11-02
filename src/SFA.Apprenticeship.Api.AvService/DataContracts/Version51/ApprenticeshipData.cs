namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Common;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class ApprenticeshipData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string Framework { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public VacancyApprenticeshipType Type { get; set; }

        [DataMember(IsRequired = false, Order = 3)]
        public string TrainingToBeProvided { get; set; }

        [DataMember(IsRequired = false, Order = 4)]
        public string ExpectedDuration { get; set; }

    }
}
