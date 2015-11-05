namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class WebInterfaceGenericDetailsData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string ApprenticeshipVacanciesDescription { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public string ApprenticeshipVacanciesURL { get; set; }
    }
}
