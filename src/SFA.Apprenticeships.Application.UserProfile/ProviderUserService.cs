namespace SFA.Apprenticeships.Application.UserProfile
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;

    public class ProviderUserService : IProviderUserService
    {
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IProviderUserWriteRepository _providerUserWriteRepository;
        
        public ProviderUserService(IProviderUserReadRepository providerUserReadRepository, IProviderUserWriteRepository providerUserWriteRepository)
        {
            _providerUserReadRepository = providerUserReadRepository;
            _providerUserWriteRepository = providerUserWriteRepository;
        }

        public ProviderUser GetProviderUser(string username)
        {
            return _providerUserReadRepository.Get(username);
        }

        public ProviderUser CreateProviderUser(ProviderUser providerUser)
        {
            //Throw error is already exixts.
            return null;
        }

        public ProviderUser UpdateProviderUser(ProviderUser providerUser)
        {
            //Check if email is being updated and set pending, vertification code, send email etc
            return null;
        }
    }
}
