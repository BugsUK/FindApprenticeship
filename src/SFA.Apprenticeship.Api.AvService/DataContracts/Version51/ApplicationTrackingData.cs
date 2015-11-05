namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class ApplicationTrackingData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public int VacancyReference { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public int NewApplicationsCount { get; set; }
    }
}
