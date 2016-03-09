namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System;

    public class Provider : ICreatableEntity, IUpdatableEntity
    {
        public int ProviderId { get; set; }
        public Guid ProviderGuid { get; set; }
        public string Ukprn { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}
