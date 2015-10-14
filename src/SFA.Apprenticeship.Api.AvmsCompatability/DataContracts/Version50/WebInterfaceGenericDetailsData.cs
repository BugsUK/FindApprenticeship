namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version50
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfaces)]
    public class WebInterfaceGenericDetailsData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string ApprenticeshipVacanciesDescription { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public string ApprenticeshipVacanciesURL { get; set; }
    }
}
