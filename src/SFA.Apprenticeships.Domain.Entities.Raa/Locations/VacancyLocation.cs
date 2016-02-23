namespace SFA.Apprenticeships.Domain.Entities.Raa.Locations
{
    using System;

    public class VacancyLocation : ICreatableEntity, IUpdatableEntity
    {
        public int VacancyLocationId { get; set; }
        public Guid VacancyLocationGuid { get; set; }
        public int VacancyId { get; set; }
        public PostalAddress Address { get; set; }
        public int NumberOfPositions { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}