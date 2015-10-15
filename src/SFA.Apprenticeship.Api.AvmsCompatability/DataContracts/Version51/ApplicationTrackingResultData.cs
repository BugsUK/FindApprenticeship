namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version51
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class ApplicationTrackingResultData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public int VacancyReference { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public int ErrorCode { get; set; }
    }
}
