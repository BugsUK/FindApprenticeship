namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version51;
    using Namespaces.Version51;

    [MessageContract]
    public class SubmitApplicationTrackingInternalResponse : NavmsInternalResponseHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri)]
        public List<ApplicationTrackingResultData> Vacancies { get; set; }
    }
}
