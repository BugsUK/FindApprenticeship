namespace SFA.Apprenticeships.Domain.Entities.Providers
{
    using System;

    public class Provider : BaseEntity<Guid>
    {
        public string Ukprn { get; set; }

        public string Name { get; set; }
    }
}
