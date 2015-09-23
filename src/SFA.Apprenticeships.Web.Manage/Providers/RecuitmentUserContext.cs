namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System;
    using Common.Providers;

    public class RecuitmentUserContext : UserContext
    {
        public Guid RecuiterId { get; set; }
    }
}
