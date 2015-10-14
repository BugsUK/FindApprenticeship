namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version50
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Generic message header
    /// </summary>
    [MessageContract]
    public abstract class NavmsMessageHeader
    {
        [MessageHeader( Namespace = CommonNamespaces.ExternalInterfaces, MustUnderstand = true )]
        public Guid MessageId { get; set; }

        [MessageHeader( Namespace = CommonNamespaces.ExternalInterfaces, MustUnderstand = true )]
        public Guid ExternalSystemId { get; set; }

        [MessageHeader( Namespace = CommonNamespaces.ExternalInterfaces, MustUnderstand = true )]
        public string PublicKey { get; set; }
    }
}
