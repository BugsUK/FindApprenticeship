namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System;
    using System.ServiceModel;
    using Namespaces.Version51;

    /// <summary>
    /// Generic message header
    /// </summary>
    [MessageContract]
    public abstract class NavmsMessageHeader
    {
        [MessageHeader(Namespace = Namespace.Uri, MustUnderstand = true)]
        public Guid MessageId { get; set; }

        [MessageHeader(Namespace = Namespace.Uri, MustUnderstand = true)]
        public Guid ExternalSystemId { get; set; }

        [MessageHeader(Namespace = Namespace.Uri, MustUnderstand = true)]
        public string PublicKey { get; set; }
    }
}
