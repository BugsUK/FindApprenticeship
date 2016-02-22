namespace SFA.Apprenticeships.Domain.Entities.Raa.Locations
{
    using System;

    public class VacancyLocationAddress : ICreatableEntity, IUpdatableEntity
    {
        public int VacancyLocationAddressId { get; set; }
        public Guid VacancyLocationAddressGuid { get; set; }
        public int VacancyId { get; set; }
        public PostalAddress Address { get; set; }
        public int NumberOfPositions { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}