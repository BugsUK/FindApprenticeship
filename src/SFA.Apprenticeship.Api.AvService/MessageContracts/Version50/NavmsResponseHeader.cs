namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System;
    using System.ServiceModel;
    using Namespaces.Version50;

    /// <summary>
    /// Generic message header
    /// </summary>
    [MessageContract]
    public abstract class NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri)]
        public Guid MessageId { get; set; }
    }
}
