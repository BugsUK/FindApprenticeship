namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;
    using Entities;

    public class UserCredentials : BaseEntity<Guid>
    {
        public string PasswordHash { get; set; }
    }
}
