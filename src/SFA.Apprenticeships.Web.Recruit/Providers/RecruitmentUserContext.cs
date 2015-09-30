namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using Common.Providers;

    public class RecruitmentUserContext : UserContext
    {
        public Guid RecruiterId { get; set; }
    }
}
