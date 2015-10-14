namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version50
{
    using System.ServiceModel;
    using DataContracts.Version50;

    [MessageContract]
    public class SubmitApplicationTrackingRequest : NavmsMessageHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfaces)]
        public ApplicationTrackingData Vacancies { get; set; }
    }
}
