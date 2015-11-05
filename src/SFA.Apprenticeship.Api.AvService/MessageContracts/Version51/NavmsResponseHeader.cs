namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System;
    using System.ServiceModel;
    using Namespaces.Version51;

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
