namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using Common.Providers;

    public class RecuitmentUserContext : UserContext
    {
        public Guid RecuiterId { get; set; }
    }
}
