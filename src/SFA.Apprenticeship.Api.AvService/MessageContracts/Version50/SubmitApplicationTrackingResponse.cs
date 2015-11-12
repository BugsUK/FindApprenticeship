namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version50;
    using Namespaces.Version50;

    [MessageContract]
    public class SubmitApplicationTrackingResponse : NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri)]
        public List<ApplicationTrackingResultData> Vacancies { get; set; }

        [MessageBodyMember(Namespace = Namespace.Uri)]
        public ElementErrorData ErrorCode { get; set; }
    }
}
