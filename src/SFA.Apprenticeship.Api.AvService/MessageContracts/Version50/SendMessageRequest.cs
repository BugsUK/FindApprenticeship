namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.ServiceModel;
    using DataContracts.Version50;

    [MessageContract]
    public class SendMessageRequest
    {
         [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfaces, Order = 1)]
         public VacancyTransferLogListData VacancyTransferLogList { get; set; }

         [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfaces, Order = 2)]
         public string webServerName { get; set; }
    }
}
