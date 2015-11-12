namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version50
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Namespaces;
    using Namespaces.Version50;

    [DataContract(Namespace = Namespace.Uri)]
    public class ApplicationTrackingData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public List<ApplicationTrackingVacancyReferenceData> VacancyReferences { get; set; }
    }
}
