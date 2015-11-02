namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version50;

    [MessageContract]
    public class SubmitApplicationTrackingResponse : NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfaces)]
        public List<ApplicationTrackingResultData> Vacancies { get; set; }

        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfaces)]
        public ElementErrorData ErrorCode { get; set; }
    }
}
