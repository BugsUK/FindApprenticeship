namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version51;

    [MessageContract]
    public class GetErrorCodesResponse : NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
        public List<ErrorCodesData> ErrorCodes { get; set; }
    }
}
