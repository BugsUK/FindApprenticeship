namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version50
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Generic message header
    /// </summary>
    [MessageContract]
    public abstract class NavmsResponseHeader
    {
        [MessageBodyMember( Namespace = CommonNamespaces.ExternalInterfaces )]
        public Guid MessageId { get; set; }
    }
}
