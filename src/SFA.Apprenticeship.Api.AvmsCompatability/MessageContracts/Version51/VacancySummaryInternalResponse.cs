namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version51
{
    using System.ServiceModel;
    using DataContracts.Version51;

    [MessageContract]
    public class VacancySummaryInternalResponse: NavmsInternalResponseHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51, Order = 1)]
        public VacancySummaryResponseData ResponseData { get; set; }
    }
}
