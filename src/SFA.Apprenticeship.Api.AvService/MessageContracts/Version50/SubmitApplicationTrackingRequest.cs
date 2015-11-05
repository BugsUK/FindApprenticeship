namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.ServiceModel;
    using DataContracts.Version50;
    using Namespaces.Version50;

    [MessageContract]
    public class SubmitApplicationTrackingRequest : NavmsMessageHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri)]
        public ApplicationTrackingData Vacancies { get; set; }
    }
}
