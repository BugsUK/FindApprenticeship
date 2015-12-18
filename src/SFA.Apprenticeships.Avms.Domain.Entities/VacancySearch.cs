namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VacancySearch")]
    public partial class VacancySearch
    {
        public int VacancySearchId { get; set; }

        public int VacancyId { get; set; }

        public int VacancyReferenceNumber { get; set; }

        [Required]
        [StringLength(256)]
        public string EmployerName { get; set; }

        [Required]
        [StringLength(256)]
        public string VacancyOwnerName { get; set; }

        [StringLength(256)]
        public string DeliveryOrganisationName { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int? LocalAuthorityID { get; set; }

        public DateTime VacancyPostedDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(256)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public DateTime ApplicationClosingDate { get; set; }

        public int ApprenticeshipFrameworkId { get; set; }

        [Required]
        [StringLength(200)]
        public string ApprenticeshipOccupationName { get; set; }

        [Required]
        [StringLength(200)]
        public string ApprenticeshipFrameworkName { get; set; }

        public int CountyId { get; set; }

        [Column(TypeName = "money")]
        public decimal? WeeklyWage { get; set; }

        public int WageType { get; set; }

        [Required]
        [StringLength(40)]
        public string Town { get; set; }

        public int ApprenticeshipType { get; set; }

        public int ApplicationClosingDateAsInt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(4000)]
        public string EmployerSearch { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(4000)]
        public string TrainingProviderSearch { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(4000)]
        public string DeliveryOrganisationSearch { get; set; }

        public string RealityCheck { get; set; }

        public string OtherImportantInformation { get; set; }

        public bool NationalVacancy { get; set; }

        public virtual ApprenticeshipFramework ApprenticeshipFramework { get; set; }

        public virtual ApprenticeshipType ApprenticeshipType1 { get; set; }

        public virtual County County { get; set; }

        public virtual Vacancy Vacancy { get; set; }

        public virtual VacancyStatusType VacancyStatusType { get; set; }
    }
}
