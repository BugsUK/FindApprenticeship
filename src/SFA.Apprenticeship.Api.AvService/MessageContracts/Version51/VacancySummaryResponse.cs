namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System.ServiceModel;
    using DataContracts.Version51;

    [MessageContract]
    public class VacancySummaryResponse: NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51, Order = 1)]
        public VacancySummaryResponseData ResponseData { get; set; }
    }
}
