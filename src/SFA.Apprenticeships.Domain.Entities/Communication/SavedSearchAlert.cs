namespace SFA.Apprenticeships.Domain.Entities.Communication
{
    using System;

    public class SavedSearchAlert : BaseEntity
    {
        //todo: 1.7: properties for a saved search alert item

        public Guid? BatchId { get; set; }

        public DateTime? SentDateTime { get; set; }
    }
}
