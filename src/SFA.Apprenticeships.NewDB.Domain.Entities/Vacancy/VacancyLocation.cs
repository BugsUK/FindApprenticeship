namespace SFA.Apprenticeships.NewDB.Domain.Entities.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vacancy.VacancyLocation")]
    public partial class VacancyLocation
    {
        public int VacancyLocationId { get; set; }

        public int VacancyId { get; set; }

        public int AddressId { get; set; }

        public int NumberOfPositions { get; set; }

        public string DirectApplicationUrl { get; set; }

        public virtual Vacancy Vacancy { get; set; }
    }
}
