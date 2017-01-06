namespace SFA.Apprenticeships.Application.Provider.Strategies
{
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces;
    using Interfaces.Employers;

    public class GetVacancyOwnerRelationshipStrategy : IGetVacancyOwnerRelationshipStrategy
    {
        private readonly IEmployerService _employerService;
        private readonly ILogService _logService;
        private readonly IVacancyOwnerRelationshipReadRepository _vacancyOwnerRelationshipReadRepository;
        private readonly IVacancyOwnerRelationshipWriteRepository _vacancyOwnerRelationshipWriteRepository;

        public GetVacancyOwnerRelationshipStrategy(IEmployerService employerService, ILogService logService, IVacancyOwnerRelationshipReadRepository vacancyOwnerRelationshipReadRepository, IVacancyOwnerRelationshipWriteRepository vacancyOwnerRelationshipWriteRepository)
        {
            _employerService = employerService;
            _logService = logService;
            _vacancyOwnerRelationshipReadRepository = vacancyOwnerRelationshipReadRepository;
            _vacancyOwnerRelationshipWriteRepository = vacancyOwnerRelationshipWriteRepository;
        }

        public VacancyOwnerRelationship GetVacancyOwnerRelationship(int providerSiteId, string edsUrn, bool liveOnly = true)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            _logService.Debug("Calling Employer Service to get employer with EDSURN='{0}'.", edsUrn);

            var employer = _employerService.GetEmployer(edsUrn);

            return GetVacancyOwnerRelationship(providerSiteId, employer.EmployerId, liveOnly);
        }

        public VacancyOwnerRelationship GetVacancyOwnerRelationship(int providerSiteId, int employerId, bool liveOnly = true)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(employerId);

            _logService.Debug(
                $"Calling VacancyOwnerRelationshipReadRepository to get vacancy party for provider site with Id='{providerSiteId}' and employer with Id='{employerId}'.");

            var vacancyOwnerRelationship =
                _vacancyOwnerRelationshipReadRepository.GetByProviderSiteAndEmployerId(providerSiteId, employerId, liveOnly) ??
                new VacancyOwnerRelationship { ProviderSiteId = providerSiteId, EmployerId = employerId, StatusType = VacancyOwnerRelationshipStatusTypes.Active};

            return vacancyOwnerRelationship;
        }

        public bool IsADeletedVacancyOwnerRelationship(int providerSiteId, string edsUrn)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            _logService.Debug("Calling Employer Service to get employer with EDSURN='{0}'.", edsUrn);

            var employer = _employerService.GetEmployer(edsUrn);

            return IsADeletedVacancyOwnerRelationship(providerSiteId, employer.EmployerId);
        }

        public bool IsADeletedVacancyOwnerRelationship(int providerSiteId, int employerId)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(employerId);

            _logService.Debug(
                "Calling VacancyOwnerRelationshipReadRepository to check if the vacancy party has been deleted for provider site with Id='{0}' and employer with Id='{1}'.",
                providerSiteId, employerId);

            return _vacancyOwnerRelationshipReadRepository.IsADeletedVacancyOwnerRelationship(providerSiteId, employerId);
        }

        public void ResurrectVacancyOwnerRelationship(int providerSiteId, string edsUrn)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            _logService.Debug("Calling Employer Service to get employer with EDSURN='{0}'.", edsUrn);

            var employer = _employerService.GetEmployer(edsUrn);

            ResurrectVacancyOwnerRelationship(providerSiteId, employer.EmployerId);
        }

        public void ResurrectVacancyOwnerRelationship(int providerSiteId, int employerId)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(employerId);

            _logService.Debug(
                "Calling VacancyOwnerRelationshipWriteRepository to resurrect the vacancy party for provider site with Id='{0}' and employer with Id='{1}'.",
                providerSiteId, employerId);

            _vacancyOwnerRelationshipWriteRepository.ResurrectVacancyOwnerRelationship(providerSiteId, employerId);
        }
    }
}