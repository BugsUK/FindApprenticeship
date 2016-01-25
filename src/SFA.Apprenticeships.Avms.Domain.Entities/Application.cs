namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Application")]
    public partial class Application
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Application()
        {
            AdditionalAnswers = new HashSet<AdditionalAnswer>();
            ApplicationHistories = new HashSet<ApplicationHistory>();
            CAFFields = new HashSet<CAFField>();
            SubVacancies = new HashSet<SubVacancy>();
        }

        public int ApplicationId { get; set; }

        public int CandidateId { get; set; }

        public int VacancyId { get; set; }

        public int ApplicationStatusTypeId { get; set; }

        public int WithdrawnOrDeclinedReasonId { get; set; }

        public int UnsuccessfulReasonId { get; set; }

        [StringLength(100)]
        public string OutcomeReasonOther { get; set; }

        public int NextActionId { get; set; }

        [StringLength(100)]
        public string NextActionOther { get; set; }

        [StringLength(200)]
        public string AllocatedTo { get; set; }

        public int? CVAttachmentId { get; set; }

        [StringLength(50)]
        public string BeingSupportedBy { get; set; }

        public DateTime? LockedForSupportUntil { get; set; }

        public bool? WithdrawalAcknowledged { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdditionalAnswer> AdditionalAnswers { get; set; }

        public virtual ApplicationNextAction ApplicationNextAction { get; set; }

        public virtual ApplicationStatusType ApplicationStatusType { get; set; }

        public virtual ApplicationUnsuccessfulReasonType ApplicationUnsuccessfulReasonType { get; set; }

        public virtual ApplicationWithdrawnOrDeclinedReasonType ApplicationWithdrawnOrDeclinedReasonType { get; set; }

        public virtual AttachedDocument AttachedDocument { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual Vacancy Vacancy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationHistory> ApplicationHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAFField> CAFFields { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubVacancy> SubVacancies { get; set; }
    }
}
