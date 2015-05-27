namespace SFA.Apprenticeships.Application.Candidates.Strategies.DormantAccount
{
    using System;
    using Domain.Entities.Users;

    public static class UserExtensions
    {
        public static DateTime GetLastLogin(this User user)
        {
            if (user.LastLogin.HasValue) return user.LastLogin.Value;
            if (user.ActivationDate.HasValue) return user.ActivationDate.Value;
            if (user.DateUpdated.HasValue) return user.DateUpdated.Value;
            return user.DateCreated;
        }
    }
}