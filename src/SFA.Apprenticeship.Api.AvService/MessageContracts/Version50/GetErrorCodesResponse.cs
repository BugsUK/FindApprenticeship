namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version50;
    using Namespaces.Version50;

    [MessageContract]
    public class GetErrorCodesResponse : NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri)]
        public List<ErrorCodesData> ErrorCodes { get; set; }
    }
}
