namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version50
{
    using System.ServiceModel;
    using DataContracts.Version50;

    [MessageContract]
    public class VacancySummaryResponse: NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfaces, Order = 1)]
        public VacancySummaryResponseData ResponseData { get; set; }
    }
}
