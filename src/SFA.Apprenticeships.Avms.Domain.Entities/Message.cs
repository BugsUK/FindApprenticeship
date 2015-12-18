namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message")]
    public partial class Message
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Message()
        {
            Candidates = new HashSet<Candidate>();
        }

        public int MessageId { get; set; }

        public int Sender { get; set; }

        public int SenderType { get; set; }

        public int Recipient { get; set; }

        public int RecipientType { get; set; }

        public DateTime MessageDate { get; set; }

        public int MessageEventId { get; set; }

        public string Text { get; set; }

        [StringLength(1000)]
        public string Title { get; set; }

        public bool IsRead { get; set; }

        public bool IsDeleted { get; set; }

        public int? MessageCategoryID { get; set; }

        public DateTime? ReadDate { get; set; }

        [StringLength(250)]
        public string DeletedBy { get; set; }

        [StringLength(250)]
        public string ReadByFirst { get; set; }

        public DateTime? DeletedDate { get; set; }

        public virtual MessageEvent MessageEvent { get; set; }

        public virtual UserType UserType { get; set; }

        public virtual UserType UserType1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}
