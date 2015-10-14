namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version50
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfaces)]
    public class ApplicationTrackingResultData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public int VacancyReference { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public ElementErrorData ErrorCode { get; set; }
    }
}
