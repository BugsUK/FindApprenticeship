namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.ServiceModel;
    using DataContracts.Version50;
    using Namespaces.Version50;

    [MessageContract]
    public class VacancyDetailsResponse : NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri, Order = 1)]
        public VacancyDetailResponseData SearchResults { get; set; }
    }
}
