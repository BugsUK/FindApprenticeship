namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class WebInterfaceGenericDetailsData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string ApprenticeshipVacanciesDescription { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public string ApprenticeshipVacanciesURL { get; set; }
    }
}
