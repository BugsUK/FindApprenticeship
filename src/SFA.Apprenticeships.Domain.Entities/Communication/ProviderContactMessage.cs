namespace SFA.Apprenticeships.Domain.Entities.Communication
{
    using System;

    public class ProviderContactMessage : BaseEntity
    {
        public ContactMessageTypes Type { get; set; }

        public Guid? UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Enquiry { get; set; }

        public string Details { get; set; }
    }
}