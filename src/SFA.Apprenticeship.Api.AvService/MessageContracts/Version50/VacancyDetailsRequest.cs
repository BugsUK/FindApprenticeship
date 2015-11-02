namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.ServiceModel;
    using DataContracts.Version50;

    [MessageContract]
    public class VacancyDetailsRequest: NavmsMessageHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfaces, Order = 1)]
        public VacancySearchData VacancySearchCriteria { get; set; }
    }
}
