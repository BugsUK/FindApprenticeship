namespace SFA.Apprenticeships.Application.UserProfile
{
    using System.Collections.Generic;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class UserProfileService : IUserProfileService
    {
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IProviderUserWriteRepository _providerUserWriteRepository;
        private readonly IAgencyUserReadRepository _agencyUserReadRepository;
        private readonly IAgencyUserWriteRepository _agencyUserWriteRepository;

        public UserProfileService(IProviderUserReadRepository providerUserReadRepository, IProviderUserWriteRepository providerUserWriteRepository, IAgencyUserReadRepository agencyUserReadRepository, IAgencyUserWriteRepository agencyUserWriteRepository)
        {
            _providerUserReadRepository = providerUserReadRepository;
            _providerUserWriteRepository = providerUserWriteRepository;
            _agencyUserReadRepository = agencyUserReadRepository;
            _agencyUserWriteRepository = agencyUserWriteRepository;
        }

        public ProviderUser GetProviderUser(string username)
        {
            return _providerUserReadRepository.Get(username);
        }

        public IEnumerable<ProviderUser> GetProviderUsers(string ukprn)
        {
            return _providerUserReadRepository.GetForProvider(ukprn);
        }

        public ProviderUser SaveUser(ProviderUser providerUser)
        {
            //Check if email is being updated and set pending, verification code, send email etc
            return _providerUserWriteRepository.Save(providerUser);
        }

        public AgencyUser GetAgencyUser(string username)
        {
            return _agencyUserReadRepository.Get(username);
        }

        public AgencyUser SaveUser(AgencyUser agencyUser)
        {
            return _agencyUserWriteRepository.Save(agencyUser);
        }
    }
}
