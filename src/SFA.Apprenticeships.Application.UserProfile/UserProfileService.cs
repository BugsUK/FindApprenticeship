namespace SFA.Apprenticeships.Application.UserProfile
{
    using System.Collections.Generic;
    using System.Linq;
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

        public ProviderUser GetProviderUser(int id)
        {
            return _providerUserReadRepository.Get(id);
        }

        public ProviderUser GetProviderUser(string username)
        {
            return _providerUserReadRepository.Get(username);
        }

        public IEnumerable<ProviderUser> GetProviderUsers(string ukprn)
        {
            return _providerUserReadRepository.GetForProvider(ukprn);
        }

        public ProviderUser CreateProviderUser(ProviderUser providerUser)
        {
            return _providerUserWriteRepository.Create(providerUser);
        }

        public ProviderUser UpdateProviderUser(ProviderUser providerUser)
        {
            return _providerUserWriteRepository.Update(providerUser);
        }

        public AgencyUser GetAgencyUser(string username)
        {
            return _agencyUserReadRepository.Get(username);
        }

        public AgencyUser SaveUser(AgencyUser agencyUser)
        {
            return _agencyUserWriteRepository.Save(agencyUser);
        }

        public IEnumerable<Team> GetTeams()
        {
            //TODO: Get these from config or a repo once the design and full list has been agreed
            return new List<Team>
            {
                GetTeam("All", "All", true)
            };
        }

        private static Team GetTeam(string codeName, string name, bool isDefault = false)
        {
            return new Team
            {
                CodeName = codeName,
                Name = name,
                IsDefault = isDefault
            };
        }

        public IEnumerable<Role> GetRoles(string roleList)
        {
            //TODO: Get these from config or a repo once the design and full list has been agreed
            const string technicalAdviser = Role.CodeNameTechnicalAdviser;

            var roles = new List<Role>
            {
                GetRole(1, Role.CodeNameHelpdeskAdviser, "Helpdesk adviser"),
                GetRole(2, Role.CodeNameQAAdviser, "Vacancy reviewer", true)
                //GetRole(technicalAdvisor, "Technical adviser")
            };

            return roleList == "Serco" ? roles.Where(r => r.CodeName != technicalAdviser) : roles;
        }

        private static Role GetRole(int id, string codeName, string name, bool isDefault = false)
        {
            return new Role
            {
                CodeName = codeName,
                Name = name,
                IsDefault = isDefault
            };
        }
    }
}
