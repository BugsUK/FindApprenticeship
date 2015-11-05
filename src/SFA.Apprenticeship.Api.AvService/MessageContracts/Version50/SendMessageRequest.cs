namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.ServiceModel;
    using DataContracts.Version50;
    using Namespaces.Version50;

    [MessageContract]
    public class SendMessageRequest
    {
        [MessageBodyMember(Namespace = Namespace.Uri, Order = 1)]
        public VacancyTransferLogListData VacancyTransferLogList { get; set; }

        [MessageBodyMember(Namespace = Namespace.Uri, Order = 2)]
        // ReSharper disable once InconsistentNaming
        public string webServerName { get; set; }
    }
}
