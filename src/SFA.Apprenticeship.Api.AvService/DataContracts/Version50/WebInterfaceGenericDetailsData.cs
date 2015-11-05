namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version50
{
    using System.Runtime.Serialization;
    using Namespaces.Version50;

    [DataContract(Namespace = Namespace.Uri)]
    public class WebInterfaceGenericDetailsData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string ApprenticeshipVacanciesDescription { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        // ReSharper disable once InconsistentNaming
        public string ApprenticeshipVacanciesURL { get; set; }
    }
}
