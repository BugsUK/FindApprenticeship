namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using System;

    public class AnonymisedApplicationLink
    {
        public AnonymisedApplicationLink(Guid applicationId, DateTime expirationDateTime)
        {
            ApplicationId = applicationId;
            ExpirationDateTime = expirationDateTime;
        }

        public Guid ApplicationId { get; private set; }

        public DateTime ExpirationDateTime { get; private set; }
    }
}
