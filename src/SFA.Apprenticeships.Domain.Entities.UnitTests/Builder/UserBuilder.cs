namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Builder
{
    using System;
    using Users;

    public class UserBuilder
    {
        private readonly Guid _userId;
        private UserStatuses _userStatus;

        public UserBuilder(Guid userId)
        {
            _userId = userId;
        }

        public UserBuilder WithStatus(UserStatuses userStatus)
        {
            _userStatus = userStatus;
            return this;
        }

        public User Build()
        {
            return new User
            {
                EntityId = _userId,
                Status = _userStatus
            };
        }
    }
}
