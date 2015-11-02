namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.ServiceModel;
    using DataContracts.Version50;

    [MessageContract]
    public class VacancyDetailsResponse: NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfaces, Order = 1)]
        public VacancyDetailResponseData SearchResults { get; set; }
    }
}
