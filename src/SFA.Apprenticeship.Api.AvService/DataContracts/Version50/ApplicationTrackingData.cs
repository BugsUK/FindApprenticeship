namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version50
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfaces)]
    public class ApplicationTrackingData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public List<ApplicationTrackingVacancyReferenceData> VacancyReferences { get; set; }
    }
}
