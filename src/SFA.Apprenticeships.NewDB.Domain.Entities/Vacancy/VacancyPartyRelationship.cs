namespace SFA.Apprenticeships.NewDB.Domain.Entities.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vacancy.VacancyPartyRelationship")]
    public partial class VacancyPartyRelationship
    {
        public int VacancyPartyRelationshipId { get; set; }

        public int FromVacancyPartyId { get; set; }

        public int ToVacancyPartyId { get; set; }

        [StringLength(3)]
        public string VacancyPartyRelationshipTypeCode { get; set; }

        public virtual VacancyParty VacancyParty { get; set; }

        public virtual VacancyParty VacancyParty1 { get; set; }

        public virtual VacancyPartyRelationshipType VacancyPartyRelationshipType { get; set; }
    }
}
