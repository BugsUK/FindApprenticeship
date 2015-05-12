namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Builder
{
    using System;
    using Users;

    public class UserBuilder
    {
        private readonly Guid _userId;
        private DateTime _dateCreated;
        private UserStatuses _userStatus;
        private string _activationCode;
        private DateTime? _activateCodeExpiry;

        public UserBuilder(Guid userId)
        {
            _userId = userId;
            _dateCreated = DateTime.Now;
        }

        public User Build()
        {
            var user = new User
            {
                DateCreated = _dateCreated,
                EntityId = _userId,
                Status = _userStatus,
                ActivationCode = _activationCode,
                ActivateCodeExpiry = _activateCodeExpiry
            };

            return user;
        }

        public UserBuilder WithDateCreated(DateTime dateCreated)
        {
            _dateCreated = dateCreated;
            return this;
        }

        public UserBuilder WithStatus(UserStatuses userStatus)
        {
            _userStatus = userStatus;
            return this;
        }

        public UserBuilder Activated(bool activated)
        {
            _userStatus = activated ? UserStatuses.Active : UserStatuses.PendingActivation;
            _activationCode = activated ? null : "ABC123";
            _activateCodeExpiry = activated ? (DateTime?) null : _dateCreated.AddDays(30);
            return this;
        }
    }
}
