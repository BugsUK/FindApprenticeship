namespace SFA.Apprenticeships.Domain.Entities.Communication
{
    using System;
    using Candidates;

    public class SavedSearchAlert : BaseEntity
    {
        //todo: 1.8: properties for a saved search alert item (apprenticeships only)
        //todo: don't automap entity properties!
        public SavedSearch Search { get; set; }

        public Guid? BatchId { get; set; }

        public DateTime? SentDateTime { get; set; }
    }
}
