namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version51;
    using Namespaces;
    using Namespaces.Version51;

    [MessageContract]
    public class SendMessageRequest
    {
        [MessageBodyMember(Namespace = Namespace.Uri, Order = 1)]
        public List<MessageData> Messages { get; set; }
    }
}
