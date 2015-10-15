namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version51
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Generic message header
    /// </summary>
    [MessageContract]
    public abstract class NavmsMessageHeader
    {
        [MessageHeader(Namespace = CommonNamespaces.ExternalInterfacesRel51, MustUnderstand = true)]
        public Guid MessageId { get; set; }

        [MessageHeader(Namespace = CommonNamespaces.ExternalInterfacesRel51, MustUnderstand = true)]
        public Guid ExternalSystemId { get; set; }

        [MessageHeader(Namespace = CommonNamespaces.ExternalInterfacesRel51, MustUnderstand = true)]
        public string PublicKey { get; set; }
    }
}
