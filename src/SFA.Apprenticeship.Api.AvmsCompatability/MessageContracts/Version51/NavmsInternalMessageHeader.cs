namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version51
{
    using System;
    using System.ServiceModel;
    using Common;

    /// <summary>
    /// Generic message header
    /// </summary>
    [MessageContract]
    public abstract class NavmsInternalMessageHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
        public Guid ExternalSystemId { get; set; }

        /// <summary>
        /// Gets or sets the internal id (normaly UPRN). Can be null (at least for 3rd parties)
        /// </summary>
        /// <value>The internal system id.</value>
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
        public int? InternalId { get; set; }

        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
        public Guid MessageId { get; set; }

        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
        public ExternalSystemType SystemType { get; set; }
    }
}
