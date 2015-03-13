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

        public User Build()
        {
            var user = new User
            {
                EntityId = _userId,
                Status = _userStatus
            };

            return user;
        }

        public UserBuilder WithStatus(UserStatuses userStatus)
        {
            _userStatus = userStatus;
            return this;
        }

        public UserBuilder Activated(bool activated)
        {
            _userStatus = activated ? UserStatuses.Active : UserStatuses.Inactive;
            return this;
        }
    }
}
