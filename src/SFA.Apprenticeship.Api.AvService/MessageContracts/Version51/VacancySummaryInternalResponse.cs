namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System.ServiceModel;
    using DataContracts.Version51;
    using Namespaces.Version51;

    [MessageContract]
    public class VacancySummaryInternalResponse: NavmsInternalResponseHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri, Order = 1)]
        public VacancySummaryResponseData ResponseData { get; set; }
    }
}
