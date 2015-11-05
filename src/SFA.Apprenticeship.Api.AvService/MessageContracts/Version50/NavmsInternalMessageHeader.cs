namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System;
    using System.ServiceModel;
    using Common;
    using Namespaces.Version50;

    /// <summary>
    /// Generic message header
    /// </summary>
    [MessageContract]
    public abstract class NavmsInternalMessageHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri)]
        public Guid ExternalSystemId { get; set; }

        /// <summary>
        /// Gets or sets the internal id (normaly UPRN). Can be null (at least for 3rd parties)
        /// </summary>
        /// <value>The internal system id.</value>
        [MessageBodyMember(Namespace = Namespace.Uri)]
        public int? InternalId { get; set; }

        [MessageBodyMember(Namespace = Namespace.Uri)]
        public Guid MessageId { get; set; }

        [MessageBodyMember(Namespace = Namespace.Uri)]
        public ExternalSystemType SystemType { get; set; }
    }
}
