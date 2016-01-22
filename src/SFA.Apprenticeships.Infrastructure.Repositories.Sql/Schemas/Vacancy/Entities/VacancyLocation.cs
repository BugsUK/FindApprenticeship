namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Vacancy.VacancyLocation")]
    public class VacancyLocation
    {
        public int VacancyLocationId { get; set; }

        public Guid VacancyId { get; set; }

        public int PostalAddressId { get; set; }

        public int NumberOfPositions { get; set; }

        public string DirectApplicationUrl { get; set; }
    }
}
