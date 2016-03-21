﻿namespace SFA.Apprenticeships.Infrastructure.Common.CurrentUser
{
    using System.Threading;
    using SFA.Infrastructure.Interfaces;

    public class CurrentUserService : ICurrentUserService
    {
        public string CurrentUserName => Thread.CurrentPrincipal.Identity.Name;
    }
}