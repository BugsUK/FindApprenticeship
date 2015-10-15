namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version51
{
    using System;
    using System.Runtime.Serialization;
    using Common;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class MessageData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public MessageEvent MessageEvent { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public string Text { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public string Title { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public DateTime MessageDate { get; set; }

        [DataMember(IsRequired = true, Order = 5)]
        public int Sender { get; set; }

        [DataMember(IsRequired = true, Order = 6)]
        public UserType SenderType { get; set; }

        [DataMember(IsRequired = true, Order = 7)]
        public int Recipient { get; set; }

        [DataMember(IsRequired = true, Order = 8)]
        public UserType RecipientType { get; set; }

        [DataMember(IsRequired = true, Order = 9)]
        public MessageCategory? MessageCategory { get; set; }

        [DataMember(IsRequired = true, Order = 10)]
        public MessageType MessageType { get; set; }

        [DataMember(IsRequired = true, Order = 11)]
        public string RecipientEmail { get; set; }
    }
}
