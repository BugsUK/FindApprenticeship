namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version51;
    using Namespaces.Version51;

    [MessageContract]
    public class SubmitApplicationTrackingRequest : NavmsMessageHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri)]
        public List<ApplicationTrackingData> Vacancies { get; set; }
    }
}
